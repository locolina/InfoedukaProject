using InfoedukaMVC.Models;
using InfoedukaMVC.Models.DTO;

namespace InfoedukaMVC.Mappers;

public class CommentMapper
{
    public static CommentsDTO MapToDTO(Comment comment)
    {
        return new CommentsDTO
        {
            CommentId = comment.CommentId,
            Title = comment.Title,
            Content = comment.Content,
            DatePosted = comment.DatePosted,
            DateExpires = comment.DateExpires,
            IsActive = comment.IsActive,
            UserId = comment.UserId,
            CourseId = comment.CourseId
        };
    }

    public static Comment MapToDAL(CommentsDTO comment)
    {
        return new Comment
        {
            CommentId = comment.CommentId,
            Title = comment.Title,
            Content = comment.Content,
            DatePosted = DateTime.UtcNow,
            DateExpires = comment.DateExpires,
            IsActive = comment.IsActive,
            UserId = comment.UserId,
            CourseId = comment.CourseId
        };
    }
}