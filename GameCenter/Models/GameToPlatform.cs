namespace GameCenter.Models;
public class GameToPlatform
{
    public Guid Id { get; set; }
    public Game Game { get; set; }
    public Guid GId { get; set; }
    public Platform Platform { get; set; }
    public Guid PId { get; set; }
}
