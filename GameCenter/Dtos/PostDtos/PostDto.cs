using GameCenter.Dtos.FileDtos;

namespace GameCenter.Dtos.PostDto
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public FileDto? Image { get; set; }
        public string UserName { get; set; }
        public List<string> Platforms { get; set; }
    }
}