using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IUserDetailRepository
    {
        void AddUserDetails(BuyerDetail details);
        void UpdateUserDetails(int userId, BuyerDetail detail);
    }
}
