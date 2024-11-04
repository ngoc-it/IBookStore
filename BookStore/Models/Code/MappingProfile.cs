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
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();
            CreateMap<UserInfomationModel, User>();
            CreateMap<User, UserInfomationModel>();
            CreateMap<Cart, CartModel>();
            CreateMap<CartModel, Cart>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();

        }

            
        }
    
}
