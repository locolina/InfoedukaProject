using Microsoft.AspNetCore.Mvc.Rendering;
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

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
  
    
}
