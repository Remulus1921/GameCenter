using GameCenter.Core.Repositories.CommentsRepository;
using GameCenter.Core.Repositories.GameRepository;
using GameCenter.Core.Repositories.PlatformRepository;
using GameCenter.Core.Repositories.PostsRepository;
using GameCenter.Core.Repositories.RatesRepository;
using GameCenter.Core.Services.AuthService;
using GameCenter.Core.Services.CommentsService;
using GameCenter.Core.Services.GameService;
using GameCenter.Core.Services.PlatformService;
using GameCenter.Core.Services.PostsService;
using GameCenter.Core.Services.RatesService;
using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register repositories
builder.Services.AddScoped<IGamesRepository, GamesRepository>();
builder.Services.AddScoped<IPlatformsRepository, PlatformsRepository>();
builder.Services.AddScoped<IRatesRepository, RatesRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddScoped<IPlatformsService, PlatformsService>();
builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IRatesService, RatesService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IPostsService, PostsService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<GameCenterUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(options => options.AddPolicy("FrontEnd", policy =>
{
    policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var servPro = services.GetService<UserManager<GameCenterUser>>();

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var adminPass = builder.Configuration.GetValue<string>("AdminPass");
    await SeedData.CreateRole(services);
    await SeedData.Initialize(services, servPro!, adminPass!);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("FrontEnd");

app.MapControllers();

app.Run();
