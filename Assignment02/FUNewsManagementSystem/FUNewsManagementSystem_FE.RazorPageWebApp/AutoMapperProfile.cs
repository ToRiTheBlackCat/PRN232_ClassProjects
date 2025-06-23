using AutoMapper;
using FUNewsManagementSystem_FE.RazorPageWebApp.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem_FE.RazorPageWebApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Account mapping
            CreateMap<CreateAccountForm, SystemAccountView>().ForMember(dest => dest.AccountId, opt => opt.Ignore());
            CreateMap<UpdateAccountForm, SystemAccountView>().ReverseMap();
        }
    }
}
