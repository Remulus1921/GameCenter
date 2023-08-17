using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Comments")]
public class Comment
{
    public Guid Id { get; set; }
    public string CommentContent { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    public Game Game { get; set; }
    public Guid GId { get; set; }
    public IdentityUser User { get; set; }
    public Guid UId { get; set; }
    public Comment? Reply { get; set; }
    public Guid? ParentId { get; set; }
    public ICollection<Comment> Replies { get; set; }

}
