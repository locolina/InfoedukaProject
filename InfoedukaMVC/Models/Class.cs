using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<UserClassMapping> UserClassMappings { get; set; } = new List<UserClassMapping>();
}
