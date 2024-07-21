﻿using COCOApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COCOApp.Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        List<User> GetUsers(string nameQuery, int pageNumber, int pageSize);
        int GetTotalUsers(string nameQuery);
        void AddUser(User user);
        void UpdateUser(int userId, User user);
        void UpdateUserPassword(int userId, string password);
        User GetUserByNameAndPass(string username, string password);
        User GetUserById(int userId);
        User GetActiveUserByEmail(string email);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        Task UpdateUserPasswordResetTokenAsync(string email);
        Task<bool> CheckPasswordResetTokenAsync(string email);
        Task UpdateRemembermeTokenAsync(string username);
        Task<User> CheckRemembermeTokenAsync();
    }
}
