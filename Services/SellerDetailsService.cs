using COCOApp.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace COCOApp.Services
{
    public class SellerDetailsService: StoreManagerService
    {
        public void AddSellerDetails(SellerDetail details)
        {
            _context.SellerDetails.Add(details);
            _context.SaveChanges();
        }
        public void UpdateSellerDetails(int userId,SellerDetail detail)
        {
            // Find the existing seller detail in the database
            var existingDetail = _context.SellerDetails.SingleOrDefault(d => d.Id == userId);

            if (existingDetail == null)
            {
                throw new ArgumentException("Seller detail not found.");
            }

            // Update seller detail properties
            existingDetail.Fullname = detail.Fullname;
            existingDetail.Dob = detail.Dob;
            existingDetail.Gender = detail.Gender; 
            existingDetail.Address = detail.Address;
            existingDetail.Phone = detail.Phone;

            // Save changes to the database
            _context.SaveChanges();
        }

    }
}
