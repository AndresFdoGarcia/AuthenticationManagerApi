using AuthenticationManagerApi.Business.Core.Business.JWT;
using AuthenticationManagerApi.Data.Models;
using AuthenticationManagerApi.Data.MySQL.UserConfig.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI.Common;
using System.Security.Claims;
using static AuthenticationManagerApi.Business.Core.Business.JWT.JWTConfigurationBusiness;

namespace AuthenticationManagerApi.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserApiConfiguration _usersApiConfiguration;
        private readonly JWTConfigurationBusiness _jwt;

        public UsersController(
            IUserApiConfiguration userApiConfiguration,
            JWTConfigurationBusiness jWTConfiguration)
        {
            _usersApiConfiguration = userApiConfiguration;
            _jwt = jWTConfiguration;
        }
        [HttpGet]
        public async Task<IActionResult> healthcheck()
        {
            return StatusCode(200, "Todo va bien");
        }

        [HttpGet("AllUsers")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _usersApiConfiguration.GetAllUsers();
            return StatusCode(200,response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            var result = _jwt.Authenticate(loginUser).Result;
            if (result == null)
            {
                return StatusCode(401, "No autorizado");
            }
            return StatusCode(200,result);
        }

        [HttpPost("token")]
        public dynamic ValidateToken()
        {
            var salga = HttpContext.User.Identity as ClaimsIdentity;
            var rtoken = _jwt.ValidateToken(salga);
            return rtoken;
        }
        
        [HttpPost("Create")]
        //[Authorize(Roles =("Admin"))]
        public async Task<IActionResult> CreateUser(UserApi userApi)
        {
            if(userApi == null) return BadRequest();                   
            var result = _jwt.Createuser(userApi).Result;

            if (result.success == false)
            {
                return BadRequest(result.message);
            }
            else
            {
                var userf = new UserInfo();
                userf.Username = userApi.Username;
                userf.Firstname = userApi.Firstname;
                userf.Lastname = userApi.Lastname;
                userf.Email = userApi.Email;
                return StatusCode(200, userf);
            }
            
        }

        [HttpGet("OneUser")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> GetOneUser(string username)
        {
            var result = _usersApiConfiguration.GetUser(username).Result;
            if(result is null)
            {
                var eror = new UserApi();

                eror.Id = "";
                eror.Username = "";
                eror.Lastname = "";
                eror.IdNumber = "";
                eror.IdType = "";
                eror.Email = "";
                eror.Firstname = "";


                return StatusCode(200,eror);
            }
            else
            {
                result.Username= username;
            }

            return StatusCode(200, result);
        }

        [HttpGet("OnlyUser")]
        //[Authorize(Roles = ("Admin"))]
        public async Task<IActionResult> GetUserbyId(string id)
        {
            var result = _usersApiConfiguration.GetById(id).Result;
            if (result is null)
            {
                var eror = new UserApi();

                eror.Id = "";
                eror.Username = "";
                eror.Lastname = "";
                eror.IdNumber = "";
                eror.IdType = "";
                eror.Email = "";
                eror.Firstname = "";


                return StatusCode(200, eror);
            }

            return StatusCode(200, result);
        }
    }
}
