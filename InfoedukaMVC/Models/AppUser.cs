using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class AppUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public int UserTypeId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<UserClassMapping> UserClassMappings { get; set; } = new List<UserClassMapping>();

    public virtual UserType UserType { get; set; } = null!;
}
