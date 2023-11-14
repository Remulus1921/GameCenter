using GameCenter.Data;
using GameCenter.Data.UnitOfWork;
using GameCenter.Dtos.FileDtos;
using GameCenter.Dtos.PostDto;
using GameCenter.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCenter.Core.Services.PostsService
{

    public class PostsService : IPostsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<GameCenterUser> _userManager;
        private readonly string _uploadsFolder;

        public PostsService(IUnitOfWork unitOfWork, UserManager<GameCenterUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "Resources/Images");
        }

        public async Task<bool> AddPost(PostAddUpdateDto post, string email)
        {
            string uniqueName = string.Empty;
            if (post.Image != null)
            {
                uniqueName = this.AddFile(post.Image);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var platforms = new List<Platform>();
            foreach (var platformName in post.Platforms)
            {
                var platform = await _unitOfWork.Platforms.GetByName(platformName);
                if (platform == null)
                    continue;
                platforms.Add(platform);
            }

            var newPost = new Post
            {
                Title = post.Title,
                Content = post.Content,
                ImageName = post.Image != null ? uniqueName : null,
                User = user,
                Platforms = platforms
            };

            await _unitOfWork.Posts.Add(newPost);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<PostDto?> GetPost(Guid postId)
        {
            var post = await _unitOfWork.Posts.GetById(postId);
            if (post == null)
            {
                return null;
            }

            var postDto = new PostDto
            {
                Title = post.Title,
                Content = post.Content,
                Created = post.Created,
                Modified = post.Modified,
                Image = post.ImageName != null ? this.FindFile(post.ImageName) : null,
                UserName = post.User.UserName,
                Platforms = post.Platforms.Select(p => p.PlatformName).ToList(),
            };

            return postDto;
        }

        public async Task<List<PostSmallDto>?> GetPosts()
        {
            var posts = await _unitOfWork.Posts.All();

            if (posts == null)
            {
                return null;
            }

            List<PostSmallDto> postsList = new List<PostSmallDto>();

            foreach (var post in posts)
            {
                postsList.Add(new PostSmallDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Created = post.Created,
                    Modified = post.Modified,
                    Image = post.ImageName != null ? this.FindFile(post.ImageName) : null,
                    UserName = post.User.UserName!,
                    Platforms = post.Platforms.Select(p => p.PlatformName).ToList(),
                });
            }

            return postsList;
        }

        public async Task<bool> RemovePost(Guid postId)
        {
            var post = await _unitOfWork.Posts.GetById(postId);
            if (post == null)
            {
                return false;
            }

            if (post.ImageName != null)
            {
                this.DeleteFile(post.ImageName);
            }

            await _unitOfWork.Posts.Delete(post);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdatePost(Guid postId, PostAddUpdateDto post)
        {
            var postExists = await _unitOfWork.Posts.GetById(postId);

            if (postExists == null)
            {
                return false;
            }

            var platforms = new List<Platform>();
            foreach (var platformName in post.Platforms)
            {
                var platform = await _unitOfWork.Platforms.GetByName(platformName);
                if (platform == null)
                    continue;
                platforms.Add(platform);
            }

            string uniqueName = string.Empty;

            if (post.Image != null && postExists.ImageName != null)
            {
                this.DeleteFile(postExists.ImageName);
                uniqueName = this.AddFile(post.Image);
            }
            else if (post.Image != null && postExists.ImageName == null)
            {
                uniqueName = this.AddFile(post.Image);
            }

            postExists.Title = post.Title;
            postExists.Content = post.Content;
            postExists.Modified = DateTime.Now;
            postExists.Platforms = platforms;
            postExists.ImageName = post.Image != null ? uniqueName : null;

            await _unitOfWork.Posts.Update(postExists);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        private void DeleteFile(string fileName)
        {
            string[] matchingFiles = Directory.GetFiles(_uploadsFolder, fileName + ".jpg");

            if (matchingFiles.Any())
            {
                File.Delete(matchingFiles.First());
            }
            else
            {
                throw new Exception($"File with provided file name: \"{fileName}\" doesnt exist");
            }
        }

        private string AddFile(IFormFile file)
        {
            string uniqueName = Guid.NewGuid().ToString();

            string fileExtention = Path.GetExtension(file.FileName);

            if (fileExtention == String.Empty)
            {
                fileExtention = ".jpg";
            }

            string path = Path.Combine(_uploadsFolder, uniqueName + fileExtention);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return uniqueName;
        }

        private FileDto FindFile(string fileName)
        {
            string[] matchingFile = Directory.GetFiles(_uploadsFolder, fileName + ".jpg");

            if (matchingFile.Any())
            {
                byte[] fileContent = File.ReadAllBytes(matchingFile.First());
                string content = Convert.ToBase64String(fileContent);

                FileDto file = new FileDto()
                {
                    FileContent = content,
                    FileName = fileName,
                    FileType = "image/jpeg",
                };
                return file;
            }
            else
            {
                throw new Exception($"File with provided file name: \"{fileName}\" doesnt exist");
            }
        }
    }
}
