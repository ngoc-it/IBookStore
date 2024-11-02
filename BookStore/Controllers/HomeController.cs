using AutoMapper;
using BookStore.Models;
using BookStore.Models.Code;
using BookStore.Models.Data;
using BookStore.Models.Model;
using BookStore.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList;
using static BookStore.Constant.Enumerations;

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
        private readonly IBookService _bookService;
        public HomeController(IMapper mapper,
            ILogger<HomeController> logger,
            IBaseService<Category> categoryService,
            IBaseService<Cart> cartService,
            IBaseService<News> newsService,
            IBaseService<BookReview> reviewService,
            IUserConfig userConfig,
            IBookService bookService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _bookService = bookService;
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
            var books = _bookService.GetBookActiveInCategoryActive(x => x.Quantity > 0);
            // Sách bán chạy
            ViewBag.BookSelling = books.OrderByDescending(x => x.SoldQuantity).Skip(0).Take(8).ToList();
            // Sách mới nhất
            ViewBag.BookNews = books.OrderByDescending(x => x.CreatedDate).Skip(0).Take(8).ToList();
            // Set vào ViewBag
            ViewBag.CategoryList = await _categoryService.GetList(x => x.IsActive);
            var userId = _userConfig.GetUserId();
            ViewBag.CartCount = await _cartService.Count(x => x.UserId == userId);

            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Book = await _bookService.GetEntityById(id);

            var relatedBooks = new List<Book>();

            // lấy 10 sách cùng danh mục
            var bookCategory = _bookService.GetBookActiveInCategoryActive(x => x.Quantity > 0 && x.Id != id).Skip(0).Take(8).ToList();
            relatedBooks.AddRange(bookCategory);

            // Nếu không đủ thì lấy random sách cho đủ 10
            if (relatedBooks.Count < 10)
            {
                var bookRandom = _bookService.GetBookActiveInCategoryActive(x => x.Quantity > 0 && x.Id != id && !relatedBooks.Select(x => x.Id).Contains(x.Id))
                    .Skip(0).Take(10 - relatedBooks.Count)
                    .ToList();
                relatedBooks.AddRange(bookRandom);
            }

            ViewBag.RelatedBooks = relatedBooks;

            var userId = _userConfig.GetUserId();
            ViewBag.CartCount = await _cartService.Count(x => x.UserId == userId);

            var bookReviews = await _reviewService.GetList(x => x.BookId == id && x.Status == ApproveStatus.Approve);
            var yourReview = await _reviewService.Get(x => x.UserId == userId && x.BookId == id);

            ViewBag.BookReviews = bookReviews.OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.YourReview = yourReview;

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

        public async Task<IActionResult> Search(int? pageIndex, string? keyword, int? categoryId, int? sortType)
        {
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId;
            ViewBag.SortType = sortType;
            ViewBag.CategoryList = await _categoryService.GetList(x => x.IsActive);

            var cate = await _categoryService.GetEntityById(categoryId ?? 0);
            ViewBag.CategoryName = cate?.CategoryName ?? string.Empty;

            var books = _bookService.GetBookActiveInCategoryActive(x => (string.IsNullOrEmpty(keyword) ? x.Id > 0 : x.BookName.ToLower().Contains(keyword.ToLower().Trim()))
                && (categoryId != null && categoryId != 0 ? x.CategoryId == categoryId : x.Id > 0));

            var dataPaging = books.OrderBy(x => x.BookName);

            switch (sortType)
            {
                case (int)SortType.New:
                    dataPaging = books.OrderByDescending(x => x.CreatedDate);
                    break;
                case (int)SortType.Sell:
                    dataPaging = books.OrderByDescending(x => x.SoldQuantity);
                    break;
                case (int)SortType.Cheap:
                    dataPaging = books.OrderBy(x => x.PriceDiscount ?? x.Price);
                    break;
                case (int)SortType.Expensive:
                    dataPaging = books.OrderByDescending(x => x.PriceDiscount ?? x.Price);
                    break;
                default:
                    break;
            }

            ViewBag.Paging = new PagingModel<Book>()
            {
                TotalRecord = books.Count(),
                DataPaging = dataPaging.ToPagedList(pageIndex ?? 1, 9)
            };

            var userId = _userConfig.GetUserId();
            ViewBag.CartCount = await _cartService.Count(x => x.UserId == userId);

            return View();
        }
    }
}
