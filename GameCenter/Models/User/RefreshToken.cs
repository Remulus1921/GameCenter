using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models.User
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
        public bool IsValid { get; set; } = true;
        public RefreshToken? Parent { get; set; }
        public int? ParentId { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
    }
}
