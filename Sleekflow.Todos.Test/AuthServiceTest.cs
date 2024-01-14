using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sleekflow.Todos.Core.Models;
using Sleekflow.Todos.Core.Services;
using Sleekflow.Todos.DAL;
using Sleekflow.Todos.Test.Mocks;

namespace Sleekflow.Todos.Test;

public class AuthServiceTest : IDisposable
{
    private readonly TodoContext _conetxt;
    private readonly IAuthService _authService;
    private readonly SqliteConnection _connection;

    public AuthServiceTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseSqlite(_connection)
            .Options;

        _conetxt = new TodoContext(options);
        _conetxt.Database.EnsureDeleted();
        _conetxt.Database.EnsureCreated();

        _conetxt.Users.AddRange(MockUsers.GetList());

        _conetxt.SaveChanges();

        var inMemorySettings = new Dictionary<string, string> {
            {"JwtSettings:Issuer", "Issuer"},
            {"JwtSettings:SignKey", $"{new Guid()}"},
            {"JwtSettings:ExpiredTime", "1800"},
        };

        var mockConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _authService = new AuthService(mockConfig, _conetxt);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    [Fact]
    public async void SignupAsync_Success()
    {
        var user = new UserModel()
        {
            UserName = "mockUser",
            Password = "mockUserPassword"
        };

        var result = await _authService.SignupAsync(user);

        Assert.Empty(result.Errors);
    }

    [Fact]
    public async void SignupAsync_Exist_Fail()
    {
        var user = new UserModel()
        {
            UserName = "Exist",
            Password = "mockUserPassword"
        };

        var result = await _authService.SignupAsync(user);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<AuthError>(result.Errors.First());
    }

    [Fact]
    public async void LoginAsync_UserNotExist_Fail()
    {
        var user = new UserModel()
        {
            UserName = "Exist1",
            Password = "123"
        };

        var result = await _authService.LoginAsync(user);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<AuthError>(result.Errors.First());
    }

    [Fact]
    public async void LoginAsync_PasswordIncorrect_Fail()
    {
        var user = new UserModel()
        {
            UserName = "Exist",
            Password = "1234"
        };

        var result = await _authService.LoginAsync(user);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<AuthError>(result.Errors.First());
    }

    [Fact]
    public async void LoginAsync_Suspended_Fail()
    {
        var user = new UserModel()
        {
            UserName = "Suspended",
            Password = "123"
        };

        var result = await _authService.LoginAsync(user);

        Assert.NotEmpty(result.Errors);
        Assert.IsType<AuthError>(result.Errors.First());
    }

    [Fact]
    public async void LoginAsync_Success()
    {
        var user = new UserModel()
        {
            UserName = "Exist",
            Password = "123"
        };

        var result = await _authService.LoginAsync(user);

        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data.Token);

        Assert.Empty(result.Errors);
    }
}