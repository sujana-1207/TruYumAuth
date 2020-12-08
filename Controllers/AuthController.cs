using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogin _login;
        private IConfiguration _config;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthController));


        public AuthController(IConfiguration config,ILogin login)
        {
            _config = config;
            _login = login;
        }
        
        // GET: api/Auth/5
        [HttpGet("{id}", Name = "Get")]
        public User Get(string uname)
        {
            return _login.GetUser(uname);
        }

        // POST: api/Auth
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            IActionResult response = Unauthorized();
            _log4net.Info(user.UserName + "made a requested to login");
            var u = Get(user.UserName);
            if (u.UserName == user.UserName && u.Password == user.Password)
            {
                var token = GenerateToken(user);
                _log4net.Info(user.UserName+" successfully logged in.");
                return new OkObjectResult(new { token = token });
            }
            else
            {
                _log4net.Error(user.UserName+" failed to login.");
                return BadRequest();
            }
        }

        // PUT: api/Auth/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
               _config["Jwt:Issuer"],
               null,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token).ToString();



        }
    }
}
