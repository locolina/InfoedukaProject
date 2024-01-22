using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime DatePosted { get; set; }

    public DateTime? DateExpires { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
public class BLComment
{
    public int CommentId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime DatePosted { get; set; }

    public DateTime? DateExpires { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }




}

public class MappingComment
{
    public static IEnumerable<BLComment> MapToBL(IEnumerable<Comment> comments) =>
   comments.Select(x => MapToBL(x));

    public static BLComment MapToBL(Comment comment) =>
        new BLComment
        {
            CommentId = comment.CommentId,
            Title = comment.Title,
            Content = comment.Content,
            DatePosted = comment.DatePosted,
            DateExpires = comment.DateExpires,
            IsActive = comment.IsActive,
            UserId = comment.UserId,
            ClassId = comment.ClassId,

        };

    public static IEnumerable<Comment> MapToDAL(IEnumerable<BLComment> blComments)
        => blComments.Select(x => MapToDAL(x));

    public static Comment MapToDAL(BLComment comment) =>
        new Comment
        {
            CommentId = comment.CommentId,
            Title = comment.Title,
            Content = comment.Content,
            DatePosted = DateTime.UtcNow.AddHours(1),
            DateExpires = comment.DateExpires,
            IsActive = comment.IsActive,
            UserId = comment.UserId,
            ClassId = comment.ClassId,



        };
}
