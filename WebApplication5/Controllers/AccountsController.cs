using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication5.Models;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly IRepository _repository;

        public AccountsController(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsPrincipalFactory, IConfiguration configuration, IRepository repository)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserVM uvm)
        {
            var user = await _userManager.FindByNameAsync(uvm.UserName);


            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = uvm.UserName
                    
                };

                var result = await _userManager.CreateAsync(user, uvm.Password);

                if (result.Errors.Count() > 0)
                    StatusCode(StatusCodes.Status500InternalServerError, "Internal error occured. Please contact support");
                else
                {
                    return StatusCode(StatusCodes.Status204NoContent);

                }

                return Ok("Account created successfully");
            }
            else
            { return StatusCode(StatusCodes.Status403Forbidden, "Account already exists"); }

           


        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserVM uvm)
        {
            var user = await _userManager.FindByNameAsync(uvm.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, uvm.Password))
            {
                try
                {
                    var principal = await _claimsPrincipalFactory.CreateAsync(user);
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                    //return GenerateJWTToken(user);
                    //var email = new SmtpClient
                    //{
                    //    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //    UseDefaultCredentials = false,
                    //    EnableSsl = true,
                    //    Host = "smtp.gmail.com",
                    //    Port = 587,
                    //    Credentials = new NetworkCredential("suhailmullah3@gmail.com", "iczzzkycwmapkdqr")
                    //};

                    //Random rng = new Random();
                    //string Otp = rng.Next(1000, 9999).ToString();
                    //string subject = "Top Sales OTP";
                    //string body = "Your One Time Pin (OTP) is: " + Otp;
                    //email.Send("suhailmullah3@gmail.com", uvm.EmailAddress, subject, body);

                    //var serverOtp = new OTP
                    //{
                    //    otp = Otp,
                    //    email = uvm.EmailAddress
                    //};

                    //_repository.Add(serverOtp);
                    //await _repository.SaveAllChangesAsync();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal error occured. Please contact support");
                }
            }
            else
            {
                return NotFound("Does not exist");
            }

            //var loggedInUser = new UserViewModel { EmailAddress = user.Email, Password = user.PasswordHash };

            //return Ok(loggedInUser);
            return GenerateJWTToken(user);
        }

        [HttpGet]
        private ActionResult GenerateJWTToken(User appUser)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(3)
           );

            return Created("", new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo

            });
        }
    }
}
