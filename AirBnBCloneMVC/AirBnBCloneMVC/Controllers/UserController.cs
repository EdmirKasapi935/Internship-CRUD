using AirBnBCloneMVC.Data;
using AirBnBCloneMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace AirBnBCloneMVC.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly BnBDBContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserController(BnBDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        /* --the old login method, keeping it here just in case the other method gets too messed up
         * public async Task<IActionResult> Login(string LoginEmail, string LoginPassword )
        {
            LoginPassword = this.HashPassword( LoginPassword );

            User toLogin;

            if ( (toLogin = await dbContext.Users.Where(n => n.UserEmail == LoginEmail && n.UserPassword == LoginPassword).FirstOrDefaultAsync()) == null )
            {
                return BadRequest("Invalid email or password entered");
            }
            else
            {
                return CheckLogin(toLogin);
            }
        }
        */

        public IActionResult RegisterForm()
        {
            return View("Register");
        }

        public async Task<IActionResult> Insert(UserViewModel model)
        {
            if(CheckEmailAvailability(model.UserEmail) == false)
            {
                ViewData["EmailUnavailableMessage"] = "Email is already taken";
                return View("Register");
            }
            else
            {
                var user = new User
                {
                    UserName = model.UserName,
                    UserEmail = model.UserEmail,
                    UserPassword = this.HashPassword(model.UserPassword),
                    UserPhone = model.UserPhone,
                    IsOwner = model.IsOwner,
                };

                await dbContext.Users.AddAsync(user);

                await dbContext.SaveChangesAsync();

                User registered = await dbContext.Users.Where(n => n.UserEmail == user.UserEmail && n.UserPassword == user.UserPassword).SingleAsync();

                ViewData["RegistrationSuccessfulMesssage"] = "You have been registered successfully! You may now log in.";

                return View("Views\\Home\\Index.cshtml");
            }
            
        }


        [Authorize]
        public IActionResult CheckLogin(User user)
        {
            if (user.IsOwner)
            {
                return OwnerMenu();
            }
            else
            {
                return CustomerMenu();
            }
        }

        [Authorize]
        public IActionResult CustomerMenu()
        {
            User customer = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
            var rooms = dbContext.Rooms.ToList();
            return View("Views\\User\\Customer\\CustomerCheck.cshtml", Tuple.Create( customer , rooms, dbContext.Users.Where(n => n.IsOwner == true).ToList()));
        }

        [Authorize]
        public  IActionResult OwnerMenu()
        {
            User owner = JsonConvert.DeserializeObject<User>(httpContextAccessor.HttpContext.Session.GetString("User"));
            var rooms = dbContext.Rooms.Where(n => n.Owner_Id == owner.Id.ToString()).ToList();

            return View("Views\\User\\Owner\\OwnerCheck.cshtml", Tuple.Create(owner, rooms));
        }

        public IActionResult ChangePasswordForm()
        {
            return View("Views\\User\\ChangePassword.cshtml");
        }

        public async Task<IActionResult> ChangePassword(string UserEmail, string newPassword, string newPasswordConfirm)
        {
            User user;

            if((user = await dbContext.Users.Where(n => n.UserEmail == UserEmail).FirstOrDefaultAsync()) == null)
            {
                ViewData["WrongEmailMessage"] = "Invalid credentials entered.";
                return View("Views\\User\\ChangePassword.cshtml");
            }
            else if(newPassword != newPasswordConfirm)
            {
                ViewData["ConfirmPasswordErrorMessage"] = "The new password and confirm password fields don't match.";
                return View("Views\\User\\ChangePassword.cshtml");
            }
            else
            {
                string newHashed = HashPassword(newPassword);

                user.UserPassword = newHashed;

                await dbContext.SaveChangesAsync();

                ViewData["PasswordChangeSuccessfulMessage"] = "Password changed successfully.";
                return View("Views\\Home\\Index.cshtml");
            }
        }

        private string HashPassword(string password)
        {
            string saltstring = "qwofqwfikmqwfkqwqwnqjnqjgqwgqgqg";

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(saltstring),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256/8
                ));

            return hashed;
        }

        private bool CheckEmailAvailability(string email) //Having two users with the same email can be very confusing
        {
            var check = dbContext.Users.Where(n => n.UserEmail == email ).ToList();

           if(check.Count == 0)
            {
                return true;
            }
           else
            {
                return false;
            }
        }
    }
}
