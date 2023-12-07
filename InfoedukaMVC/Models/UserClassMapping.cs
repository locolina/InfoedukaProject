using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class UserClassMapping
{
    public int UserClassMappingId { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
