using AutoMapper;
using BookStore.Models.Code;
using BookStore.Models.Data;
using BookStore.Models.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        /*private readonly IWebHostEnvironment _hostingEnvironment;
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
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IUserConfig _userConfig;
        private readonly IMapper _mapper;*/
        public IActionResult Index()
        {
            return View();
        }
    }
}
