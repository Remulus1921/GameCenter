namespace GameCenter.Models;
public class GameToPlatform
{
    public int Id { get; set; }
    public Game Game { get; set; }
    public int GId { get; set; }
    public Platform Platform { get; set; }
    public int PId { get; set; }
}
