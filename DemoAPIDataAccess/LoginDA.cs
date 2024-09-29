using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DemoAPIDataAccess
{
    public class LoginDA : BaseDA, ILoginDA
    {
        private IConfiguration _config;

        public LoginDA(IConfiguration config) : base(config) 
        { 
            _config = config;
        }

        public async Task<bool> CreateLogin(string username, string password)
        {
            // Define parameters including your output parameters
            DynamicParameters parameters = new();
            parameters.Add("@Username", username);
            parameters.Add("@Password", HashString(password));

            int result = await ExecuteStoredProcedureAsync("[dbo].[SP_Insert_Login]", parameters);

            return true;
        }

        public async Task<string> GetLogin(string username, string password)
        {
            // Define parameters including your output parameters
            DynamicParameters parameters = new();
            parameters.Add("@Username", username);
            parameters.Add("@Password", HashString(password));

            string result = await ExecuteStoredProcedureQuerySingleOrDefaultAsync<string>("[dbo].[SP_Get_Login]", parameters);

            return !string.IsNullOrEmpty(result) ? CreateToken() : string.Empty;

        }

        private string CreateToken()
        {
            //your logic for login process
            //If login usrename and password are correct then proceed to generate token

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken Sectoken = new(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        private string HashString(string str)
        {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = Encoding.ASCII.GetBytes("17e27108-f00c-4040-956f-1e695503089f");//RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }
}
