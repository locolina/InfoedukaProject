using InfoedukaMVC.Models;
using InfoedukaMVC.Models.DTO;

namespace InfoedukaMVC.Mappers;

public class AppUserMapper
{
    public static AppUserDTO MapToDTO(AppUser appUser)
    {
        return new AppUserDTO
        {
            UserId = appUser.UserId,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            UserName = appUser.UserName,
            Pass = appUser.Pass,
            UserTypeId = appUser.UserTypeId,
            IsActive = appUser.IsActive
        };
    }

    public static AppUser MapToDAL(AppUserDTO appUserDto)
    {
        return new AppUser()
        {
            UserId = appUserDto.UserId,
            FirstName = appUserDto.FirstName,
            LastName = appUserDto.LastName,
            UserName = appUserDto.UserName,
            Pass = appUserDto.Pass,
            UserTypeId = appUserDto.UserTypeId,
            IsActive = appUserDto.IsActive
        };
    }
}