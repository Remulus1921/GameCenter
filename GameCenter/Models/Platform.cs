using System.ComponentModel.DataAnnotations.Schema;

namespace GameCenter.Models;
[Table("Platforms")]
public class Platform
{
    public int Id { get; set; }
    public string PlatformName { get; set; }
}
