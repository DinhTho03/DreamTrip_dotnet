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

namespace brandportal_dotnet.Controllers.Auth
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<Role> _roleService;
        private readonly IRepository<Account> _accountService;
        private readonly IConfiguration _configuration;

        public AuthController(IRepository<Role> roleService, IRepository<Account> accountService,
            IConfiguration configuration)
        {
            _roleService = roleService;
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost("~/api/user/register")]
        public async Task<IActionResult> Register(AccountDto accountDto)
        {
            var conditionRole = new Dictionary<string, object> { { "Name", "user" } };
            var roleFilter = await _roleService.FindByProperties(conditionRole);
            var password = BCrypt.Net.BCrypt.HashPassword(accountDto.Password, Constants.PasswordSalt);
            var phoneCondition = new Dictionary<string, object> { { "Phone", accountDto.Phone } };
            var emailExisted = await _accountService.FindByProperties(phoneCondition);
            if (emailExisted != null)
            {
                return BadRequest(new { message = "Email này đã tồn tại" });
            }

            if (accountDto.Password != accountDto.ConfirmPassword)
            {
                return BadRequest(new { message = "Mật khẩu không trùng khớp" });
            }

            var dataUser = new Account
            {
                Password = password,
                Phone = accountDto.Phone,
                RegisterDate = DateTime.Now,
                RoleId = roleFilter._Id,
                IsDeleted = false
            };
            await _accountService.Insert(dataUser);
            var user = new LoginDto
            {
                Phone = dataUser.Phone,
                Password = accountDto.Password
            };
            
            return Ok(await AuthenticateUser(user));
        }

        [HttpPost("~/api/user/login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var phoneCondition = new Dictionary<string, object> { { "Phone", login.Phone } };
            var userFilter = await _accountService.FindByProperties(phoneCondition);
            if (userFilter == null)
            {
                return BadRequest(new { message = "Số điện thoại hoặc mật khẩu không trùng khớp" });
            }

            var passwordHash = BCrypt.Net.BCrypt.Verify(login.Password, userFilter.Password);
            if (passwordHash)
            {
                var token = await AuthenticateUser(login);
                return Ok(await AuthenticateUser(login));
            }

            return BadRequest(new { message = "Số điện thoại hoặc mật khẩu không trùng khớp" });
        }

        private async Task<UserDto> AuthenticateUser(LoginDto login)
        {
            // Check user by phone
            var phoneCondition = new Dictionary<string, object>
            {
                { "Phone", login.Phone }
            };
            var user = await _accountService.FindByProperties(phoneCondition); // Assume user contains password hash

            // Check user by email if phone is not found
            var emailCondition = new Dictionary<string, object>
            {
                { "Email", login.Email }
            };

            var emailUser = await _accountService.FindByProperties(emailCondition);

            if (user == null && emailUser == null)
            {
                throw new Exception("User not found");
            }

            // If phoneUser is null, use emailUser instead
            user = user ?? emailUser;

            // Verify password using bcrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
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
            return new UserDto
            {
                UserId = user._Id.ToString(),
                Role = role.Name,
                Phone = user.Phone,
                Email = user.Email,
                Avatar = user.Avatar,
            };
        }


        [HttpPost("~/api/user/google/login")]
        public async Task<IActionResult>
            LoginWithGoogle([FromBody] CredentialDto data) // Sử dụng DTO để nhận dữ liệu từ client
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
                    Phone = user.Phone,
                    Email = user.Email,
                };
                return Ok(await AuthenticateUser(login));
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
                    Avatar = payload.Picture,
                    IsDeleted = false
                };

                await _accountService.Insert(newUser);

                var login = new LoginDto
                {
                    Password = newUser.Password,
                    Phone = newUser.Phone,
                    Email = newUser.Email
                };
                var tokenDto = await AuthenticateUser(login);

                return Ok(tokenDto);
            }
        }
    }
}