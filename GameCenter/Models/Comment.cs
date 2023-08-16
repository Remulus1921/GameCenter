using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Comments")]
public class Comment
{
    public int Id { get; set; }
    public string CommentContent { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    public Game Game { get; set; }
    public int GId { get; set; }
    public IdentityUser User { get; set; }
    public int UId { get; set; }
    public Comment? Reply { get; set; }
    public int? ParentId { get; set; }


}
