using InfoedukaMVC.Models;

namespace InfoedukaMVC.Services
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SignIn(int userId, string userName, int isAdmin)
        {
            _httpContextAccessor.HttpContext?.Session.SetInt32("UserId", userId);
            _httpContextAccessor.HttpContext?.Session.SetString("UserName", userName);
            _httpContextAccessor.HttpContext?.Session.SetInt32("IsAdmin", isAdmin);
            _httpContextAccessor.HttpContext?.Session.SetString("IsLoggedIn", "true");
        }

        public void SignOut()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }

        public AppUser GetCurrentUser(System.Security.Claims.ClaimsPrincipal user)
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            var userName = _httpContextAccessor.HttpContext?.Session.GetString("UserName");
            var isAdmin = _httpContextAccessor.HttpContext?.Session.GetInt32("IsAdmin");

            if (userId.HasValue && !string.IsNullOrEmpty(userName) && isAdmin.HasValue)
            {
                return new AppUser { UserId = userId.Value, UserName = userName, UserTypeId = isAdmin.Value };
            }

            return null;
        }

      
    }

}
