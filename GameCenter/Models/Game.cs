using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models
{

    [Table("Games")]
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string GameType { get; set; }
        public string Description { get; set; }
        public string Studio { get; set; }
        public string Rating { get; set; }
        public int Capacity { get; set; }
        public string ImagePath { get; set; }
        public ICollection<Rate> GameRates { get; set; }
        public ICollection<Comment> GameComments { get; set; }
        public ICollection<Platform> Platforms { get; set; }
    }
}