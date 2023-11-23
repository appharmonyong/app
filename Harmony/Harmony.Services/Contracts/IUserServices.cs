using Harmony.Bussiness.Services.Contracts.Common;
using Harmony.Bussiness.ViewModel;

namespace Harmony.Bussiness.Services.Contracts
{
    public interface IUserServices: IReadable<UserVm>, IEditable<UserVm>
    {
        Task<UserVm> SignIn(UserLogInVm user);
        Task<UserVm> Register(UserRegisterVm user);

    }
}