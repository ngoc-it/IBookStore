using AutoMapper;
using BookStore.Models.Data;
using BookStore.Models.Model;

namespace BookStore.Models.Code
{
    
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
            CreateMap<User, RegisterModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UserInfomationModel, User>();
            CreateMap<User, UserInfomationModel>();
            }

            
        }
    
}
