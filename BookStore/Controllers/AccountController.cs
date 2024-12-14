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
using Microsoft.Extensions.Caching.Memory;


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
        private readonly IUserConfig _userConfig;
        // Quản lý các cấu hình cụ thể của người dùng
        private IMemoryCache _cache;
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
            IUserConfig userConfig,
            IMemoryCache memoryCache)

        {
            _mapper = mapper; //gán dịch vụ
            _authService = authService;
            _userService = userService;
            _cartService = cartService;
            _configuration = configuration;
            _userConfig = userConfig;
            _cache = memoryCache;

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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _authService.AuthenticationUser(model);
                if (user != null)
                {
                    if (user.IsDelete)
                    {
                        ViewBag.ToastMessage = "Tài khoản không khả dụng hoặc đã bị xóa. Vui lòng kiểm tra lại.";
                        ViewBag.ToastType = Constants.Error;

                        return View(model);
                    }
                    else if (!user.IsActive)
                    {
                        ViewBag.ToastMessage = "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên để được hỗ trợ.";
                        ViewBag.ToastType = Constants.Error;

                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)),
                        new Claim(ClaimTypes.Role, user.RoleType == RoleEnum.Admin ? Role.Admin : Role.User),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = model.RememberMe,
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ViewBag.ToastMessage = "Tài khoản hoặc mật khẩu không chính xác.";
                    ViewBag.ToastType = Constants.Error;

                    return View(model);
                }
            }
            return View(model);
        }

        /*        // GET: /Account/Login
                [HttpGet]
                public IActionResult Login(string? returnUrl = null)
                {
                    ViewData["ReturnUrl"] = returnUrl;

                    return View();
                }

        // POST: /Account/Login
        /*[HttpPost]
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
                        // Trả về trang đăng nhập cùng thông tin lỗi
                        ModelState.AddModelError(string.Empty, "Tài khoản không khả dụng hoặc đã bị xóa. Vui lòng kiểm tra lại.");
                        return View(model);
                    }
                    // Kiểm tra xem tài khoản có bị khóa không
                    else if (!user.IsActive)
                    {
                        // Trả về trang đăng nhập với thông tin lỗi
                        ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa.");
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
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

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

                    // Sử dụng TempData để lưu trữ thông báo
                    TempData["SuccessMessage"] = "Đăng nhập thành công.";
                    // Chuyển hướng người dùng về URL họ yêu cầu ban đầu hoặc về trang chủ
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    // Nếu tài khoản hoặc mật khẩu không chính xác, hiển thị thông báo lỗi
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác.");
                    return View(model);
                }
            }

            // Nếu dữ liệu form không hợp lệ (ModelState không đúng), trả về trang đăng nhập với dữ liệu đã nhập
            return View(model);
        }*/








        // Get: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken] //// Xác nhận mã chống giả mạo để bảo vệ khỏi các cuộc tấn công CSRF
        public async Task<ActionResult>Register(RegisterModel model)
        {
            //kiểm tra tính hợp lệ của dữ liệu nhập vào form (Model)
            if (ModelState.IsValid)
            {
                //kiểm tra sự tôn tại của tên đăng nhập trong cơ sở dữ liệu (bỏ qua khoảng trắng và ko phân biệt chữ hoa thường)
                var userExist = await _userService.Exist(x => x.UserName.ToLower().Trim().Equals(model.UserName.ToLower().Trim()));
                //kiểm tra emal trong csdl 
                var emailExist = await _userService.Exist(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()));
                if (userExist && emailExist) //nếu cả tên đăng nhập và email đã tồn tại 
                {
                    //thêm thông báo lỗi vào ModelState cho tên đăng nhập
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    // Thêm thông báo lỗi cho email
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(model); //trả vê view với thông báo lỗi email
                }
                // Nếu chỉ có tên đăng nhập tồn tại
                else if (userExist)
                {// Thêm thông báo lỗi vào ModelState cho tên đăng nhập
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    // Trả về view với thông báo lỗi về tên đăng nhập
                    return View(model);
                }
                // Nếu chỉ có email tồn tại
                else if (emailExist)
                {
                    // Thêm thông báo lỗi vào ModelState cho email
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    // Trả về view với thông báo lỗi về email
                    return View(model);
                }
                else
                {  // Nếu cả tên đăng nhập và email đều chưa tồn tại
                    await _authService.InsertUser(model);
                    // Gọi hàm để chèn người dùng mới vào cơ sở dữ liệu thông qua _authService
                    // Thiết lập thông báo thành công vào TempData để hiển thị ở trang kế tiếp
                    TempData["ToastMessage"] = "Đăng ký tài khoản thành công.";
                    TempData["ToastType"] = Constants.Success;
                    // Chuyển hướng người dùng tới trang đăng nhập sau khi đăng ký thành công
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Infomation()
        {
            // Thiết lập giá trị mặc định cho ToastMessage (thông báo) là không có
            ViewBag.ToastType = Constants.None;
            // Kiểm tra nếu có thông báo Toast (từ TempData)
            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                // Gán thông báo và loại thông báo vào ViewBag để hiển thị trong view
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];
                // Xóa thông báo sau khi đã lấy ra
                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }
            // Lấy ID của người dùng hiện tại
            var userId = _userConfig.GetUserId();
            /*ViewBag.CartCount = await _cartService.Count(x => x.UserId == userId);*/
            // Lấy thông tin người dùng từ cơ sở dữ liệu theo ID
            var user = await _userService.GetEntityById(userId);
            // Ánh xạ dữ liệu người dùng từ entity User sang mô hình UserInfomationModel bằng AutoMapper
            var userModel = _mapper.Map<UserInfomationModel>(user);
            // Thiết lập danh sách lựa chọn giới tính và lưu vào ViewBag để sử dụng trong view

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
        [Authorize] // Yêu cầu người dùng phải đăng nhập
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
                    // Lấy thông tin người dùng hiện tại từ cơ sở dữ liệu
                    var user = await _userService.GetEntityById(model.Id);

                    var userModel = _mapper.Map(model, user);
                    // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                    await _userService.Update(userModel);

                    TempData["ToastMessage"] = "Cập nhật thông tin tài khoản thành công.";
                    TempData["ToastType"] = Constants.Success;
                    // Chuyển hướng về trang "Infomation" sau khi cập nhật thành công
                    return RedirectToAction("Infomation");
                }
            }
            // Nếu dữ liệu không hợp lệ, trả về view với mô hình dữ liệu
            return View(model);
        }

        public async Task<IActionResult> ChangePassword()
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
            ViewBag.CartCount = await _cartService.Count(x => x.UserId == userId);

            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(PasswordModel model)
        {
            var userId = _userConfig.GetUserId();
            // Lấy thông tin người dùng theo ID
            var user = await _userService.GetEntityById(userId);
            // Kiểm tra mật khẩu cũ
            var validPass = model.OldPassword == user.Password; // Kiểm tra mật khẩu không băm

            if (!validPass)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu hiện tại không chính xác");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                user.Password = model.NewPassword; // Lưu mật khẩu mới không băm
                await _userService.Update(user);

                TempData["ToastMessage"] = "Cập nhật mật khẩu thành công.";
                TempData["ToastType"] = Constants.Success;

                return RedirectToAction("ChangePassword");
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
