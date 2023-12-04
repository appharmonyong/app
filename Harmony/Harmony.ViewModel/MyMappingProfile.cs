using AutoMapper;
using Harmony.Persistence.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Bussiness.ViewModel
{
    public class MyMappingProfile : Profile
    {

        public MyMappingProfile()
        {

            #region UserMapping
            CreateMap<UserEntity, UserVm>();
            CreateMap<UserVm, UserEntity>();

            CreateMap<UserEntity, UserRegisterVm>();
            CreateMap<UserRegisterVm, UserEntity>();
            #endregion

            // Add more mappings as needed
        }
    }
}
