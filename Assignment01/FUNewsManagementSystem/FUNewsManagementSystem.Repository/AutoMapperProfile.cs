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
            //Create Account mapping
            CreateMap<CreateAccountForm, SystemAccount>().ForMember(dest => dest.AccountId, opt => opt.Ignore());

            //Tag mapping (no circular reference)
            CreateMap<Tag, TagView>().ReverseMap();

            //NewsArticle mapping
            CreateMap<NewsArticle, NewsArticleView>()
                .ForMember(dest => dest.TagsList, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap()
                .ForMember(dest => dest.Tags, opt => opt.Ignore()); //News only
        }
    }
}
