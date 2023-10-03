using GameCenter.Dtos.FileDtos;

namespace GameCenter.Dtos.GameDto
{

    public class GameSmallDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string GameType { get; set; }
        public string Rating { get; set; }
        public FileDto Image { get; set; }
    }
}