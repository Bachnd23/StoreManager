using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using COCOApp.Helpers;
namespace COCOApp.Services
{
    public class UserService : StoreManagerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public List<User> GetUsers()
        {
            var query = _context.Users.AsQueryable();
            query = query.Include(u => u.SellerDetail);
            return query.ToList();
        }
        public List<User> GetUsers(string nameQuery, int pageNumber, int pageSize)
        {
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(pageNumber, 1);

            var query = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Username.Contains(nameQuery));
            }
            query = query.OrderByDescending(p => p.Id);
            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public int GetTotalUsers(string nameQuery)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(nameQuery))
            {
                query = query.Where(c => c.Username.Contains(nameQuery));
            }
            return query.Count();
        }

        public void AddUser(User user)
        {
            // Check if the email or username already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new ArgumentException("Email is already in use.");
            }

            if (_context.Users.Any(u => u.Username == user.Username))
            {
                throw new ArgumentException("Username is already in use.");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(int userId, User user)
        {
            // Find the existing user in the database
            var existingUser = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }
            User userByName=GetUserByUsername(user.Username);
            User userByEmail=GetUserByEmail(user.Email);
/*            Debug.WriteLine(userByName.Id + "," + userByEmail.Id + "," + user.Id);*/
            if (userByEmail!=null&&userByName!=null&&(userByName.Id!=user.Id||userByEmail.Id!=user.Id))
            {
                throw new ArgumentException("Dupplicated name or email.");
            }
            // Update user properties
            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.UpdatedAt = user.UpdatedAt;

            // Save changes to the database
            _context.SaveChanges();
        }
        public void UpdateUserPassword(int userId, String password)
        {
            // Find the existing user in the database
            var existingUser = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }

            // Update user properties
            existingUser.Password = password;

            // Save changes to the database
            _context.SaveChanges();
        }

        public User GetUserByNameAndPass(string username, string password)
        {
            var user = _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefault(u => u.Username == username);

            if (user != null && user.Status == true && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null; // User not found or password incorrect
        }
        public User GetUserById(int userId)
        {
            var user = _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return user;
            }

            return null; // User not found 
        }
        public User GetActiveUserByEmail(string email)
        {
            var user = _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefault(u => u.Email == email);

            if (user != null && user.Status == true)
            {
                return user;
            }

            return null; // User not found 
        }
        public User GetUserByUsername(string username)
        {
            var user = _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                return user;
            }

            return null; // User not found 
        }
        public User GetUserByEmail(string email)
        {
            var user = _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                return user;
            }

            return null; // User not found 
        }
        public async Task UpdateUserPasswordResetTokenAsync(string email)
        {
            var user = await _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                // Generate a new password reset token (this is just an example, you should implement a proper token generation mechanism)
                user.ResetPasswordToken = Guid.NewGuid().ToString();

                // Save the changes asynchronously
                await _context.SaveChangesAsync();

                // Set the expiration time in a cookie
                var expirationTime = DateTime.UtcNow.AddHours(24);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure the cookie is only sent over HTTPS
                    Expires = expirationTime
                };

                // Set both the token and the expiration time in cookies
                _httpContextAccessor.HttpContext.Response.Cookies.Append("PasswordResetToken", user.ResetPasswordToken, cookieOptions);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("PasswordResetTokenExpiration", expirationTime.ToString("o"), cookieOptions);

                Console.WriteLine($"Password reset token updated for: {email}");
            }
            else
            {
                Console.WriteLine($"No user found with email: {email}");
            }
        }

        public async Task<bool> CheckPasswordResetTokenAsync(string email)
        {
            var tokenExpiration = _httpContextAccessor.HttpContext.Request.Cookies["PasswordResetTokenExpiration"];
            var tokenFromCookie = _httpContextAccessor.HttpContext.Request.Cookies["PasswordResetToken"];

            if (tokenExpiration != null && tokenFromCookie != null)
            {
                DateTime expirationTime;
                if (DateTime.TryParse(tokenExpiration, out expirationTime))
                {
                    if (DateTime.UtcNow <= expirationTime)
                    {
                        // Fetch the user from the database using the email
                        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                        if (user != null && user.ResetPasswordToken == tokenFromCookie)
                        {
                            Console.WriteLine("Password reset token is valid and matches the database.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Password reset token does not match or user not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Password reset token has expired.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No password reset token found.");
            }
            return false;
        }

    }
}
