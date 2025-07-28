using AirBnBCloneMVC.Data;
using AirBnBCloneMVC.Models;
//using AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AirBnBCloneMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly BnBDBContext dbContext;
        private readonly UserController userController;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(IConfiguration configuration,  BnBDBContext dbContext, UserController userController, IHttpContextAccessor httpContextAccessor)
        {
          this.configuration = configuration;
          this.dbContext = dbContext;
          this.userController = userController;
            this.httpContextAccessor = httpContextAccessor;  
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginAuth(string LoginEmail, string LoginPassword)
        {
            string PasswordHash = this.HashPassword(LoginPassword);

            User toLogin = await dbContext.Users.Where(n => n.UserEmail == LoginEmail && n.UserPassword == PasswordHash).FirstOrDefaultAsync();

            if (toLogin == null)
            {
                ViewData["InvalidUserMessage"] = "Incorrect email or password entered";
                return View("Views\\Home\\Index.cshtml");
            }
            else
            {
                var token = GenerateJwtToken(toLogin);
                
                //TempData["token"] = token;
                //TempData["User"] = toLogin;

                var claims = new List<Claim> {
                  new Claim(ClaimTypes.Name, toLogin.Id.ToString()),
                  new Claim(ClaimTypes.Name, toLogin.UserName)
            };
           

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity) );

                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(toLogin));
                HttpContext.Session.SetString("TOken", JsonConvert.SerializeObject(token));
                //User check = JsonConvert.DeserializeObject<User>( httpContextAccessor.HttpContext.Session.GetString("User") );


                return userController.CheckLogin( JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User")) );
               
            }

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            return RedirectToAction("Index","Home");
        }

        private string HashPassword(string password)
        {
            string saltstring = "qwofqwfikmqwfkqwqwnqjnqjgqwgqgqg";

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(saltstring),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                ));

            return hashed;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = Encoding.UTF8.GetBytes(configuration["JwtConfig:Key"]);

            var claims = new Claim[] {
                  new Claim(ClaimTypes.Name, user.Id.ToString()),
                  new Claim(ClaimTypes.Name, user.UserName)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["JwtConfig:Issuer"],
                configuration["JwtConfig:Audience"], //this is the audience
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtConfig:TokenValidityMins"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
