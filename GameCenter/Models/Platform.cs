using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Platforms")]
public class Platform
{
    public Guid Id { get; set; }
    public string PlatformName { get; set; }
    public ICollection<GameToPlatform> Games { get; set; }
}
