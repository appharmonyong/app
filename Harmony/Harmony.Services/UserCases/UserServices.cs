using AutoMapper;
using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Harmony.Common;
using Harmony.Common.Extensions;
using Harmony.Persistence.Context;
using Harmony.Persistence.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Harmony.Bussiness.Services.UserCases
{
    public class UserServices : IUserServices
    {
   
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Method for create user
        /// </summary>
        /// <param name="user">User vm for register</param>
        /// <returns>UserVm object</returns>
        public async Task<UserVm> SignIn(UserLogInVm user)
        {
            var dbUser = _context.User.FirstOrDefault(x =>
         x.Email!.Trim() == user.Email.Trim() || x.UserName!.Trim() == user.Email.Trim());

            if (dbUser != null)
            {
                if (!dbUser.IsActive || dbUser.IsDelete)
                    throw new ArgumentNullException(EMensajesSistema.USUARIO_DESHABILITADO_ELIMINADO.GetDescription());

                string hash = Algorithm.HashPassword(dbUser.Salt, user.Password);
                if (dbUser.Hash == hash)
                    return await GetById(dbUser.Id)!;
            }

            throw new ArgumentNullException(EMensajesSistema.USUARIO_NO_EXISTENTE.GetDescription(), EMensajesSistema.USUARIO_NO_EXISTENTE.GetDescription());
        }

        public async Task<UserVm> Register(UserRegisterVm userVm)
        {
            // Verificar si ya existe un usuario con el mismo correo electrónico o nombre de usuario
            if (_context.User.Any(x => x.Email == userVm.Email || x.UserName == userVm.FirstName))
            {
                throw new ArgumentException(EMensajesSistema.USUARIO_EXISTENTE.GetDescription());
            }

            // Mapear el modelo de vista a la entidad de usuario
            var newUser = _mapper.Map<UserEntity>(userVm);

            // Generar salt y hash para la contraseña
            newUser.Salt = Algorithm.GenerateSalt();
            newUser.Hash = Algorithm.HashPassword(newUser.Salt, userVm.Password);

            // Configurar el nombre de usuario (podrías ajustar esto según tus necesidades)
            newUser.UserName = userVm.Email.Split("@")[0];

            // Agregar el nuevo usuario al contexto y guardar en la base de datos
            try
            {
                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("Violation of PRIMARY KEY constraint") == true)
                {
                    throw new ArgumentException(EMensajesSistema.USUARIO_EXISTENTE.GetDescription());
                }
                else
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            // Devolver el nuevo usuario creado
            return await GetById(newUser.Id) ?? throw new ArgumentException("Error al recuperar el usuario recién creado.");
        }

    public async Task<IEnumerable<UserVm>> Get()
        {
            return _mapper.Map<List<UserEntity>, List<UserVm>>(await _context.User.ToListAsync());
        }

        public async Task<UserVm>? GetById(int id)
        {
            var user = await _context.User.FindAsync(id);

            return user is null ? throw new ArgumentNullException(EMensajesSistema.USUARIO_NO_EXISTENTE.GetDescription()) : _mapper.Map<UserEntity, UserVm>(user);
        }

        public async Task<UserVm> Create(UserRegisterVm userVm)
        {
            var dbObject = _mapper.Map<UserEntity>(userVm);

            dbObject.Salt = Algorithm.GenerateSalt();
            dbObject.Hash = Algorithm.HashPassword(dbObject.Salt, userVm.Password);
            dbObject.UserName = dbObject.Email!.Split("@")[0];

            try
            {

                await _context.User.AddAsync(dbObject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException!.Message.Contains("Violation of PRIMARY KEY constraint"))
                    throw new ArgumentNullException(EMensajesSistema.USUARIO_EXISTENTE.GetDescription());
                else
                {
                    throw new ArgumentNullException(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            return await GetById(dbObject.Id)!;

        }

        public async Task<UserVm> Update(int id, UserUpdateVm entity)
        {

            // Buscar el usuario por ID
            var userToUpdate = _context.User.Find(id);

            if (userToUpdate is null)
            {
                throw new ArgumentNullException(EMensajesSistema.USUARIO_NO_EXISTENTE.GetDescription());
            }

            userToUpdate.UpdateChangedProperties(entity);
            _context.User.Update(userToUpdate);
            await _context.SaveChangesAsync();

            return await GetById(id)!;

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var userToBeDeleted = _context.User.Find(id) ?? throw new ArgumentNullException(EMensajesSistema.USUARIO_NO_EXISTENTE.GetDescription());
                userToBeDeleted.IsActive = false;
                _context.User.Update(userToBeDeleted);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }

        }

        public async Task<IEnumerable<UserVm>> GetCertainProperties()
        {
            return _mapper.Map<IEnumerable<UserVm>>(await _context.User.Where(us => us.IsActive && !us.IsDelete).ToListAsync());
        }


        //El objetivo de este metodo es hacer una verificación del tipo de cuenta del usuario antes de hacer la eliminación, esto es para
        //que aunque halla un sólo Delete dentro del controlador para usuarios, pueda realizar opciones distintas para el admin y el
        //regular.
        public async Task<bool> GetEType(int id)
        {

            var fetchId= await _context.User.FindAsync(id);
            if(fetchId.UserType== EUserTypes.Administrator)
            {
                return true; //Cambiar cuando se haga el borrar de admin
            } else if(fetchId.UserType == EUserTypes.Worker)
            {
                await this.Delete(fetchId.Id);
                fetchId.IsDelete= true;
                return true;
            }
            return false;
        }


        public async Task<UserVm> GetByName(string name)
        {
            var user= await _context.User.FirstOrDefaultAsync(us => us.UserName == name);
            return _mapper.Map<UserEntity, UserVm>(user);
        }
    }
}
