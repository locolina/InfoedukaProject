namespace InfoedukaMVC.Models.DTO;

public class AppUserDTO
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Pass { get; set; }

    public int UserTypeId { get; set; }

    public bool IsActive { get; set; }
}