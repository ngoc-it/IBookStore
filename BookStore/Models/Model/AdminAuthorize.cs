using BookStore.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BookStore.Models.Model
{   // Định nghĩa thuộc tính AdminAuthorizeAttribute cho phép kiểm tra quyền truy cập admin
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        // Phương thức này sẽ được gọi khi một yêu cầu đến controller hoặc action được đánh dấu
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check _userConfig
            // Lấy thông tin cấu hình người dùng từ claims
            var userConfigStr = context.HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;

            if (!string.IsNullOrEmpty(userConfigStr))
            {// Kiểm tra xem thông tin người dùng có tồn tại không
                var userConfig = JsonConvert.DeserializeObject<User>(userConfigStr);  // Chuyển đổi thông tin người dùng từ JSON thành đối tượng User
                if (userConfig != null)
                {
                    // Nếu có rồi thì check xem có phải admin không thì mới cho zô
                    if (userConfig.RoleType == Constant.RoleEnum.Admin)
                    {
                        return;
                    }
                    else
                    {
                        // Nếu người dùng không phải là admin, chuyển hướng đến trang Access Denied
                        context.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new
                            {
                                controller = "Account",
                                action = "AccessDenied",
                            })
                        );
                    }
                }
                else
                {
                    //Nếu chưa có thì đăng nhập
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new
                        {
                            controller = "Account",
                            action = "Login",
                        })
                    );
                }
            }
            else
            {
                //Nếu chưa có thì đăng nhập
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "Login",
                    })
                );
            }
        }
    }
}
