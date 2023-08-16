using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Rates")]
public class Rate
{
    public int Id { get; set; }
    public int GameRate { get; set; }
    public Game Game { get; set; }
    public int GId { get; set; }
    public IdentityUser User { get; set; }
    public int UId { get; set; }

}
