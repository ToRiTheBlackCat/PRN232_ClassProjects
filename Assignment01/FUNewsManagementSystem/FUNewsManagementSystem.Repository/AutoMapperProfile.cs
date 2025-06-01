using AutoMapper;
using FUNewsManagementSystem.Repository.Models;
using FUNewsManagementSystem.Repository.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Repository
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateAccountForm, SystemAccount>().ForMember(dest => dest.AccountId, opt => opt.Ignore());
        }
    }
}
