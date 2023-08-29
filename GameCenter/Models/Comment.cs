using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Comments")]
public class Comment
{
    public Guid Id { get; set; }
    public string CommentContent { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime ModificationDate { get; set; }
    public Game Game { get; set; }
    public Guid GameId { get; set; }
    public IdentityUser User { get; set; }
    public string UserId { get; set; }
    public Comment? Parent { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<Comment>? Replies { get; set; }

}
