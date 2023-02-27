using AuthenticationManagerApi.Data.Models;
using AuthenticationManagerApi.Data.MySQL.UserConfig.Contract;
using AuthenticationManagerApi.Data.MySQL.UserConfig.Implementation;
using Microsoft.IdentityModel.Tokens;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace AuthenticationManagerApi.Business.Core.Business.JWT
{
    public class JWTConfigurationBusiness
    {
        private readonly IUserApiConfiguration _userApiConfiguration;
        private readonly IConfiguration _config;
        public JWTConfigurationBusiness(
            IUserApiConfiguration userApiConfiguration,
            IConfiguration configuration) {
            _userApiConfiguration = userApiConfiguration;
            _config = configuration;
        }

        public async Task<MResponse> Authenticate(LoginUser loginUser)
        {
            var result = await _userApiConfiguration.GetAuthUser(loginUser.UserName, loginUser.Password);
            if(result!=null)
            {
                var token = GenerateToken(result);
                MResponse response = new MResponse
                {
                    success= true,
                    message = token                   
                };
                return response;
            }
            return null;
        }

        private string GenerateToken(UserApi user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.Firstname),
                new Claim(ClaimTypes.Surname,user.Lastname),
                new Claim(ClaimTypes.Role,user.Rol)
            };

            //Crear el token

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(6),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public dynamic ValidateToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "No autorizado",
                        result = ""
                    };
                }

                var userName = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var user = _userApiConfiguration.GetUser(userName);

                middleUser middleUser = new middleUser
                {
                    UserName = user.Result.Username,
                    Email = user.Result.Email,
                    Rol = user.Result.Rol
                };

                return new
                {
                    success = true,
                    message = "Autorizado",
                    result = middleUser
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }

        public async Task<MResponse> Createuser(UserApi userApi)
        {
            var errors = ValidateIntroUser(userApi);
            if (!string.IsNullOrEmpty(errors))
            {
                return new MResponse
                {
                    success = false,
                    message = errors.ToString(),
                };
            }

            await _userApiConfiguration.InsertUser(userApi);
            return new MResponse
            {
                success = true,
                message = "Usuario creado"
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var result = await _userApiConfiguration.DeleteUser(id);
            if (!result) return false;
            return true;
        }

        public class middleUser
        {
            public string UserName { get; set;}
            public string Email { get; set;}
            public string Rol { get; set;}
        }

        public class MResponse
        {
            public bool success { get; set;}
            public string message { get; set;}           
        }

        private string ValidateIntroUser(UserApi user)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(user.Username))
            {
                errors.Add("Campo UserName es requerido");
            };
            if (string.IsNullOrEmpty(user.Rol))
            {
                errors.Add("Campo Rol es requerido");
            };
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.Add("Campo Email es requerido");
            };
            if (string.IsNullOrEmpty(user.Password))
            {
                errors.Add("Campo Password es requerido");
            };
            if (string.IsNullOrEmpty(user.Firstname))
            {
                errors.Add("Campo FirstName es requerido");
            };
            if (string.IsNullOrEmpty(user.Lastname))
            {
                errors.Add("Campo LastName es requerido");
            };

            return errors.Count == 0 ? "" : string.Join(",", errors);
        }
    }
}
