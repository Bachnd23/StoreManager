using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface ISellerDetailRepository
    {
        void AddSellerDetails(SellerDetail details);
        void UpdateSellerDetails(int userId, SellerDetail detail);
    }
}
