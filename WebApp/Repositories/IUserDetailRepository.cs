using COCOApp.Models;
using System.Collections.Generic;

namespace COCOApp.Repositories
{
    public interface IUserDetailRepository
    {
        void AddUserDetails(UserDetail details);
        void UpdateUserDetails(int userId, UserDetail detail);
    }
}
