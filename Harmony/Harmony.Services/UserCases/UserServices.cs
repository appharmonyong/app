﻿using AutoMapper;
using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Harmony.Common;
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

        public async Task<UserVm> SignIn(UserLogInVm user)
        {
            var dbObject = _context.User.FirstOrDefault(x =>
            x.Email!.Trim() == user.Email.Trim() || x.UserName!.Trim() == user.Email.Trim());

            if (dbObject != null)
            {
                string hash = Algorithm.HashPassword(dbObject.Salt, user.Password);
                if (dbObject.Hash == hash)
                    return await GetById(dbObject.Id)!;
            }

            throw new ArgumentNullException("Usuario no existente");
        }

        public async Task<IEnumerable<UserVm>> Get()
        {
            return _mapper.Map<List<UserEntity>, List<UserVm>>(await _context.User.ToListAsync());
        }

        public async Task<UserVm>? GetById(int id)
        {
            var user = await _context.User.FindAsync(id);

            return user is null ? throw new ArgumentNullException("Usuario no existente") : _mapper.Map<UserEntity, UserVm>(user);
        }

        public async Task<UserVm> Register(UserRegisterVm userVm)
        {
            var dbObject = _mapper.Map<UserEntity>(userVm);

            //CheckUser(userVm);

            dbObject.Salt = Algorithm.GenerateSalt();
            dbObject.Hash = Algorithm.HashPassword(dbObject.Salt, userVm.Password);
            dbObject.UserName = dbObject.Email!.Split("@")[0];

            try
            {

                await _context.User.AddAsync(dbObject);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await GetById(dbObject.Id)!;

        }

        public Task<UserVm> Create(UserVm entity)
        {
            throw new NotImplementedException();
        }

        public Task<UserVm> Update(int id, UserVm entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var userToBeDeleted = _context.User.Find(id);
                if (userToBeDeleted != null)
                {
                    UserEntity entity = _mapper.Map<UserEntity>(userToBeDeleted);
                    entity.IsActive = false;
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                return false;
            }


        }

        //Este metodo se  utilizara dentro de la vista para asegurar que los usuarios mostrados sean los que realmente existen
        public async Task<IEnumerable<UserVm>> GetCertainProperties()
        {
            bool isActiveFalse = true;
            var result= await _context.User.Where(us=> isActiveFalse ? us.IsActive: true).ToListAsync();
            var userVms = _mapper.Map<IEnumerable<UserVm>>(result);

            return userVms;
        }
    }
}


//TODO: Mas o menos lo que se usara para el LogIn
// https://www.youtube.com/watch?v=uGoNCJf0t1g&ab_channel=CodeStudentNet

//var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, userVm.Email!),
//                new Claim(ClaimTypes.Sid,  userVm.Id.ToString()),
//            };

//ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//AuthenticationProperties properties = new AuthenticationProperties()
//{
//    AllowRefresh = true,
//    IsPersistent = true,
//};

//await HttpContext.SigInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);