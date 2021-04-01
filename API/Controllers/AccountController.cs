using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDto)
        {
            if(await UserExists(registerDto.Username)) BadRequest("User already exists");

           using var hmac = new HMACSHA512();

           var user = new AppUser()
           {
               UserName = registerDto.Username.ToLower(),
               PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
               PasswordSalt = hmac.Key
           };

           _context.User.Add(user);
           await _context.SaveChangesAsync();

           return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await _context.User.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
                return Unauthorized("Invalid User Name");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(var i=0; i<computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _context.User.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}