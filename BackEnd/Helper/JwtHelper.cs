namespace BackEnd.Helper;

using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class JwtHelper : ControllerBase
{
    private readonly CrdpCurriculumMsContext _context;
    private readonly IConfiguration _configuration;

    public JwtHelper(CrdpCurriculumMsContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<UserInfo> GetUserInfoFromJwt(string jwtToken)
    {
        try
        {
            if (jwtToken.StartsWith("Bearer "))
            {
                jwtToken = jwtToken.Substring("Bearer ".Length);
            }

            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = false,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]))
            };

            var claims = handler.ValidateToken(jwtToken, validationParameters, out _);

            // Check for both "userId" and "sub" claims
            var userIdClaim = claims.Claims.FirstOrDefault(c => c.Type == "userId") ??
                              claims.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
            {
                throw new Exception("No userId claim found in token.");
            }

            var userId = int.Parse(userIdClaim.Value);

            // Fetch the user's roles from the database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var userRoleIds = await _context.UserRoles
                                            .Where(ur => ur.UserId == user.Id)
                                            .Select(ur => ur.RoleId)
                                            .ToListAsync();

            // Fetch the services associated with the user's roles
            var roleServices = await _context.RoleServices
                                             .Where(rs => userRoleIds.Contains(rs.RoleId))
                                             .Include(rs => rs.Service)
                                             .ToListAsync();

            var serviceNames = roleServices.Select(rs => rs.Service.ServiceName).Distinct().ToList();

            return new UserInfo
            {
                UserId = userId,
                RoleIds = userRoleIds,
                ServiceNames = serviceNames
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserInfoFromJwt: {ex.Message}");
            return null;
        }
    }
}

    public class UserInfo
    {
        public int? UserId { get; set; }
        public List<int?>RoleIds { get; set; }
        public List<string>? ServiceNames { get; set; }
    }