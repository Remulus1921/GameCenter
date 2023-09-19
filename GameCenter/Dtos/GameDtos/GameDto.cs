using GameCenter.Dtos.CommentDtos;

namespace GameCenter.Dtos.GameDto
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string GameType { get; set; }
        public string Description { get; set; }
        public string Studio { get; set; }
        public string Rating { get; set; }
        public int Capacity { get; set; }
        public string ImagePath { get; set; }
        public List<string> PlatformsName { get; set; }
        public List<int>? GameRates { get; set; }
        public List<CommentDto>? Comments { get; set; }
    }
}
