using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BusinessCentralApi.Data;
using BusinessCentralApi.Models;
using System.Security.Cryptography;

namespace BusinessCentralApi.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto model);
        Task<User> RegisterAsync(AuthRequestDto model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly BusinessCentralContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            BusinessCentralContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto model)
        {
            // Find user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            // Validate credentials
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Generate token
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(2),
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<User> RegisterAsync(AuthRequestDto model)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Create new user
            var user = new User
            {
                Username = model.Username,
                PasswordHash = HashPassword(model.Password),
                Role = "User" // Default role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            if (secretKey == null)
            {
                throw new InvalidOperationException("Jwt:SecretKey is not configured");
            }
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            );
            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            // Use PBKDF2 for secure password hashing
            using (var deriveBytes = new Rfc2898DeriveBytes(
                password,
                saltSize: 16,
                iterations: 10000,
                HashAlgorithmName.SHA256))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] subkey = deriveBytes.GetBytes(32); // 256 bits

                var outputBytes = new byte[49]; // 16 (salt) + 32 (subkey) + 1 (version marker)
                outputBytes[0] = 0x01; // Version marker
                Buffer.BlockCopy(salt, 0, outputBytes, 1, 16);
                Buffer.BlockCopy(subkey, 0, outputBytes, 17, 32);

                return Convert.ToBase64String(outputBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] outputBytes = Convert.FromBase64String(hashedPassword);

            // Extract version marker, salt, and subkey
            if (outputBytes[0] != 0x01)
                return false;

            byte[] salt = new byte[16];
            Buffer.BlockCopy(outputBytes, 1, salt, 0, 16);

            byte[] expectedSubkey = new byte[32];
            Buffer.BlockCopy(outputBytes, 17, expectedSubkey, 0, 32);

            // Verify the password
            using (var deriveBytes = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations: 10000,
                HashAlgorithmName.SHA256))
            {
                byte[] actualSubkey = deriveBytes.GetBytes(32);
                return actualSubkey.SequenceEqual(expectedSubkey);
            }
        }
    }
}