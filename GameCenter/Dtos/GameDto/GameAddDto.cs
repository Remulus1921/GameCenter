namespace GameCenter.Dtos.GameDto;

public class GameAddDto
{
    public string Name { get; set; }
    public string GameType { get; set; }
    public string Description { get; set; }
    public string Studio { get; set; }
    public string Rating { get; set; }
    public int Capacity { get; set; }
    public string ImagePath { get; set; }
    public List<string> Platforms { get; set; }
}
