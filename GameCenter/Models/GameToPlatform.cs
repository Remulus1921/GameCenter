namespace GameCenter.Models;
public class GameToPlatform
{
    public Guid Id { get; set; }
    public Game Game { get; set; }
    public Guid GameId { get; set; }
    public Platform Platform { get; set; }
    public Guid PlatformId { get; set; }
}
