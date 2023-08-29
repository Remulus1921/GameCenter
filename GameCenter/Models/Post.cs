using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;

[Table("Posts")]
public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public string? ImagePath { get; set; }
    public IdentityUser User { get; set; }
    public string UserId { get; set; }
    public ICollection<Platform> Platforms { get; set; }
}
