using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace COCOApp.Repositories
{
    public class SellerDetailRepository : ISellerDetailRepository
    {
        private readonly StoreManagerContext _context;

        public SellerDetailRepository(StoreManagerContext context)
        {
            _context = context;
        }

        public void AddSellerDetails(SellerDetail details)
        {
            _context.SellerDetails.Add(details);
            _context.SaveChanges();
        }

        public void UpdateSellerDetails(int userId, SellerDetail detail)
        {
            var existingDetail = _context.SellerDetails.SingleOrDefault(d => d.Id == userId);

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
