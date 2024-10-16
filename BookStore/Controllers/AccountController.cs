using BookStore.Constant;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using BookStore.Models.Model;
using BookStore.Models.Service;
using AutoMapper;
using BookStore.Models.Code;
using BookStore.Models.Data;
using Microsoft.AspNetCore.Authorization;
using static BookStore.Constant.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;


namespace BookStore.Controllers
{
    public class AccountController : Controller //Lớp quản lý tài khoản ứng dụng 
    {
        // Các trường (fields) lưu trữ các đối tượng dịch vụ được đưa vào qua Dependency Injection
        private readonly IMapper _mapper; // Dịch vụ AutoMapper dùng để chuyển đổi dữ liệu giữa các mô hình (models)
        private readonly IAuthService _authService; // Dịch vụ xác thực (Authentication) để quản lý đăng nhập và phân quyền
        private readonly IBaseService<User> _userService; //quản lý người dùng (User)
        private readonly IBaseService<Cart> _cartService; //quản lý giỏ hàng (Cart)
        private readonly IConfiguration _configuration; // Truy cập các cài đặt cấu hình từ appsettings.json
        private readonly IUserConfig _userConfig; // Quản lý các cấu hình cụ thể của người dùng
        /* public IActionResult Index()
         {
             return View();
         }*/
        public AccountController( //thêm dịch vụ
            IMapper mapper,
            IAuthService authService,
            IBaseService<User> userService,
            IBaseService<Cart> cartService,
            IConfiguration configuration,
            IUserConfig userConfig)

        {
            _mapper = mapper; //gán dịch vụ
            _authService = authService;
            _userService = userService;
            _cartService = cartService;
            _configuration = configuration;
            _userConfig = userConfig;
            
        }
            

            

        // Get: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.ToastType = Constants.None;

            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];

                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }

            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel model, string? returnUrl = null)
        {
            // Thực hiện đăng xuất bất kỳ người dùng hiện tại nào trước khi đăng nhập
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Lưu trữ URL người dùng đã truy cập trước khi được chuyển hướng đến trang đăng nhập
            ViewData["ReturnUrl"] = returnUrl;

            // Kiểm tra xem dữ liệu từ form đăng nhập có hợp lệ không
            if (ModelState.IsValid)
            {
                // Thực hiện xác thực người dùng bằng cách gọi dịch vụ xác thực (authentication service)
                var user = await _authService.AuthenticationUser(model);

                // Nếu người dùng tồn tại trong hệ thống
                if (user != null)
                {
                    // Kiểm tra xem tài khoản đã bị xóa hay chưa
                    if (user.IsDelete)
                    {
                        // Nếu tài khoản bị xóa, hiển thị thông báo lỗi cho người dùng
                        ViewBag.ToastMessage = "Tài khoản không khả dụng hoặc đã bị xóa. Vui lòng kiểm tra lại.";
                        ViewBag.ToastType = Constants.Error;

                        // Trả về trang đăng nhập cùng thông tin lỗi
                        return View(model);
                    }
                    // Kiểm tra xem tài khoản có bị khóa không
                    else if (!user.IsActive)
                    {
                        // Nếu tài khoản bị khóa, hiển thị thông báo lỗi và yêu cầu liên hệ quản trị viên
                        ViewBag.ToastMessage = "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ.";
                        ViewBag.ToastType = Constants.Error;

                        // Trả về trang đăng nhập với thông tin lỗi
                        return View(model);
                    }

                    // Khởi tạo danh sách các claim (thông tin về người dùng để lưu trữ trong phiên làm việc)
                    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName), // Tên người dùng
            new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)), // Dữ liệu người dùng dưới dạng JSON
            new Claim(ClaimTypes.Role, user.RoleType == RoleEnum.Admin ? Role.Admin : Role.User), // Quyền của người dùng (Admin hoặc User)
        };

                    // Tạo đối tượng ClaimsIdentity với danh sách các claim
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Cấu hình thuộc tính xác thực (authentication properties)
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true, // Cho phép làm mới phiên đăng nhập
                        IsPersistent = model.RememberMe, // Ghi nhớ người dùng nếu họ chọn "Remember Me"
                    };

                    // Đăng nhập người dùng bằng ClaimsIdentity và các thuộc tính xác thực
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Thêm thông báo thành công sau khi đăng nhập thành công
                    TempData["ToastMessage"] = "Đăng nhập thành công.";
                    TempData["ToastType"] = Constants.Success;

                    // Chuyển hướng người dùng về URL họ yêu cầu ban đầu hoặc về trang chủ
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    // Nếu tài khoản hoặc mật khẩu không chính xác, hiển thị thông báo lỗi
                    ViewBag.ToastMessage = "Tài khoản hoặc mật khẩu không chính xác.";
                    ViewBag.ToastType = Constants.Error;

                    // Trả về trang đăng nhập với thông tin lỗi
                    return View(model);
                }
            }

            // Nếu dữ liệu form không hợp lệ (ModelState không đúng), trả về trang đăng nhập với dữ liệu đã nhập
            return View(model);

        }


        // Get: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userService.Exist(x => x.UserName.ToLower().Trim().Equals(model.UserName.ToLower().Trim()));
                var emailExist = await _userService.Exist(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()));
                if (userExist && emailExist)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(model);
                }
                else if (userExist)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }
                else if (emailExist)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(model);
                }
                else
                {
                    await _authService.InsertUser(model);

                    TempData["ToastMessage"] = "Đăng ký tài khoản thành công.";
                    TempData["ToastType"] = Constants.Success;

                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Infomation()
        {
            ViewBag.ToastType = Constants.None;
            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];

                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }

            var userId = _userConfig.GetUserId();
            /*ViewBag.CartCount = await _cartService.Count(x => x.UserId == userId);*/

            var user = await _userService.GetEntityById(userId);

            var userModel = _mapper.Map<UserInfomationModel>(user);

            // Set vào ViewBag
            ViewBag.GenderList = new SelectList(new List<ItemDropdownModel>()
            {
                new ItemDropdownModel(){ Value = 0, Name = "Chọn giới tính" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Male, Name = "Nam" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Female, Name = "Nữ" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Other, Name = "Khác" },
            }, "Value", "Name");

            return View(userModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Infomation(UserInfomationModel model)
        {
            // Set vào ViewBag
            ViewBag.GenderList = new SelectList(new List<ItemDropdownModel>()
            {
                new ItemDropdownModel(){ Value = 0, Name = "Chọn giới tính" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Male, Name = "Nam" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Female, Name = "Nữ" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Other, Name = "Khác" },
            }, "Value", "Name");

            if (ModelState.IsValid)
            {
                var userExist = await _userService.Exist(x => x.UserName.ToLower().Trim().Equals(model.UserName.ToLower().Trim()) && model.Id != x.Id);
                var emailExist = await _userService.Exist(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()) && model.Id != x.Id);
                if (userExist && emailExist)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(model);
                }
                else if (userExist)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }
                else if (emailExist)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(model);
                }
                else
                {
                    var user = await _userService.GetEntityById(model.Id);

                    var userModel = _mapper.Map(model, user);

                    await _userService.Update(userModel);

                    TempData["ToastMessage"] = "Cập nhật thông tin tài khoản thành công.";
                    TempData["ToastType"] = Constants.Success;
                    return RedirectToAction("Infomation");
                }
            }

            return View(model);
        }



        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
