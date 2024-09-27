using COCOApp.Models;
using COCOApp.Repositories;

namespace COCOApp.Services
{
    public class UserDetailsService : StoreManagerService
    {
        private readonly IUserDetailRepository _userDetailRepository;

        public UserDetailsService(IUserDetailRepository userDetailRepository)
        {
            _userDetailRepository = userDetailRepository;
        }

        public void AddUserDetails(BuyerDetail details)
        {
            _userDetailRepository.AddUserDetails(details);
        }

        public void UpdateUserDetails(int userId, BuyerDetail detail)
        {
            _userDetailRepository.UpdateUserDetails(userId, detail);
        }
    }
}
