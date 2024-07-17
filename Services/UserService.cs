using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace COCOApp.Services
{
    public class UserService : StoreManagerService
    {
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

            // Update user properties
            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.UpdatedAt = user.UpdatedAt;

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
        public async Task UpdateUserPasswordResetTokenAsync(string email)
        {
            // Simulate asynchronous database update
            var user = await _context.Users.Include(u => u.SellerDetail)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                // Generate a new password reset token (this is just an example, you should implement a proper token generation mechanism)
/*                user.fo = Guid.NewGuid().ToString();
                user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1);*/

                // Save the changes asynchronously
                await _context.SaveChangesAsync();

                Console.WriteLine($"Password reset token updated for: {email}");
            }
            else
            {
                Console.WriteLine($"No user found with email: {email}");
            }
        }

    }
}
