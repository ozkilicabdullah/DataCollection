using Dapper;
using SwEpApi.Model;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using SwEpApi.Services;
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

namespace SwEpApi.Controllers
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
        public ResponseModel Login([FromBody]LoginRequest login)
        {
            if(login==null || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return new ResponseModel()
                {
                    Data = new Dictionary<string, object>(),
                    Success = false,
                    Errors = new List<string>() { "Incorrect or missing parameters." }
                };
            }
            var response = new ResponseModel()
            {
                Data = new Dictionary<string, object>(),
                Success = false,
                Errors = new List<string>() { "The username or password is incorrect please try again" }
            };

            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken( new LoginRequest() { UserName = user.Username } );
                response.Data.Add("token", tokenString);
                response.Data.Add("name", user.Name );
                response.Data.Add("email", user.Email);
                response.Data.Add("role", user.Role);
                response.Success = true;
                response.Errors.Clear();
            }

            return response;

        }

        [Authorize(AuthenticationSchemes = "Bearer")]        
        [HttpGet]
        [Route("verify")]
        public ResponseModel Verify()
        {
            var response = new ResponseModel()
            {
                Data = new Dictionary<string, object>(),
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
                  new Claim("user", userInfo.UserName)
              },
              expires: DateTime.Now.AddYears(10),
              signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(LoginRequest login)
        {
            //  User userLogin = ConnectionService.Scope("Default", x => x.Query<User>("SELECT * FROM [User] WHERE Email=@Email and Password=@Password", new { Email = login.UserName, login.Password })).FirstOrDefault();
            //Validate the User Credentials     
            List<User> Users = new List<User>();
            Configuration.GetSection("Users").Bind(Users);

            var user = (from c in Users where c.Username == login.UserName && c.Password == login.Password select c).FirstOrDefault();
            if(user!=null)
            {
                return new User { Name = user.Name, Username=user.Username, Email=user.Email, Role=user.Role };
            }
            
            return null;
        }

    }

}
