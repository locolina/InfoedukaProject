namespace InfoedukaMVC.Models.DTO;

public class CommentsDTO
{
    public int CommentId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime DatePosted { get; set; }

    public DateTime? DateExpires { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }
}