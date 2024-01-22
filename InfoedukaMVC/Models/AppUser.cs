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
public class BLAppUser
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Pass { get; set; }

    public int UserTypeId { get; set; }

    public bool IsActive { get; set; }


}

public class MappingAppUser
{

    public static IEnumerable<BLAppUser> MapToBL(IEnumerable<AppUser> users) =>
    users.Select(x => MapToBL(x));

    public static BLAppUser MapToBL(AppUser user) =>
        new BLAppUser
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            UserTypeId = user.UserTypeId,
            Pass = user.Pass,
        };

    public static IEnumerable<AppUser> MapToDAL(IEnumerable<BLAppUser> blUsers)
        => blUsers.Select(x => MapToDAL(x));

    public static AppUser MapToDAL(BLAppUser user) =>
        new AppUser
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            UserTypeId = user.UserTypeId,
            Pass = user.Pass,


        };
}
