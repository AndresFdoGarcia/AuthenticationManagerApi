using AuthenticationManagerApi.Data.Models;
using AuthenticationManagerApi.Data.MySQL.UserConfig.Contract;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagerApi.Data.MySQL.UserConfig.Implementation
{
    public class UserApiConfiguration : IUserApiConfiguration
    {
        private readonly MySQLConfiguration _connectionString;
        public UserApiConfiguration(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }


        public async Task<bool> DeleteUser(int id)
        {
            var db = dbConnection();

            var sql = @"DELTE FROM authmanager WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new {Id = id});

            return result > 0;
        }

        public Task<IEnumerable<UserApi>> GetAllUsers()
        {
            var db = dbConnection();

            var sql = @"SELECT id, username, email, rol
                        FROM users";

            return db.QueryAsync<UserApi>(sql, new { });
        }

        public Task<UserApi> GetAuthUser(string username, string password)
        {
            var db = dbConnection();

            var sql = @"SELECT id, username, firstname, lastname, email, rol
                        FROM users
                        WHERE username = @Username
                        AND password = @Password";

            return db.QueryFirstOrDefaultAsync<UserApi>(sql, new { Username = username, Password = password });
        }

        public Task<UserApi> GetUser(string username)
        {
            var db = dbConnection();

            var sql = @"SELECT id, username, firstname, lastname, email, rol
                        FROM users
                        WHERE username = @Username";                        

            return db.QueryFirstOrDefaultAsync<UserApi>(sql, new { Username = username});
        }

        public async Task<bool> InsertUser(UserApi user)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO `authmanager`.`users` (`username`, `password`, `email`, `firstname`, `lastname`, `rol`) 
                        VALUES(@Username, @Password, @Email, @Firstname, @Lastname, @Rol)";

            var result = await db.ExecuteAsync(sql, new
            {
                user.Username, user.Password, user.Email, user.Firstname, user.Lastname, user.Rol
            });

            return result > 0;
        }

        public async Task<bool> UpdateUser(UserApi user, int id)
        {
            var db = dbConnection();

            var sql = @"UDPATE authmanager
                        SET username = @Username,
                            password = @Password,
                            email = @Email,
                            firstname = @Firstname,
                            lastname = @Lastname,
                            rol = @Rol,
                        WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new
            {
                user.Id, user.Username,user.Password,user.Email,user.Firstname,user.Lastname,user.Rol
            });

            return result > 0;
        }
    }
}
