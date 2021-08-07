using DataCollection.Model;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DataCollection.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IConnectionService ConnectionService;

        public LoginController(IConfiguration configuration, IConnectionService connectionService)
        {
            Configuration = configuration;
            ConnectionService = connectionService;
        }

        [AllowAnonymous]
        [HttpPost]
        public LoginReponseModel Login([FromBody] LoginRequest login)
        {
            if (login == null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return new LoginReponseModel()
                {
                    User = new Dictionary<string, object>(),
                    Success = false,
                    Errors = new List<string>() { "Incorrect or missing parameters." }
                };
            }
            var response = new LoginReponseModel()
            {
                User = new Dictionary<string, object>(),
                Success = false,
                Errors = new List<string>() { "The username or password is incorrect please try again" }
            };

            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(new LoginRequest() { UserName = user.Username, Password = user.Password });
                response.User.Add("token", tokenString);
                response.User.Add("name", user.Name);
                response.User.Add("email", user.Email);
                response.Success = true;
                response.Errors.Clear();
            }

            return response;

        }

        [HttpGet]
        public bool Login()
        {
            return ConnectionService.SetupProject();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("verify")]
        public LoginReponseModel Verify()
        {
            var response = new LoginReponseModel()
            {
                User = new Dictionary<string, object>(),
                Success = true,
                Errors = new List<string>()
            };

            return response;

        }

        private string GenerateJSONWebToken(LoginRequest userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
              Configuration["Jwt:Audience"],
              claims: new List<Claim>() {
                  new Claim("user", userInfo.UserName),
                  new Claim("password", userInfo.Password)
              },
              expires: DateTime.Now.AddYears(10),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(LoginRequest login)
        {
            User user = ConnectionService.GetCurrentUser(login.UserName, login.Password);

            if (user != null)
            {
                return new User { Name = user.Name, Username = user.Username, Email = user.Email, Role = user.Role, Password = user.Password, Perms = user.Perms };
            }

            return null;
        }

    }

}
