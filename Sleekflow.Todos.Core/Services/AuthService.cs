using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.DAL.Models;

namespace Sleekflow.Todos.Core.Services;

public class AuthService(IConfiguration config, TodoContext context) : IAuthService
{
    private readonly IConfiguration _config = config;
    private readonly TodoContext _context = context;

    public async Task<ServiceResponseModel<TokenModel>> LoginAsync(UserModel model)
    {
        var result = new ServiceResponseModel<TokenModel>();
        var user = await _context.Users
            .Where(user => user.UserName == model.UserName)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            result.Errors.Add(new AuthError());
            return result;
        }

        GenerateJwtToken(user);

        var isValid = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.PasswordHash);

        if (!isValid)
        {
            result.Errors.Add(new AuthError());
            return result;
        }

        result.Data = new()
        {
            Token = GenerateJwtToken(user)
        };
        return result;
    }

    public async Task<ServiceResponseModel> SignupAsync(UserModel model)
    {
        var result = new ServiceResponseModel();
        try
        {
            var user = await _context.Users
               .Where(user => user.UserName == model.UserName)
               .FirstOrDefaultAsync();

            if (user != null)
            {
                result.Errors.Add(new AuthError("User is already exist."));
                return result;
            }

            var signUpUser = new User()
            {
                UserName = model.UserName,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password),
                IsActive = true,
                CreatedBy = model.UserName,
                UpdatedBy = model.UserName,
            };

            await _context.Users.AddAsync(signUpUser);
            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            result.Errors.Add(new ErrorModel(ex.Message));
        }

        return result;
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SignKey"] ?? "thisIsAKey")
        );
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, $"{user.Id}"),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Issuer"],
            claims,
            expires: DateTime.Now.AddSeconds(
                Convert.ToDouble(_config["JwtSettings:ExpiredTime"])
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}