using Library_Management_System.DTOs.Registration;
using Library_Management_System.DTOs.User;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IAuthenticationService
    {
        Task registerUserAsync(RegisterUserDTO user);
        Task<string> loginAsync(LoginDTO loginUser);
        Task<bool> logoutAsync(int userId);
    }
}
