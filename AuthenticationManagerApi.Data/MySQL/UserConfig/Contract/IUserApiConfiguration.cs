using AuthenticationManagerApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagerApi.Data.MySQL.UserConfig.Contract
{
    public interface IUserApiConfiguration
    {
        Task<IEnumerable<UserApi>> GetAllUsers();
        Task<UserApi> GetUser(string username);
        Task<UserApi> GetById(int id);
        Task<bool> InsertUser(UserApi user);
        Task<bool> UpdateUser(UserApi user, int id);
        Task<bool> DeleteUser(int id);
        Task<UserApi> GetAuthUser(string username,string password);
    }
}
