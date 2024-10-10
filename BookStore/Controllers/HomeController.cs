using AutoMapper;
using BookStore.Models;
using BookStore.Models.Code;
using BookStore.Models.Data;
using BookStore.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseService<Category> _categoryService;
        //private readonly IBookService _bookService;
        private readonly IBaseService<Cart> _cartService;
        private readonly IBaseService<News> _newsService;
        private readonly IBaseService<BookReview> _reviewService;
        private readonly IUserConfig _userConfig;
        public HomeController(IMapper mapper,
            ILogger<HomeController> logger,
            IBaseService<Category> categoryService,
            IBaseService<Cart> cartService,
            IBaseService<News> newsService,
            IBaseService<BookReview> reviewService,
            IUserConfig userConfig)
           /* IBookService bookService)*/
        {
            _logger = logger;
            _categoryService = categoryService;
            /*_bookService = bookService;*/
            _cartService = cartService;
            _mapper = mapper;
            _userConfig = userConfig;
            _newsService = newsService;
            _reviewService = reviewService;
        }
        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
*/
        public async Task <IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
