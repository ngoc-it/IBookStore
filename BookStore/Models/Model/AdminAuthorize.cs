using BookStore.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BookStore.Models.Model
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AdminAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check _userConfig
            var userConfigStr = context.HttpContext.User.FindFirst(ClaimTypes.UserData)?.Value;
            if (!string.IsNullOrEmpty(userConfigStr))
            {
                var userConfig = JsonConvert.DeserializeObject<User>(userConfigStr);
                if (userConfig != null)
                {
                    // Nếu có rồi thì check xem có phải admin không thì mới cho zô
                    if (userConfig.RoleType == Constant.RoleEnum.Admin)
                    {
                        return;
                    }
                    else
                    {
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
