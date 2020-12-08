using AuthService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class LoginRepository : ILogin
    {
        private static readonly List<User> _users = new List<User>()
        {
            new User{ UserName="steve",Password="psteve"},
            new User{ UserName="tony",Password="ptony"},
            new User{ UserName="bruce",Password="pbruce"},
            new User{ UserName="carol",Password="pcarol"},
        };
        public User GetUser(string uname)
        {
            var u = _users.Where(c => c.UserName == uname).FirstOrDefault();
            return u;
        }
    }
}
