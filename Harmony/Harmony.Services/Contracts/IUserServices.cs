using Harmony.Bussiness.Services.Contracts.Common;
using Harmony.Bussiness.ViewModel;

namespace Harmony.Bussiness.Services.Contracts
{
    public interface IUserServices : IReadable<UserVm>
    {
        Task<UserVm> SignIn(UserLogInVm user);
        Task<IEnumerable<UserVm>> Get();
        Task<UserVm> GetByName(string name);
        Task<UserVm>? GetById(int id);
        Task<IEnumerable<UserVm>> GetCertainProperties(); //Me permite asegurar que las entidades que no esten activas no se muestren
        
        Task<UserVm> Create(UserRegisterVm entity);
        Task<UserVm> Update(int id, UserUpdateVm entity);
        Task<bool> Delete(int id);

        Task<bool> GetEType(int id); //A traves de este metodo se buscaran los tipos dentro del Enum para las distintas operaciones de eliminar entre cuentas

        // Nuevo método para el registro de usuarios
        Task<UserVm> Register(UserRegisterVm userVm);

    }
}