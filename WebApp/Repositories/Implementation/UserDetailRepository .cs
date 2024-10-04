using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace COCOApp.Repositories
{
    public class UserDetailRepository : IUserDetailRepository
    {
        private readonly StoreManagerContext _context;

        public UserDetailRepository(StoreManagerContext context)
        {
            _context = context;
        }
        public void AddUserDetails(BuyerDetail details)
        {
            _context.BuyerDetails.Add(details);
            _context.SaveChanges();
        }

        public void UpdateUserDetails(int userId, BuyerDetail detail)
        {
            var existingDetail = _context.BuyerDetails.SingleOrDefault(d => d.UserId == userId);

            if (existingDetail == null)
            {
                throw new ArgumentException("Seller detail not found.");
            }

            existingDetail.Fullname = detail.Fullname;
            existingDetail.Dob = detail.Dob;
            existingDetail.Gender = detail.Gender;
            existingDetail.Address = detail.Address;
            existingDetail.Phone = detail.Phone;

            _context.SaveChanges();
        }
    }
}
