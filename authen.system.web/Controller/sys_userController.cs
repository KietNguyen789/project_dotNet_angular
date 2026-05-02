using authen.common.BaseClass;
using authen.common.data.Models;
using authen.Database.Mongodb.Collection;
using authen.system.data.DataAccess;
using authen.system.data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using authen.common.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;  

namespace authen.system.web.Controller
{

    
    [ApiController]
    public partial class sys_userController : BaseAuthenticationController
    {
        private readonly IConfiguration _config;

        public MongoDBContext _context;

        public sys_user_repo _repo;
        public AppSettings _appsetting;

        public sys_userController(IConfiguration config, MongoDBContext context, IOptions<AppSettings> appsetting)
        {
            _config = config;
            _context = context;
            _appsetting = appsetting.Value;
            _repo = new sys_user_repo(context);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> sign_up([FromBody] JObject json)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<sys_user_model>(json.ToString());
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordSalt = passwordSalt;
                model.db.PasswordHash = passwordHash;
                await _repo._context.sys_user_col.InsertOneAsync(model.db);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.ToString());
            var user = _repo.getElementById(model.email);
            var settings = _config.GetSection("AppSettings");
            if (model.password == null || model.password.Trim() == "")
            {
                ModelState.AddModelError("password", "Password is required");
            }
            if(model.email == null || model.email.Trim() == "")
            {
                ModelState.AddModelError("username", "Username is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                ModelState.AddModelError("username", "user doesn't sign in");
            }
            else
            {
                if (!VerifyPasswordHash(model.password, user.PasswordHash, user.PasswordSalt))
                {
                    ModelState.AddModelError("password", "Password is incorrect");
                }
            }

            if (!ModelState.IsValid)
            {
                return generateError();
            }


            var tokenHandler = new JwtSecurityTokenHandler();   
            var key = Encoding.ASCII.GetBytes(_appsetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,"school"),
                    new Claim(ClaimTypes.Gender, user.gender.ToString()),
                    new Claim(ClaimTypes.DateOfBirth,user.date_of_birth.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(settings.GetValue<double>("ExpireMinutes")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
         //   HttpContext.Session.Remove("CaptchaCode");
        
            // return basic user info and authentication token
            return Ok(new
            {
                user,
                token = tokenString,
                host = "https://" + Request.Host.Value
            });
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private void generate_token()
        {

        }

    

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            var username = User.Identity?.Name;
            return Ok(new { username });
        }

    }

   
}
