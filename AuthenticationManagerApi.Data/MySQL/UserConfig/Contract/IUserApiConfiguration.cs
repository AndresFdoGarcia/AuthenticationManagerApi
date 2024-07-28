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
        Task<IEnumerable<UserInfo>> GetAllUsers();
        Task<UserInfo> GetUser(string username);
        Task<UserApi> GetById(string id);
        Task<bool> InsertUser(UserApi user);
        Task<bool> UpdateUser(UserApi user, string id);
        Task<bool> DeleteUser(string id);
        Task<UserInfo> GetAuthUser(string username,string password);        
    }
}
