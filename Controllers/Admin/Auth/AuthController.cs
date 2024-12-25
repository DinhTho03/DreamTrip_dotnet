using brandportal_dotnet.Constant;
using brandportal_dotnet.Contracts.Account;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using DnsClient;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using brandportal_dotnet.Contracts.Admin.User;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using MimeKit.Text;

namespace brandportal_dotnet.Controllers.Auth
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        string[] Scopes = { GmailService.Scope.GmailSend };
        private static string ApplicationName = "DreamTrip";
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<Account> _accountService;
        private readonly IConfiguration _configuration;
        private readonly IRepository<OtpManager> _otpManager;

        public AuthController(IRepository<Role> roleService, IRepository<Account> accountService,
            IConfiguration configuration, IRepository<OtpManager> otpManager)
        {
            _roleService = roleService;
            _accountService = accountService;
            _configuration = configuration;
            _otpManager = otpManager;
        }

        [HttpPost("~/api/user/confirm/register")]
        public async Task<IActionResult> ConfirmRegister(AccountDto accountDto)
        {
            bool otpresponse = await ValidateOTP(accountDto.Email, accountDto.Otptext);
            if (!otpresponse)
            {
                return BadRequest(new { message = "Nhập sai mã OTP hoặc mã OTP hết hạn" });
            }

            if (accountDto.Password != accountDto.ConfirmPassword)
            {
                return BadRequest(new { message = "Mật khẩu không trùng khớp" });
            }
            else
            {
                var password = BCrypt.Net.BCrypt.HashPassword(accountDto.Password, Constants.PasswordSalt);
                var dataUser = new Account()
                {
                    Email = accountDto.Email,
                    Password = password,
                    RegisterDate = DateTime.Now,
                    IsDeleted = false,
                    LoginDate = DateTime.Now,
                    Point = 0
                };
                await _accountService.Insert(dataUser);
                var user = new LoginDto
                {
                    Email = dataUser.Email,
                    Password = accountDto.Password
                };
                var dataCondition = new Dictionary<string, object>
                {
                    { "Email", accountDto.Email },
                };
                var data = await _otpManager.FindByProperties(dataCondition);
                await _otpManager.Delete(data._Id);
                return await AuthenticateUser(user);
            }
        }
        
       

        private async Task<bool> ValidateOTP(string email, string OTPText)
        {
            bool response = false;
            var dataCondition = new Dictionary<string, object>
            {
                { "Email", email },
                { "Otptext", OTPText },
                { "Expiration", Tuple.Create("gt", DateTime.Now) }
            };
            var data = await _otpManager.FindByProperties(dataCondition);
            if (data != null)
            {
                response = true;
            }

            return response;
        }

        [HttpPost("~/api/user/register-otp")]
        public async Task<IActionResult> RegisterUser(EmailRegister email)
        {
            var conditionRole = new Dictionary<string, object> { { "Name", "user" } };
            var roleFilter = await _roleService.FindByProperties(conditionRole);
            // var password = BCrypt.Net.BCrypt.HashPassword(accountDto.Password, Constants.PasswordSalt);
            var emailCondition = new Dictionary<string, object> { { "Email", email.email } };
            var emailExisted = await _accountService.FindByProperties(emailCondition);
            if (emailExisted != null)
            {
                return BadRequest(new { message = "Email này đã tồn tại" });
            }

            //
            // if (accountDto.Password != accountDto.ConfirmPassword)
            // {
            //     return BadRequest(new { message = "Mật khẩu không trùng khớp" });
            // }
            string OTPText = Generaterandomnumber();
            var dataUser = new OtpManager()
            {
                Email = email.email,
                Otptext = OTPText,
                Createddate = DateTime.Now,
                Expiration = DateTime.Now.AddMinutes(30),
                Otptype = "register",
            };
            await _otpManager.Insert(dataUser);
            await SendOtpMail(email.email, OTPText);

            return Ok();
        }
        
        [HttpPost("~/api/user/forgot-password")]
        public async Task<IActionResult> ForgotPassword(EmailRegister email)
        {
            var conditionRole = new Dictionary<string, object> { { "Name", "user" } };
            var roleFilter = await _roleService.FindByProperties(conditionRole);
            // var password = BCrypt.Net.BCrypt.HashPassword(accountDto.Password, Constants.PasswordSalt);
            var phoneCondition = new Dictionary<string, object> { { "Email", email.email } };
            var emailExisted = await _accountService.FindByProperties(phoneCondition);
            if (emailExisted == null)
            {
                return BadRequest(new { message = "Email này Không tồn tại" });
            }
            string OTPText = Generaterandomnumber();
            var dataUser = new OtpManager()
            {
                Email = email.email,
                Otptext = OTPText,
                Createddate = DateTime.Now,
                Expiration = DateTime.Now.AddMinutes(30),
                Otptype = "forgotPassword",
            };
            await _otpManager.Insert(dataUser);
            await SendOtpMail(email.email, OTPText);

            return Ok();
        }
        
        [HttpPost("~/api/user/confirm/forgot-password")]
        public async Task<IActionResult> ConfirmForgotPassword(AccountDto accountDto)
        {
            bool otpresponse = await ValidateOTP(accountDto.Email, accountDto.Otptext);
            if (!otpresponse)
            {
                return BadRequest(new { message = "Nhập sai mã OTP hoặc mã OTP hết hạn" });
            }

            if (accountDto.Password != accountDto.ConfirmPassword)
            {
                return BadRequest(new { message = "Mật khẩu không trùng khớp" });
            }
            else
            {
                var password = BCrypt.Net.BCrypt.HashPassword(accountDto.Password, Constants.PasswordSalt);
        
                var userCondition = new Dictionary<string, object>
                {
                    { "Email", accountDto.Email }
                };
                var user = await _accountService.FindByProperties(userCondition);
                user.Password = password;
                user.LoginDate = DateTime.Now;
                
        
                await _accountService.Update(user._Id, user);

                var userLogin = new LoginDto
                {
                    Email = accountDto.Email,
                    Password = accountDto.Password
                };

                // Delete the used OTP
                var dataCondition = new Dictionary<string, object>
                {
                    { "Email", accountDto.Email },
                };
                var otpData = await _otpManager.FindByProperties(dataCondition);
                await _otpManager.Delete(otpData._Id);

                return await AuthenticateUser(userLogin);
            }
        }


        private void SendEmailWithGmailAPI(Mailrequest mailrequest)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailSettings:Email").Value));
            email.To.Add(MailboxAddress.Parse(mailrequest.Email));
            email.Subject = mailrequest.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = mailrequest.Emailbody };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailSettings:Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration.GetSection("EmailSettings:Email").Value,
                _configuration.GetSection("EmailSettings:Password").Value); // Use App Password here
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        private async Task UpdateOtp(string email, string otptext, string otptype)
        {
            var _opt = new OtpManager()
            {
                Email = email,
                Otptext = otptext,
                Expiration = DateTime.Now.AddMinutes(30),
                Createddate = DateTime.Now,
                Otptype = otptype
            };
            await _otpManager.Insert(_opt);
        }

        private string Generaterandomnumber()
        {
            Random random = new Random();
            string randomno = random.Next(0, 1000000).ToString("D6");
            return randomno;
        }

        private async Task SendOtpMail(string useremail, string OtpText)
        {
            var mailrequest = new Mailrequest();
            mailrequest.Email = useremail;
            mailrequest.Subject = "Thanks for registering : OTP";
            mailrequest.Emailbody = GenerateEmailBody(OtpText);
            SendEmailWithGmailAPI(mailrequest);
        }

        private string GenerateEmailBody(string otptext)
        {
            string emailbody = "<div>";
            emailbody += "<p>Hi you, Thanks for registering</p>";
            emailbody += "<p>Please enter OTP text and complete the registration</p>";
            emailbody += "<p>OTP Text is: <span style=\"font-weight: 900\">" + otptext + "</span></p>";
            emailbody += "</div>";
            return emailbody;
        }

        [HttpPost("~/api/user/login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var phoneCondition = new Dictionary<string, object> { { "Email", login.Email } };
            var userFilter = await _accountService.FindByProperties(phoneCondition);
            if (userFilter == null)
            {
                return BadRequest(new { message = "Email hoặc mật khẩu không trùng khớp" });
            }

            var passwordHash = BCrypt.Net.BCrypt.Verify(login.Password, userFilter.Password);
            if (passwordHash)
            {
                return await AuthenticateUser(login);
            }

            return BadRequest(new { message = "Email hoặc mật khẩu không trùng khớp" });
        }

        private async Task<IActionResult> AuthenticateUser(LoginDto login)
        {
            // Check user by email if phone is not found
            var emailCondition = new Dictionary<string, object>
            {
                { "Email", login.Email }
            };

            var emailUser = await _accountService.FindByProperties(emailCondition);

            if (emailUser == null)
            {
                BadRequest(new
                {
                    message = ("User not found")
                });
            }


            // Verify password using bcrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, emailUser.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid credentials");
            }

            // Get user role (if needed)
            var roleCondition = new Dictionary<string, object>
            {
                { "Name", "user" }
            };
            var role = await _roleService.FindByProperties(roleCondition);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            // Return user information
            return Ok(new UserDto
            {
                UserId = emailUser._Id,
                Role = role.Name,
                Phone = emailUser.Phone,
                Email = emailUser.Email,
                Avatar = emailUser.Avatar,
                Point = emailUser.Point,
            });
        }


        [HttpPost("~/api/user/google/login")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] CredentialDto data) // Sử dụng DTO để nhận dữ liệu từ client
        {
            // Cấu hình xác thực Google ID token với Client ID của bạn
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    this._configuration.GetSection("App:GoogleClientId").Value
                } // Đảm bảo GoogleClientId được lấy đúng
            };

            // Xác thực Google ID token
            var payloadTask = Task.Run(() => GoogleJsonWebSignature.ValidateAsync(data.credential, settings));
            // Thực hiện các tác vụ khác ở đây nếu cần
            var payload = await payloadTask;

            // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
            var fullNameCondition = new Dictionary<string, object>
            {
                { "Email", payload.Email }
            };
            var user = await _accountService.FindByProperties(fullNameCondition);

            // Nếu người dùng đã tồn tại, tạo token và trả về
            if (user != null)
            {
                var login = new LoginDto
                {
                    Password = payload.Subject,
                    Email = user.Email,
                };
                return await AuthenticateUser(login);
            }
            else
            {
                // Nếu người dùng chưa tồn tại, tạo tài khoản mới và trả về token
                var password = BCrypt.Net.BCrypt.HashPassword(payload.Subject, Constants.PasswordSalt);

                var conditionRole = new Dictionary<string, object> { { "Name", "user" } };
                var roleFilter = await _roleService.FindByProperties(conditionRole);

                var newUser = new Account
                {
                    Password = password,
                    Phone = "", // Không có số điện thoại trong thông tin Google
                    Email = payload.Email,
                    FullName = payload.Name,
                    RegisterDate = DateTime.Now,
                    RoleId = roleFilter._Id,
                    Avatar = user.Avatar,
                    IsDeleted = false,
                    Point = 0
                };

                await _accountService.Insert(newUser);

                var login = new LoginDto
                {
                    Password = newUser.Password,
                    Email = newUser.Email
                };
                var tokenDto = await AuthenticateUser(login);

                return Ok(tokenDto);
            }
        }
        
        [HttpGet("~/api/user/point/{userId}")]
        public async Task<IActionResult> GetPointToPlayGameAsync(string userId)
        {
            var user = await _accountService.GetById(userId);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "User not found"
                });
            }
            
            var data = new PointUserDto
            {
                Id = user._Id,
                Point = user.Point
            };

            return Ok(data);

        }
    }
    
    
    
}