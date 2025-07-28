using AirBnBCloneMVC.Data;
using AirBnBCloneMVC.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AirBnBCloneMVC.Services
{
    public class JwtService
    {
        private readonly BnBDBContext dbContext;
        private readonly IConfiguration configuration;
        public JwtService(BnBDBContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        /*public async Task<LoginResponseModel> Authenticate(LoginRequestModel loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.UserName) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return null;

            var userAccount = await dbContext.Users.FirstOrDefaultAsync(n => n.UserName == loginRequest.UserName);
                
        }*/
    }
}
