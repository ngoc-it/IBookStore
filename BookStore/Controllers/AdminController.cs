using AutoMapper;
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
        private readonly IBaseService<BookReview> _reviewService;
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;
        //private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IUserConfig _userConfig;
        private readonly IMapper _mapper;
        public AdminController(IBaseService<Category> categoryService,
            IBaseService<BookReview> reviewService,
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
            _reviewService = reviewService;
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

        #endregion
        #region VOUCHER
        #endregion






    }
}
