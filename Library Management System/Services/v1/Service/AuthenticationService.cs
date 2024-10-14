using Library_Management_System.DTOs.Registration;
using Library_Management_System.DTOs.User;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Management_System.Services.v1.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        private readonly GenerateJwtTokenService _generateJwtToken;

        public AuthenticationService(OnlineLibraryManagementSystemContext context, GenerateJwtTokenService generateJwtToken)
        {
            _context = context;
            _generateJwtToken = generateJwtToken;

        }

        public async Task<string> loginAsync(LoginDTO loginUser)
        {
            string hashPassword = Sha256Hash.HashData(loginUser.Password);

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginUser.UserName &&
                                           u.Password == hashPassword);

            if (user is null || !user.Active)
            {
                throw new LibraryManagementSystemException("İstifadəçi tapılmadı ve ya aktiv deyil.");
            }

            var userlogin = new UserLogin
            {
                UserId = user.Id,
                UserLoginDate = DateTime.Now
            };

            await _context.UserLogins.AddAsync(userlogin);
            await _context.SaveChangesAsync();

            var token = _generateJwtToken.GenerateJwtToken(user);
            return token;
        }

        public async Task<bool> logoutAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users.FindAsync(userId);

            if (user == null || !user.Active)
            {
                return false;
            }

            user.Active = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task registerUserAsync(RegisterUserDTO newUser)
        {
            if (string.IsNullOrEmpty(newUser.Firstname) || string.IsNullOrWhiteSpace(newUser.Firstname))
            {
                throw new LibraryManagementSystemException("İstifadəçinin adı düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(newUser.Lastname) || string.IsNullOrWhiteSpace(newUser.Lastname))
            {
                throw new LibraryManagementSystemException("İstifadəçinin soyadı düzgün verilməyib.");
            }

            var usernameExists = await _context.Users.AnyAsync(u => u.Username == newUser.Username);

            if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Username) || !usernameExists)
            {
                throw new LibraryManagementSystemException("UserName düzgün verilməyib,ya da bu username mövcüddur.");
            }

            if (string.IsNullOrEmpty(newUser.Password) || string.IsNullOrWhiteSpace(newUser.Password) ||
                newUser.Password.Length < 8
                )
            {
                throw new LibraryManagementSystemException("Password en azi 8 simvol olmalidir.");
            }

            var role = await _context.Roles.AnyAsync(r => r.Id == newUser.RoleId);

            if (!role)
            {
                throw new LibraryManagementSystemException($"Id-si {newUser.RoleId} olan rol yoxdur.");
            }

            var superAdminId = await _context.Roles.SingleOrDefaultAsync(s => s.RoleName.ToLower() == "SuperAdmin".ToLower());

            if (newUser.RoleId == superAdminId.Id)
            {
                throw new LibraryManagementSystemException("Əlavə edə bilməzsiniz!");
            }

            string hashPassword = Sha256Hash.HashData(newUser.Password);

            var user = new User
            {
                Firstname = newUser.Firstname,
                Lastname = newUser.Lastname,
                Username = newUser.Username,
                Password = hashPassword,
                RoleId = newUser.RoleId,
                Active = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
