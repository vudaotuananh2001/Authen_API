using JWT_API.DTO;
using JWT_API.Models;
using BCrypt.Net;
using JWT_API.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_API.Repository.Authen
{
    public class AuthencationImpl : IAuthen
    {
        private readonly int HASHPASSWORD = 10;
        private readonly string _secretKey;
        private readonly int _expiresInDays;
        private readonly ApplicationContext _applicationContext;

        public AuthencationImpl(ApplicationContext applicationContext, IConfiguration configuration)
        {
            _applicationContext = applicationContext;
            _secretKey = configuration["AppSettings:SecretKey"];
            _expiresInDays = 1;
        }

        public string Login(UserLoginDTO loginDTO)
        {
            try
            {
                User user = _applicationContext.Users
                    .Where(user => user.Email.Equals(loginDTO.Email)).FirstOrDefault();

                if (user != null)
                {
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
                    if (isPasswordValid)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_secretKey);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[]
                            {
                                new Claim(ClaimTypes.Name, user.Id.ToString()),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User") // Thêm vai trò vào token, mặc định là "User" nếu vai trò không tồn tại
                            }),
                            Expires = DateTime.UtcNow.AddDays(_expiresInDays), // Sử dụng thời gian sống từ cấu hình
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        return tokenHandler.WriteToken(token);
                    }
                    else
                    {
                        // Password không hợp lệ
                        throw new UnauthorizedAccessException("Invalid password.");
                    }
                }
                else
                {
                    // User không tồn tại
                    throw new ArgumentException("User not found.");
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi chi tiết
                Console.WriteLine($"An error occurred during login: {ex.Message}");
                throw; // Ném lại ngoại lệ để thông báo cho lớp gọi
            }
        }

        public User Register(UserDTO userDTO)
        {
            try
            {
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password, HASHPASSWORD);

                User user = new User
                {
                    Address = userDTO.Address,
                    Email = userDTO.Email,
                    Role_Id = userDTO.Role_Id,
                    Password = hashPassword,
                    Status = userDTO.Status
                };

                user = _applicationContext.Users.Add(user).Entity;
                _applicationContext.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error repository " + ex.Message);
            }
        }
    }
}
