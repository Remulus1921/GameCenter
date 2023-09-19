namespace GameCenter.Dtos.PostDto
{

    public class PostSmallDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string? ImagePath { get; set; }
        public string UserName { get; set; }
        public List<string> Platforms { get; set; }

    }
}