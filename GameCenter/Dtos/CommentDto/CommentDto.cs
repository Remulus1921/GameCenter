﻿namespace GameCenter.Dtos.CommentDto;

public class CommentDto
{
    public Guid Id { get; set; }
    public string CommentContent { get; set; }
    public string UserName { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    public Guid? ParentId { get; set; }
}
