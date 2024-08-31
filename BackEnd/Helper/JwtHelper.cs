namespace BackEnd.Helper;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BackEnd.Models;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

using System.Security.Claims;

using Newtonsoft.Json;
using BackEnd.Data;
using BackEnd.Class;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
public class JwtHelper : ControllerBase
{
    private readonly CrdpCurriculumMsContext _context;
    private readonly IConfiguration _configuration;

    public JwtHelper(CrdpCurriculumMsContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<UserInfo> GetUserInfoFromJwt(string jwtToken, string page)
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
            var userId = int.Parse(claims.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);

            // Fetch the user's roles from the database
            var user = await _context.Users.FindAsync(userId);

            var userRoleIds = await _context.UserRoles.Where(ur => ur.UserId == user.Id)
                                                 .Select(ur => ur.RoleId)
                                                 .ToListAsync();

            // Fetch the services associated with the user's roles
            var roleServices = await _context.RoleServices
                                          .Where(rs => userRoleIds.Contains(rs.RoleId))
                                          .Include(rs => rs.Service)
                                          .ToListAsync();

            var serviceNames = roleServices
                .Where(rs => rs.Service.Clurl == page)
                .Select(rs => rs.Service.Title)
                .Distinct()
                .ToList();

            return new UserInfo
            {
                UserId = userId,
                RoleIds = userRoleIds,
                ServiceNames = serviceNames,
                isAdmin = user.Isadmin,
            };
        }
        catch (Exception ex)
        {
            var m = ex.Message;
            return null;
        }
    }

    public async Task<PermissionCheckResponse> CheckPermission(HttpRequest request, string PageName, string requiredPermission)
    {
        var token = request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(token))
        {
            return new PermissionCheckResponse
            {
                Success = false,
                Message = "No token provided"
            };
        }

        var userInfo = await GetUserInfoFromJwt(token, PageName);
        if (userInfo == null)
        {
            return new PermissionCheckResponse
            {
                Success = false,
                Message = "Not authorized"
            };
        }


        if (userInfo.isAdmin ?? false)
        {
            return new PermissionCheckResponse
            {
                Success = true,
                Message = "Permission granted"
            };
        }

        if (!userInfo.ServiceNames.Any(sn => sn.Contains(requiredPermission)))
        {
            return new PermissionCheckResponse
            {
                Success = false,
                Message = $"User does not have the '{requiredPermission}' permission"
            };
        }
        return new PermissionCheckResponse
        {
            Success = true,
            Message = "Permission granted"
        };
    }




public async Task<Dictionary<string, bool>> CheckMultiplePermissions(HttpRequest request, string[] pageNames)
{
    var token = request.Headers["Authorization"].FirstOrDefault();
    if (string.IsNullOrEmpty(token))
    {
        return new Dictionary<string, bool>
        {
            { "No token provided", false }
        };
    }

    var pagePermissions = new Dictionary<string, bool>();
    foreach (var pageName in pageNames)
    {

        var userInfo = await GetUserInfoFromJwt(token, pageName);
        if (userInfo == null)
        {
            return new Dictionary<string, bool>
            {
                { "Not authorized", false }
            };
        }
        if (userInfo.isAdmin ?? false) {
                pagePermissions[pageName] = true;
        }
        else
        {
                if (userInfo.ServiceNames != null)
                {
                    bool hasManageOrViewPermission = userInfo.ServiceNames.Any(sn => sn.Contains("manage") || sn.Contains("view"));
                    pagePermissions[pageName] = hasManageOrViewPermission;
                }
                else
                {
                    pagePermissions[pageName] = false;

                }

            }
           
    }

    return pagePermissions;
}

}









public class UserInfo
{
    public int? UserId { get; set; }
    public Boolean? isAdmin { get; set; }
    public List<int?> RoleIds { get; set; }
    public List<string>? ServiceNames { get; set; }
}

public class PermissionCheckResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class Permissions
{
    public bool CanEdit { get; set; }
    public bool CanAdd { get; set; }
    public bool CanDelete { get; set; }
    public bool CanView { get; set; }
}
