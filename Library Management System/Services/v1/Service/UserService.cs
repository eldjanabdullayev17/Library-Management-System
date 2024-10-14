using Library_Management_System.DTOs.User;
using Library_Management_System.Models;
using Library_Management_System.Exceptions;
using Microsoft.EntityFrameworkCore;
using Library_Management_System.Validation;
using Library_Management_System.Services.v1.Interfaces;


namespace Library_Management_System.Services.v1.Service
{
    public class UserService : IUserService
    {
        private readonly OnlineLibraryManagementSystemContext _context;

        public UserService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddUserDTO> addUserAsync(AddUserDTO user)
        {
            if (string.IsNullOrEmpty(user.Firstname) || string.IsNullOrWhiteSpace(user.Firstname))
            {
                throw new LibraryManagementSystemException("İstifadəçinin adı düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(user.Lastname) || string.IsNullOrWhiteSpace(user.Lastname))
            {
                throw new LibraryManagementSystemException("İstifadəçinin soyadı düzgün verilməyib.");
            }

            var usernameExists = await _context.Users.AnyAsync(u => u.Username == user.Username);

            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrWhiteSpace(user.Username) || usernameExists)
            {
                throw new LibraryManagementSystemException("UserName düzgün verilməyib,ya da bu username mövcüddur.");
            }

            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrWhiteSpace(user.Password) ||
                user.Password.Length < 8
                )
            {
                throw new LibraryManagementSystemException("Password en azi 8 simvol olmalidir.");
            }

            IsValid.IsValidId(user.RoleId);

            var role = await _context.Roles.AnyAsync(r => r.Id == user.RoleId);

            if (!role)
            {
                throw new LibraryManagementSystemException($"Id-si {user.RoleId} olan rol yoxdur.");
            }

            var superAdminId = await _context.Roles.SingleOrDefaultAsync(s => s.RoleName.ToLower() == "SuperAdmin".ToLower());

            if (user.RoleId == superAdminId.Id)
            {
                throw new LibraryManagementSystemException("Əlavə edə bilməzsiniz!");
            }

            string hashPassword = Sha256Hash.HashData(user.Password);

            var newUser = new User
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Password = hashPassword,
                RoleId = user.RoleId,
                Active = true
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> deactivateUserAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user.Role.RoleName.ToLower() == "SuperAdmin".ToLower())
            {
                throw new LibraryManagementSystemException("SuperAdmin deaktiv oluna bilməz!");
            }

            if (user != null && user.Active != false)
            {
                user.Active = false;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> deleteUserAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users
                            .Include(x => x.Role)
                            .SingleOrDefaultAsync(x => x.Id == userId);

            if (user.Role.RoleName.ToLower() == "SuperAdmin".ToLower())
            {
                throw new LibraryManagementSystemException("SuperAdmin silinə bilməz!");
            }

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserDTO>> getAllUsersAsync()
        {
            return await _context.Users
                .Include(x => x.Role)
                .Where(x => x.Active && x.Role.RoleName.ToLower() != "SuperAdmin".ToLower())
                .Select(x => new UserDTO
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Username = x.Username,
                    Password = x.Password,
                    Role = x.Role.RoleName,
                    Active = x.Active
                }).ToListAsync();
        }

        public async Task<UserDTO> getUserByIdAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == userId && x.Active);


            /*if (user.Role.RoleName.ToLower() == "SuperAdmin".ToLower())
			{
				throw new LibraryManagementSystemException("SuperAdmin!");
			}*/

            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Username = user.Username,
                    Password = user.Password,
                    Role = user.Role.RoleName,
                    Active = user.Active
                };
            }
            else
            {
                throw new LibraryManagementSystemException("İstifadəçi tapılmadı.");
            }
        }

        public async Task<IEnumerable<GetUsersByRoleDTO>> getUsersByRoleAsync(int roleId)
        {
            IsValid.IsValidId(roleId);

            var role = await _context.Roles.AnyAsync(r => r.Id == roleId);

            if (!role)
            {
                throw new LibraryManagementSystemException("Belə role yoxdur.");
            }

            var users = await _context.Users.
                Include(x => x.Role)
                .Where(x => x.RoleId == roleId).ToListAsync();

            if (users.Any())
            {
                return users.Select(x => new GetUsersByRoleDTO
                {
                    UserId = x.Id,
                    Role = x.Role.RoleName,
                    UserFullName = $"{x.Firstname} {x.Lastname}"
                }).ToList();
            }
            else
            {
                throw new LibraryManagementSystemException("Bu rolda heç bir istifadəçi yoxdur.");
            }
        }

        public async Task<IEnumerable<UserDTO>> getUserByUsername(string username)
        {
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrWhiteSpace(username))
            {
                return await _context.Users
                    .Include(x => x.Role)
                    .Where(user => user.Username.ToLower() == username.ToLower() && user.Active)
                    .Select(x => new UserDTO
                    {
                        Id = x.Id,
                        Firstname = x.Firstname,
                        Lastname = x.Lastname,
                        Username = x.Username,
                        Password = x.Password,
                        Role = x.Role.RoleName,
                        Active = x.Active
                    }).ToListAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> updateUserAsync(int userId, UpdateUserDTO newUser)
        {
            IsValid.IsValidId(userId);
            IsValid.IsValidId(newUser.RoleId);

            if (string.IsNullOrEmpty(newUser.Firstname) || string.IsNullOrWhiteSpace(newUser.Firstname))
            {
                throw new LibraryManagementSystemException("İstifadəçinin adı düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(newUser.Lastname) || string.IsNullOrWhiteSpace(newUser.Lastname))
            {
                throw new LibraryManagementSystemException("İstifadəçinin soyadı düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Username))
            {
                throw new LibraryManagementSystemException("UserName düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(newUser.Password) || string.IsNullOrWhiteSpace(newUser.Password) ||
                newUser.Password.Length < 8)
            {
                throw new LibraryManagementSystemException("Password düzgün verilməyib.");
            }

            var user = await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == userId && x.Active != false);

            if (user.Role.RoleName.ToLower() == "SuperAdmin".ToLower())
            {
                throw new LibraryManagementSystemException("Yenilik etmək istədiyiniz istifadəçi Superadmindir.Ona görə dəyişiklik edə bilməzsiniz.");
            }

            string hashPassword = Sha256Hash.HashData(newUser.Password);

            if (user != null)
            {

                user.Firstname = newUser.Firstname;
                user.Lastname = newUser.Lastname;
                user.Username = newUser.Username;
                user.Password = hashPassword;
                user.RoleId = newUser.RoleId;
                user.Active = true;

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
