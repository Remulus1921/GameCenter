namespace GameCenter.Models;

public class PostToPlatform
{
    public Guid Id { get; set; }
    public Post Post { get; set; }
    public Guid PostId { get; set; }
    public Platform Platform { get; set; }
    public Guid Pid { get; set; }
}
