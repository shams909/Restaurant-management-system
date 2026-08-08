using RMS.Application.DTOs;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
}
