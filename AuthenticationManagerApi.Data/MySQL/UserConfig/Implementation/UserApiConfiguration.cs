using AuthenticationManagerApi.Data.Models;
using AuthenticationManagerApi.Data.MySQL.UserConfig.Contract;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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


        public async Task<bool> DeleteUser(string id)
        {
            var db = dbConnection();

            var sql = @"DELTE FROM persons WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new {Id = id});

            return result > 0;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsers()
        {
            var db = dbConnection();

            var response = await db.QueryAsync<UserInfo>("GetAllUsers", commandType: CommandType.StoredProcedure);
            return response;
        }
        
        public Task<UserInfo> GetAuthUser(string username, string password)
        {
            var db = dbConnection();

            var sql = @"SELECT  p.firstname AS firstname, p.lastname AS lastname, p.email as email
                        FROM persons p
                        INNER JOIN users u ON p.id = u.id
                        where u.username=@Username AND u.password=@Password;";

            return db.QueryFirstOrDefaultAsync<UserInfo>(sql, new { Username = username, Password = password });
        }

        public Task<UserApi> GetById(string id)
        {
            var db = dbConnection();

            var sql = @"SELECT idnumber, idtyper, username, firstname, lastname, email
                        FROM users
                        WHERE id = @Id";                        

            return db.QueryFirstOrDefaultAsync<UserApi>(sql, new { Id = id });
        }

        public Task<UserInfo> GetUser(string username)
        {
            var db = dbConnection();

            var sql = @"SELECT  p.firstname AS firstname, p.lastname AS lastname, p.email as email
                        FROM persons p
                        INNER JOIN users u ON p.id = u.id
                        where u.username=@Username";

            return db.QueryFirstOrDefaultAsync<UserInfo>(sql, new { Username = username });
        }

        public async Task<bool> InsertUser(UserApi user)
        {
            var db = dbConnection();

            var sql_p = @"INSERT INTO `authmanager`.`persons` (`id`,`idnumber`,`idtype`,`email`, `firstname`, `lastname`) 
                        VALUES(@Id,@IdNumber, @IdType, @Email, @Firstname, @Lastname)";

            var sql_u = @"INSERT INTO `authmanager`.`users` (`id`,`username`,`password`) 
                        VALUES(@Id, @Username, @Password)";
            
            var newId = Guid.NewGuid().ToString();
            user.Id= newId;
                       
                try
                {
                    var resultper = await db.ExecuteAsync(sql_p, new
                    {
                        Id = user.Id,
                        IdNumber = user.IdNumber,
                        IdType = user.IdType,
                        Email = user.Email,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname

                    });

                    var resultuser = await db.ExecuteAsync(sql_u, new {
                        Id = user.Id,
                        Username = user.Username,
                        Password = user.Password
                    });

                    if (resultper > 0 && resultuser > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch(Exception ex) {
                    
                    throw;
                }
            
        }

        public async Task<bool> UpdateUser(UserApi user, string id)
        {
            var db = dbConnection();

            var sql = @"UDPATE authmanager.persons
                        SET idnumber = @IdNumber,
                            idtype = @IdType,                            
                            email = @Email,
                            firstname = @Firstname,
                            lastname = @Lastname                            
                        WHERE id = @Id";

            var result = await db.ExecuteAsync(sql, new
            {
                user.Id, user.IdNumber, user.IdType, user.Username,user.Password,user.Email,user.Firstname,user.Lastname
            });

            return result > 0;
        }        
    }
}
