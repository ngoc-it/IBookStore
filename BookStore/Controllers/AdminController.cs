﻿using AutoMapper;
using BookStore.Constant;
using BookStore.Models.Code;
using BookStore.Models.Data;
using BookStore.Models.Model;
using BookStore.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using static BookStore.Constant.Enumerations;


namespace BookStore.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IBaseService<Category> _categoryService;
        private readonly IBaseService<Delivery> _deliveryService;
        private readonly IBaseService<Voucher> _voucherService;
        private readonly IBaseService<Order> _orderService;
        private readonly IBaseService<News> _newsService;
        private readonly IBookService _bookService;
        private readonly IBaseService<User> _userService;
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;
        //private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IUserConfig _userConfig;
        private readonly IMapper _mapper;
        public AdminController(IBaseService<Category> categoryService,
            IWebHostEnvironment environment,
            IBaseService<Delivery> deliveryService,
            IBaseService<Voucher> voucherService,
            IBaseService<Order> orderService,
            IBaseService<News> newsService,
            IBookService bookService,
            IBaseService<User> userService,
            IConfiguration configuration,
            IAdminService adminService,
            //ICartService cartService,
            IAuthService authService,
            IUserConfig userConfig,
            IMapper mapper)
        {
            _hostingEnvironment = environment;
            _deliveryService = deliveryService;
            _categoryService = categoryService;
            _voucherService = voucherService;
            _configuration = configuration;
            _adminService = adminService;
            _orderService = orderService;
            _newsService = newsService;
            _bookService = bookService;
            //_cartService = cartService;
            _userService = userService;
            _authService = authService;
            _userConfig = userConfig;
            _mapper = mapper;
        }

        #region DANHMUC
        //GET: /Admin/CategoryManagement
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CategoryManagement(int? pageIndex)
        {
            ViewBag.ToastType = Constants.None;
            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];

                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }

            var categoryAll = _categoryService.GetAll();

            // Join lấy thông tin số lượng sách
            var categories = _mapper.Map<List<CategoryModel>>(categoryAll);

            foreach (var category in categories)
            {
                category.TotalBook = await _bookService.Count(x => x.CategoryId == category.Id);
            }

            var pagingResult = new PagingModel<CategoryModel>()
            {
                TotalRecord = categories.Count(),
               /* DataPaging = categories.OrderByDescending(x => x.CategoryCode).ToPagedList(pageIndex ?? 1, 10),*/
                DataPaging = categories.OrderBy(x => x.CategoryCode).ToPagedList(pageIndex ?? 1, 10),
            };

            return View(pagingResult);
        }

        //Get: /Admin/CategoryById
        [HttpGet]
        public async Task<JsonResult> CategoryById(int id)
        {
            var entity = await _categoryService.GetEntityById(id);

            return Json(entity);
        }

        //Get: /Admin/ExistCategoryCode
        /*[HttpGet]
        public async Task<JsonResult> ExistCategoryCode(int code, int? id)
        {
            *//*var exist = await _categoryService.Exist(x => x.CategoryId.Trim().ToLower().Equals(code.Trim().ToLower())
                                                            && (id != null && id != 0 ? x.Id != id : x.Id > 0));*//*
            var exist = await _categoryService.Exist(x => x.CategoryId == code // Sửa lại so sánh kiểu số nguyên
                                                && (id != null && id != 0 ? x.Id != id : x.Id > 0));

            return Json(exist);
        }
        */
        [HttpGet]
        public async Task<JsonResult> ExistCategoryCode(int code, int? id)
        {
            /*var exist = await _categoryService.Exist(x => x.CategoryId == code &&
                                                         (id != null && id != 0 ? x.Id != id : true));
            return Json(exist);*/
            /*var exist = await _categoryService.Exist(x => x.CategoryId == code && (id == null || x.Id != id));*/
            var exist = await _categoryService.Exist(x => x.CategoryCode == code // Sửa lại so sánh kiểu số nguyên
                                                && (id != null && id != 0 ? x.Id != id : x.Id > 0));
            return Json(exist);
        }


        //Post: /Admin/InsertCategory
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> InsertCategory([FromBody] Category model)
        {
            var redirectUrl = Url.Action("Categorymanagement", "Admin");

            if (ModelState.IsValid)
            {
                await _categoryService.Insert(model);

                TempData["ToastMessage"] = "Thêm mới danh mục thành công.";
                TempData["ToastType"] = Constants.Success;
                return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
            }
            TempData["ToastMessage"] = "Có lỗi xảy ra trong quá trình xử lý.";
            TempData["ToastType"] = Constants.Error;
            return Json(new { redirectToUrl = redirectUrl, status = Constants.Error });

        }



    //Put: /Admin/UpdateCategory
    [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UpdateCategory([FromBody] Category model)
        {
            var redirectUrl = Url.Action("CategoryManagement", "Admin");

            if (ModelState.IsValid)
            {
                await _categoryService.Update(model);

                TempData["ToastMessage"] = "Cập nhật danh mục thành công.";
                TempData["ToastType"] = Constants.Success;
                return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
            }

            TempData["ToastMessage"] = "Có lỗi xảy ra trong quá trình xử lý.";
            TempData["ToastType"] = Constants.Error;
            return Json(new { redirectToUrl = redirectUrl, status = Constants.Error });
        }

        //Delete: /Admin/DeleteCategory
        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var redirectUrl = Url.Action("CategoryManagement", "Admin");

            if (ModelState.IsValid)
            {
                await _categoryService.Delete(id);

                TempData["ToastMessage"] = "Xóa danh mục thành công.";
                TempData["ToastType"] = Constants.Success;
                return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
            }

            TempData["ToastMessage"] = "Có lỗi xảy ra trong quá trình xử lý.";
            TempData["ToastType"] = Constants.Error;
            return Json(new { redirectToUrl = redirectUrl, status = Constants.Error });
        }
        #endregion
        #region SACH
        //GET: /Admin/BookManagement
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> BookManagement(int? pageIndex, string? keyword, int? categoryId)
        {
            ViewBag.ToastType = Constants.None;
            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];

                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }

            var result = new BookPagingModel()
            {
                CategoryId = categoryId,
                Keyword = keyword,
                Paging = new PagingModel<Book>()
            };

            var books = await _bookService.GetList(x => (string.IsNullOrEmpty(keyword) ? x.Id > 0 : x.BookName.ToLower().Contains(keyword.ToLower().Trim()))
                && (categoryId != null && categoryId != 0 ? x.CategoryId == categoryId : x.Id > 0));

            result.Paging = new PagingModel<Book>()
            {
                TotalRecord = books.Count(),
                DataPaging = books.OrderByDescending(x => x.UpdatedDate).ToPagedList(pageIndex ?? 1, 10),
            };

            var cate = _categoryService.GetAll();
            cate.Insert(0, new Category()
            {
                CategoryName = "Chọn danh mục"
            });

            SelectList cateList = new SelectList(cate, "Id", "CategoryName");
            // Set vào ViewBag
            ViewBag.CategoryList = cateList;

            return View(result);
        }

        //GET: /Admin/BookDetail
        [HttpGet]
        public async Task<IActionResult> BookDetail(int? id)
        {
            if (id != null)
            {
                ViewData["HeaderTitle"] = "Sửa thông tin sách";
            }
            else
            {
                ViewData["HeaderTitle"] = "Thêm sách mới";
            }

            var cate = await _categoryService.GetList(x => x.IsActive);
            cate.Insert(0, new Category()
            {
                CategoryName = "Chọn danh mục"
            });

            SelectList cateList = new SelectList(cate, "Id", "CategoryName");
            // Set vào ViewBag
            ViewBag.CategoryList = cateList;

            var book = (await _bookService.GetEntityById(id ?? 0)) ?? new Book();

            return View(book);
        }

        //Post: /Admin/BookDetail
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> BookDetail(Book model)
        {
            var cate = await _categoryService.GetList(x => x.IsActive);
            cate.Insert(0, new Category()
            {
                CategoryName = "Chọn danh mục"
            });

            SelectList cateList = new SelectList(cate, "Id", "CategoryName");
            // Set vào ViewBag
            ViewBag.CategoryList = cateList;

            bool isValid = true;

            if (ModelState.IsValid)
            {
                if (model.CategoryId == 0)
                {
                    ModelState.AddModelError("CategoryId", "Vui lòng chọn danh mục");
                    isValid = false;
                }

                if (model.PriceDiscount > 0 && model.PriceDiscount >= model.Price)
                {
                    ModelState.AddModelError("PriceDiscount", "Giá khuyến mại phải nhỏ hơn giá bán");
                    isValid = false;
                }
                var bookExist = await _bookService.Exist(x => x.BookId == model.BookId && model.Id != x.Id);

                /* var bookExist = await _bookService.Exist(x => x.BookId.ToLower().Trim().Equals(model.BookId.ToLower().Trim()) && model.Id != x.Id);*/

                if (bookExist)
                {
                    ModelState.AddModelError("BookId", "Mã sách đã tồn tại");
                    isValid = false;
                }

                if (!isValid)
                {
                    return View(model);
                }

                if (model.Id != 0)
                {
                    await _bookService.Update(model);

                    TempData["ToastMessage"] = "Cập nhật thông tin sách thành công.";
                    TempData["ToastType"] = Constants.Success;
                    return RedirectToAction("BookManagement");
                }
                else
                {
                    await _bookService.Insert(model);

                    TempData["ToastMessage"] = "Thêm sách mới thành công.";
                    TempData["ToastType"] = Constants.Success;
                    return RedirectToAction("BookManagement");
                }
            }

            return View(model);
        }

        //Delete: /Admin/DeleteBook
        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var redirectUrl = Url.Action("BookManagement", "Admin");

            if (ModelState.IsValid)
            {
                await _bookService.Delete(id);

                TempData["ToastMessage"] = "Xóa sách thành công.";
                TempData["ToastType"] = Constants.Success;
                return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
            }

            TempData["ToastMessage"] = "Có lỗi xảy ra trong quá trình xử lý.";
            TempData["ToastType"] = Constants.Error;
            return Json(new { redirectToUrl = redirectUrl, status = Constants.Error });
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public IActionResult UploadImage([FromBody] UploadModel upload)
        {
            if (!string.IsNullOrEmpty(upload.BookImageUri))
            {
                var result = _adminService.UploadImage(upload);
                return Json(result);
            }
            else
            {
                return Json(new { Success = false });
            }
        }
        #endregion
        #region QUANLYNGUOIDUNG
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UserManagement(int? pageIndex, string? keyword)
        {
            // Đặt giá trị mặc định cho ToastType, sử dụng để hiển thị loại thông báo
            ViewBag.ToastType = Constants.None;

            // Kiểm tra nếu TempData có dữ liệu thông báo (ToastMessage và ToastType)
            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                // Gán thông báo và loại thông báo từ TempData vào ViewBag
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];

                // Xóa thông tin thông báo khỏi TempData sau khi sử dụng
                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }

            // Tạo một đối tượng UserPagingModel để lưu trữ kết quả tìm kiếm và phân trang
            var result = new UserPagingModel()
            {
                Keyword = keyword, // Từ khóa tìm kiếm
                Paging = new PagingModel<UserManagementModel>() // Khởi tạo model phân trang
            };

            // Lấy ID của người dùng hiện tại từ cấu hình
            var currentUserId = _userConfig.GetUserId();

            // Lấy danh sách người dùng, loại trừ người dùng hiện tại và những người dùng đã bị xóa
            // Nếu không có từ khóa tìm kiếm thì lấy tất cả, nếu có thì lọc theo từ khóa
            var users = await _userService.GetList(x => x.Id != currentUserId && !x.IsDelete &&
                (string.IsNullOrEmpty(keyword) ? x.Id > 0 :
                (x.UserName.ToLower().Contains(keyword.ToLower().Trim()) ||
                x.LastName.ToLower().Contains(keyword.ToLower().Trim()) ||
                x.FirstName.ToLower().Contains(keyword.ToLower().Trim()))));

            // Lấy danh sách các đơn hàng liên quan đến những người dùng trong danh sách
/*            var orders = await _orderService.GetList(x => users.Select(x => x.Id).Contains(x.UserId));*/

            // Dùng LINQ để tạo danh sách người dùng bao gồm các thông tin cần thiết
            var userJoin = users.Select(u => new UserManagementModel()
            {
                Id = u.Id, // ID người dùng
                UserName = u.UserName, // Tên đăng nhập
                Password = u.Password,
                FirstName = u.FirstName, // Tên
                LastName = u.LastName, // Họ
                Email = u.Email, // Email
                CreatedDate = u.CreatedDate, // Ngày tạo
                IsActive = u.IsActive, // Trạng thái kích hoạt của người dùng
                RoleType = u.RoleType, // Loại vai trò của người dùng
                RoleName = u.RoleName, // Tên vai trò của người dùng
            });

            // Cập nhật thông tin phân trang và sắp xếp danh sách người dùng theo ngày tạo (mới nhất trước)
            result.Paging = new PagingModel<UserManagementModel>()
            {
                TotalRecord = users.Count(),
                DataPaging = userJoin.OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageIndex ?? 1, 10),
            };


            // Trả về View với dữ liệu kết quả phân trang
            return View(result);
        }

        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var redirectUrl = Url.Action("UserManagement", "Admin");

            if (ModelState.IsValid)
            {
                var user = await _userService.GetEntityById(userId);

                user.IsDelete = true;
                await _userService.Update(user);

                TempData["ToastMessage"] = $"Đã xóa tài khoản {user.UserName} thành công.";
                TempData["ToastType"] = Constants.Success;
                return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
            }

            TempData["ToastMessage"] = "Có lỗi xảy ra trong quá trình xử lý.";
            TempData["ToastType"] = Constants.Error;
            return Json(new { redirectToUrl = redirectUrl, status = Constants.Error });
        }
        /*
                [HttpGet]
                public async Task<IActionResult> UserDetail(int? id)
                {
                    if (id != null)
                    {
                        ViewData["HeaderTitle"] = "Sửa thông tin tài khoản";
                    }
                    else
                    {
                        ViewData["HeaderTitle"] = "Thêm tài khoản";
                    }

                    // Set vào ViewBag cho danh sách giới tính và vai trò
                    ViewBag.GenderList = new SelectList(new List<ItemDropdownModel>()
            {
                new ItemDropdownModel(){ Value = 0, Name = "Chọn giới tính" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Male, Name = "Nam" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Female, Name = "Nữ" },
                new ItemDropdownModel(){ Value = (int)GenderEnum.Other, Name = "Khác" },
            }, "Value", "Name");

                    ViewBag.RoleList = new SelectList(new List<ItemDropdownModel>()
            {
                new ItemDropdownModel(){ Value = (int)RoleEnum.User, Name = "Người dùng" },
            }, "Value", "Name");

                    // Lấy thông tin người dùng nếu có ID
                    var user = await _userService.GetEntityById(id ?? 0);

                    // Nếu không có ID (tức là tạo mới), không cần mật khẩu mặc định
                    if (user == null)
                    {
                        user = new User();
                    }

                    return View(user);
                }
        */
        [HttpGet]
        public async Task<IActionResult> UserDetail(int? id)
        {
            // Kiểm tra nếu id không null, cho phép chỉnh sửa thông tin
            if (id == null)
            {
                return NotFound(); // Nếu không có id, trả về 404 Not Found
            }

            ViewData["HeaderTitle"] = "Sửa thông tin tài khoản";

            // Set vào ViewBag cho danh sách giới tính
            ViewBag.GenderList = new SelectList(new List<ItemDropdownModel>()
    {
        new ItemDropdownModel(){ Value = 0, Name = "Chọn giới tính" },
        new ItemDropdownModel(){ Value = (int)GenderEnum.Male, Name = "Nam" },
        new ItemDropdownModel(){ Value = (int)GenderEnum.Female, Name = "Nữ" },
        new ItemDropdownModel(){ Value = (int)GenderEnum.Other, Name = "Khác" },
    }, "Value", "Name");

            // Set vào ViewBag cho danh sách vai trò
            ViewBag.RoleList = new SelectList(new List<ItemDropdownModel>()
    {
        new ItemDropdownModel(){ Value = (int)RoleEnum.User, Name = "Người dùng" },
    }, "Value", "Name");

            // Lấy thông tin người dùng từ dịch vụ
            var user = await _userService.GetEntityById(id.Value);

            // Kiểm tra xem người dùng có tồn tại không
            if (user == null)
            {
                return NotFound(); // Nếu người dùng không tồn tại, trả về 404 Not Found
            }

            return View(user); // Trả về view với thông tin người dùng
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> LockUser(int userId)
        {
            var user = await _userService.GetEntityById(userId);

            user.IsActive = !user.IsActive;
            await _userService.Update(user);

            TempData["ToastMessage"] = user.IsActive ? $"Đã mở khóa cho tài khoản {user.UserName}." : $"Đã khóa tài khoản {user.UserName}.";
            TempData["ToastType"] = Constants.Success;
            return RedirectToAction("UserManagement");
        }





        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> UserDetail(User model)
        {
            // Set vào ViewBag cho danh sách giới tính và vai trò
            ViewBag.GenderList = new SelectList(new List<ItemDropdownModel>()
    {
        new ItemDropdownModel(){ Value = 0, Name = "Chọn giới tính" },
        new ItemDropdownModel(){ Value = (int)GenderEnum.Male, Name = "Nam" },
        new ItemDropdownModel(){ Value = (int)GenderEnum.Female, Name = "Nữ" },
        new ItemDropdownModel(){ Value = (int)GenderEnum.Other, Name = "Khác" },
    }, "Value", "Name");

            ViewBag.RoleList = new SelectList(new List<ItemDropdownModel>()
    {
        new ItemDropdownModel(){ Value = (int)RoleEnum.User, Name = "Người dùng" },
    }, "Value", "Name");

            var user = await _userService.GetEntityById(model.Id);

            // Nếu không tìm thấy người dùng, trả về lỗi NotFound
            if (user == null)
            {
                return NotFound();
            }

            // Nếu mật khẩu trống, giữ nguyên mật khẩu cũ
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = user.Password; // Giữ nguyên mật khẩu cũ
            }
            // Nếu có mật khẩu mới, sử dụng nó trực tiếp (không hash)
            else
            {
                model.Password = model.Password; // Sử dụng mật khẩu mới
            }

            if (ModelState.IsValid)
            {
                var userExist = await _userService.Exist(x => x.UserName.ToLower().Trim().Equals(model.UserName.ToLower().Trim()) && model.Id != x.Id);
                var emailExist = await _userService.Exist(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()) && model.Id != x.Id);

                // Kiểm tra tên đăng nhập và email
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
                    // Cập nhật thông tin tài khoản
                    await _userService.Update(model);
                    TempData["ToastMessage"] = "Cập nhật thông tin tài khoản thành công.";
                    TempData["ToastType"] = Constants.Success;
                    return RedirectToAction("UserManagement");
                }
            }

            return View(model);
        }






        #endregion
        #region VOUCHER
        #endregion






    }
}
