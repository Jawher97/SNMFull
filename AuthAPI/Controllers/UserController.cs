using AuthAPI.DataContext;
using AuthAPI.DTO;
using AuthAPI.Helpers;
using AuthAPI.Models;
using AuthAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthAPI.Controllers
{
    [Route("auth/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext _dbcontext;
        private readonly IConfiguration _config;
        private readonly IEmailService _service;

        public UserController(ApplicationDbContext dbContext, IConfiguration config, IEmailService service)
        {
            _dbcontext = dbContext;
            _config = config;
            _service = service;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _dbcontext.Users
                .FirstOrDefaultAsync(x => x.Email == userObj.Email);

            if (user == null || user.isActivated == false)
                return NotFound(new { Message = "User not found or Account not activated!" });

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            user.Token = CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _dbcontext.SaveChangesAsync();

            return Ok(user);

        }

        [HttpPost("activation-email")]
        public async Task<IActionResult> SendActivationEmail([FromQuery] string email)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ActivateToken = emailToken;
            user.ActivateTokenExpiry = DateTime.Now.AddMinutes(15);

            string from = _config["EmailSettings:From"];

            var emailmodel = new EmailModel(email, "Activate Account", ActivateEmailBody.ActivateEmailStringBody(email, emailToken));

            _service.SendEmail(emailmodel);


            _dbcontext.Entry(user).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();


            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Sent!"
            });


        }


        [HttpPost("activate-account")]
        public async Task<IActionResult> ResetPassword(ActivateAccountDto activate)
        {
            var newToken = activate.EmailToken.Replace(" ", "+");
            var user = await _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == activate.Email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User doesn't exist"
                });
            }

            var tokenCode = user.ActivateToken;
            DateTime emailTokenExpiry = user.ActivateTokenExpiry;

            if (tokenCode != activate.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid Reset Link"
                });
            }

            user.isActivated = true;

            _dbcontext.Entry(user).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();


            return Ok(new
            {
                StatusCode = 200,
                Message = "Account Successfully Activated!"
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            // check email
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist" });


            var passMessage = CheckPasswordStrength(userObj.Password);

            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });

            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            await _dbcontext.AddAsync(userObj);
            await _dbcontext.SaveChangesAsync();


            /*Répertorier l'activité*/
            string icon = "heroicons_solid:star";
            string description = "Thank you for registering to our platform!";
            DateTime date = DateTime.Now;
            string username = userObj.FullName;
            string extracontent = $@"<div class=""font-bold"">Congratulations for your registration!</div><br>
                                    <div>Hi {username},<br>We hope our platform will live up to your expectations and help you achieve your professional goals.</div>";


            Activity activity = new Activity
            {
                icon = icon,
                description = description,
                date = date,
                extraContent = extracontent,
                userEmail = userObj.Email
            };

            string title = "Subscription";

            Notification notification = new Notification
            {
                icon = icon,
                title = title,
                description = description,
                time = date,
                read = false,
                userEmail = userObj.Email

            };

            await _dbcontext.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();

            await _dbcontext.AddAsync(notification);
            await _dbcontext.SaveChangesAsync();


            return Ok(new
            {
                Status = 200,
                Message = "User Added!"
            });
        }



        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest();
            }
            var user = _dbcontext.Users.AsNoTracking().FirstOrDefault(x => x.Id == updatedUser.Id);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User Not Found"
                });
            }
            else
            {
                _dbcontext.Entry(updatedUser).State = EntityState.Modified;

                /*Répertorier l'activité*/
                string icon = "heroicons_solid:refresh";
                string description = "You successfully update your profile!";
                DateTime date = DateTime.Now;


                Activity activity = new Activity
                {
                    icon = icon,
                    description = description,
                    date = date
                };

                await _dbcontext.AddAsync(activity);
                await _dbcontext.SaveChangesAsync();


                _dbcontext.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "User Updated Successfully"
                });
            }
        }

      


        private Task<bool> CheckEmailExistAsync(string? email)
            => _dbcontext.Users.AnyAsync(x => x.Email == email);



        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            return sb.ToString();
        }

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.FullName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _dbcontext.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }


        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _dbcontext.Users.ToListAsync());
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _dbcontext.Users.FindAsync(id);
            return Ok(user);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid Client Request");
            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;
            var principal = GetPrincipleFromExpiredToken(accessToken);
            var name = principal.Identity.Name;

            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.FullName == name);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid Request");
            var newAccessToken = CreateJwt(user);
            var newRefreshToken = CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _dbcontext.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }


       


        [HttpPost("send-reset-email")]
        public async Task<IActionResult> SendEmail([FromQuery] string email)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email doesn't exist"
                });
            }

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);

            string from = _config["EmailSettings:From"];

            var emailmodel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));

            _service.SendEmail(emailmodel);

            _dbcontext.Entry(user).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();


            /*Répertorier l'activité*/
            string icon = "heroicons_solid:refresh";
            string description = "You requested reset password!";
            DateTime date = DateTime.Now;


            Activity activity = new Activity
            {
                icon = icon,
                description = description,
                date = date
            };

            await _dbcontext.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Sent!"
            });



        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
        {
            var newToken = reset.EmailToken.Replace(" ", "+");
            var user = await _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == reset.Email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User doesn't exist"
                });
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;
            if(tokenCode != reset.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid Reset Link"
                });
            }
            user.Password = PasswordHasher.HashPassword(reset.NewPassword);
            _dbcontext.Entry(user).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();


            /*Répertorier l'activité*/
            string icon = "heroicons_solid:refresh";
            string description = "Your password has been reset successfully!";
            DateTime date = DateTime.Now;


            Activity activity = new Activity
            {
                icon = icon,
                description = description,
                date = date,
                userEmail = user.Email
            };

            await _dbcontext.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "Password successfully reset!"
            });
        }



        [HttpPost("reset-pwd-without-link")]
        public async Task<IActionResult> ResetPasswordWithoutLink(ResetPasswordDto model)
        {
            // Retrieve the current user from the database
            var user = _dbcontext.Users.AsNoTracking().FirstOrDefault(x => x.Id == model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Verify the current password
            var passwordHasher = new PasswordHasher();
            if (!PasswordHasher.VerifyPassword(model.CurrentPassword, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            // Update the user's password
            user.Password = PasswordHasher.HashPassword(model.NewPassword);
            _dbcontext.Entry(user).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();


            /*Répertorier l'activité*/
            string icon = "heroicons_solid:refresh";
            string description = "You successfully changed your password!";
            DateTime date = DateTime.Now;


            Activity activity = new Activity
            {
                icon = icon,
                description = description,
                date = date,
                userEmail = user.Email
            };

            await _dbcontext.AddAsync(activity);
            await _dbcontext.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("GetUser")]
        public async Task<ActionResult<Activity>> GetUser([FromQuery] string email)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound(); // Retourner une réponse 404 si aucun utilisateur n'est trouvé
            }

            return Ok(user); // Retourner l'utilisateur trouvé
        }

        [HttpGet("getAllActivities")]
        public async Task<ActionResult<Activity>> GetAllActivities([FromQuery] string email)
        {
            var all = await _dbcontext.Activitys.ToListAsync();

            List<Activity> activities = all.Where(n => n.userEmail == email).ToList();

            return Ok(activities);
        }


        [HttpGet("getAllNotifications")]
        public async Task<ActionResult<Notification>> getAllNotifications([FromQuery] string? email)
        {
            var all = await _dbcontext.Notifications.ToListAsync();

            List<Notification> notifications = all.Where(n => n.userEmail == email).ToList();

            return Ok(notifications);
        }



    }
}

