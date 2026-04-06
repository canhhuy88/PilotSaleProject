using System.Threading.Tasks;
using BizI.Application.Common;
using BizI.Domain.Enums;

namespace BizI.Application.Interfaces;

public interface IAuthService
{
    Task<CommandResult> LoginAsync(string username, string password);
    Task<CommandResult> RegisterAsync(string username, string password, UserRole role);
}
