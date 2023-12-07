using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string? UserTypeName { get; set; }

    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
}
