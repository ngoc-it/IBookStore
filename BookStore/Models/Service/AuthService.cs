using AutoMapper;
using BookStore.Models.Data;
using BookStore.Models.Model;

namespace BookStore.Models.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IBaseService<User> _userService;

        public AuthService(IMapper mapper, IBaseService<User> userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        public async Task InsertUser(RegisterModel model)
        {

            var user = _mapper.Map<User>(model);

            user.Password = model.Password;
            user.RoleType = Constant.RoleEnum.User; //gán cho người đăng kí là 0 tức người dùng
            user.IsDelete = false;
            user.IsActive = true;

            await _userService.Insert(user);
        }
        public async Task<User> AuthenticationUser(UserModel model)
        {
            
            var user = await _userService.GetList(x => x.UserName.ToLower().Trim().Equals(model.UserName.ToLower().Trim()) || x.Email.ToLower().Trim().Equals(model.UserName.ToLower().Trim()));

            if (user != null && user.Any())
            {
                for (var i = 0; i < user.Count; i++)
                {
                    var thisUser = user.ElementAt(i);
                    if (model.Password == thisUser.Password)  
                    {
                        return thisUser;
                    }
                }
            }

            return null;
        }


    }
}
