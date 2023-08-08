using FAP_API.DatabaseClasses;
using FAP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FAP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        UserDbManager db = new UserDbManager("Data Source=DatabaseFile/fap_API.db");
        public static User user = new User();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet(Name = "GetAllUsers")]
        //[Authorize]
        public IEnumerable<User> Get()
        {
            return db.GetUsers();
        }

		/*[HttpGet("{firstname}", Name = "GetUserByName")]
		public IEnumerable<User> GetUserByName(string firstname)
		{
			return db.GetUserByFirstname(firstname);
		}*/

		[HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> Get(int id)
        {
            var user = db.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("register")]
        public IActionResult Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                int newUserId = db.AddUser(user);

                user.Id = user.Id;
                user.Username = user.Username;
                user.Firstname = user.Firstname;
                user.Lastname = user.Lastname;
                user.Email = user.Email;
                user.Phone = user.Phone;
                user.Adress = user.Adress;
                user.City = user.City;
                user.Country = user.Country;
                user.Password = user.Password;

                user.Id = newUserId;
                return CreatedAtRoute("GetUser", new { id = newUserId }, user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest();
            }

            var existingUser = db.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = user.Username;
            existingUser.Firstname = user.Firstname;
            existingUser.Lastname = user.Lastname;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Adress = user.Adress;
            existingUser.City = user.City;
            existingUser.Country = user.Country;

            db.UpdateUser(existingUser);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedResponse>> login(UserDto request)
        {
            if (db.ExistanceUser(request) == null)
            {
                return BadRequest("Password incorrect.");
            }

            string token = CreateToken(db.ExistanceUser(request));
            AuthenticatedResponse response = new AuthenticatedResponse();
            response.Access_token = token;
            response.User = db.ExistanceUser(request);
            return Ok(response);
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteUser(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
