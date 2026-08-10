using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. Fetch all users and find the one trying to log in
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var user = users.FirstOrDefault(u => u.EmployeeNo == loginDto.EmployeeNo);

            // 2. If user doesn't exist, or BCrypt says the password is wrong, reject them!
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid Employee Number or Password");
            }

            // [NEW]: Since TenantId is directly on the user, we no longer need to do a slow database lookup for the Branch!
            var tenantId = user.TenantId.ToString();
            
            // If the user is a SuperAdmin (CEO), they might not have a BranchId. We use "0" as a fallback so the JWT doesn't crash.
            var branchIdString = user.BranchId.HasValue ? user.BranchId.Value.ToString() : "0";

            // 3. Grab the master key from the .env file
            var secretKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            // 4. Create the claims (What the token says about the user)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("employeeNo", user.EmployeeNo),
                    
                    // [NEW CLAIMS]: The API will now read these from the token on every request!
                    new Claim("tenantId", tenantId),
                    new Claim("branchId", branchIdString),
                    
                    // Convert the integer RoleId into the exact string the Controllers are looking for!
                    new Claim(ClaimTypes.Role, user.RoleId == 1 ? "SuperAdmin" : 
                                               user.RoleId == 2 ? "Manager" : 
                                               user.RoleId == 3 ? "Chef" : "Waiter")
                }),
                Expires = DateTime.UtcNow.AddHours(8), // Token expires in 8 hours (a standard shift)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            // 5. Generate the physical token string
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 6. Return the VIP Pass to the Waiter/Manager!
            return new AuthResponseDto
            {
                FullName = user.FullName,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
