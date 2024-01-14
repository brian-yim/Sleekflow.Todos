using Sleekflow.Todos.Core.Models;

namespace Sleekflow.Todos.Core.Services;

public interface IAuthService
{
    Task<ServiceResponseModel<TokenModel>> LoginAsync(UserModel model);
    Task<ServiceResponseModel> SignupAsync(UserModel model);
}