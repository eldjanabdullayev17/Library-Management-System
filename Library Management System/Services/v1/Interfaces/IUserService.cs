using Library_Management_System.DTOs.User;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IUserService
    {

        // Bütün istifadəçiləri əldə edir.
        Task<IEnumerable<UserDTO>> getAllUsersAsync();

        // Yeni istifadəçi əlavə edir.
        Task<AddUserDTO> addUserAsync(AddUserDTO user);

        // Mövcud istifadəçinin məlumatlarını yeniləyir.
        Task<bool> updateUserAsync(int userId, UpdateUserDTO newUser);

        // İstifadəçini silir.
        Task<bool> deleteUserAsync(int userId);

        // İstifadəçi hesabını deaktiv edir.
        Task<bool> deactivateUserAsync(int userId);

        // Müəyyən istifadəçi haqqında məlumatları əldə edir.
        Task<UserDTO> getUserByIdAsync(int userId);

        // İstifadəçi adına əsasən istifadəçi məlumatlarını əldə edir.
        Task<IEnumerable<UserDTO>> getUserByUsername(string username);

        // Müəyyən rola malik istifadəçiləri əldə edir.
        Task<IEnumerable<GetUsersByRoleDTO>> getUsersByRoleAsync(int roleId);


    }
}
