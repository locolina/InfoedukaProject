using System;
using System.Collections.Generic;

namespace InfoedukaMVC.Models;

public partial class VComment
{
    public string Title { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime DatePosted { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}
