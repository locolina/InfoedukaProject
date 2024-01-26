using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<UserCourseMapping> UserCourseMappings { get; set; } = new List<UserCourseMapping>();
}
