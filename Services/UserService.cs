using COCOApp.Models;
using Microsoft.EntityFrameworkCore;

namespace COCOApp.Services
{
    public class UserService: StoreManagerService
    {
        public List<User> GetUsers()
        {
            var query = _context.Users.AsQueryable();
            query = query.Include(u => u.SellerDetail);
            return query.ToList();
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
    }
}
