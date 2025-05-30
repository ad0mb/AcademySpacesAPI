using System.Security.Claims;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreatePermissionsJwtUseCase
{
    Task<string> CreatePermissionsJwtTokenAsync(IEnumerable<Claim> claims);
}