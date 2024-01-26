using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class UserCourseMapping
{
    public int UserCourseMappingId { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
