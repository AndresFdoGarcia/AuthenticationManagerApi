using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManagerApi.Data.Models
{
    public class UserApi
    {
        public string Id { get; set; }
        public string IdNumber { get; set; }
        public string IdType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }        

    }
}
