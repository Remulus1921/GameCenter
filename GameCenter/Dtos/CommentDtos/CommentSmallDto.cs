namespace GameCenter.Dtos.CommentDtos
{

    public class CommentSmallDto
    {
        public Guid? Id { get; set; }
        public string CommentContent { get; set; }
        public Guid? ParentId { get; set; }
    }
}
