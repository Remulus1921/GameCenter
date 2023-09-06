using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models
{

    [Table("Rates")]
    public class Rate
    {
        public Guid Id { get; set; }
        public int GameRate { get; set; }
        public Guid GameId { get; set; }
        public Game? Game { get; set; }
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }

    }
}