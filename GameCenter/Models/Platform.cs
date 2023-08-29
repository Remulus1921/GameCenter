using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Platforms")]
public class Platform
{
    public Guid Id { get; set; }
    public string PlatformName { get; set; }
    public ICollection<Game> Games { get; set; }
    public ICollection<Post> Posts { get; set; }
}
