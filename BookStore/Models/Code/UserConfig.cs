using BookStore.Models.Data;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BookStore.Models.Code
{ // Lớp UserConfig cài đặt interface IUserConfig
    public class UserConfig : IUserConfig
    {
        // Khai báo một biến riêng để truy cập vào thông tin HTTP hiện tại (IHttpContextAccessor)
        private readonly IHttpContextAccessor _httpContextAccessor;
        // Constructor của lớp, sử dụng dependency injection để lấy IHttpContextAccessor
        public UserConfig(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public User GetUserConfig()
        {
            var userConfig = new User(); //khởi tạo đối tượng rỗng
            //Lấy thông tin cấu hình người dùng từ Claim 
            var userConfigStr = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;
            //nếu userConfiStr ko rỗng hoặc null chuyển chuoxi json thành đối tượng user
            if (!string.IsNullOrEmpty(userConfigStr))
            {
                return JsonConvert.DeserializeObject<User>(userConfigStr);
            }
            //nếu không có dữ liệu trả về đối tượng user rỗng
            return userConfig;
        }
        //phương thức GetUserId trả về Id của người dùng
        public int GetUserId()
        {
            var userConfig = new User(); //khởi tạo đối tượng user rỗng
            //lấy thông tin cấu hình người dùng từ Claim 
            var userConfigStr = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;
            //nếu userConfiStr không rỗng hoặc null chuyển chuỗi Json thành đối tượng user
            if (!string.IsNullOrEmpty(userConfigStr))
            {
                userConfig = JsonConvert.DeserializeObject<User>(userConfigStr); // Deserialize từ JSON thành User
            }
            // Trả về Id của người dùng, nếu không có trả về giá trị mặc định là 0
            return userConfig?.Id ?? 0;
        }
    }
}
