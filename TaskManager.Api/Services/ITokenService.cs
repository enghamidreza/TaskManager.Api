using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

public interface ITokenService
{
    //دوتا خروجی میده :string Token, DateTime ExpiresAt
    (string Token, DateTime ExpiresAt) CreateToken(AppUser user);
}