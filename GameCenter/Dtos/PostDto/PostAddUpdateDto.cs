namespace GameCenter.Dtos.PostDto
{

    public class PostAddUpdateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
        public List<string> Platforms { get; set; }
    }
}