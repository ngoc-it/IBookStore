using AutoMapper;
using BookStore.Constant;
using BookStore.Models.Code;
using BookStore.Models.Data;
using BookStore.Models.Model;
using BookStore.Models.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserConfig _userConfig;
        private readonly IBaseService<Book> _bookService;
        private readonly IBaseService<Category> _cateService;
        private readonly IBaseService<Cart> _cartService;

        public CartController(IUserConfig userConfig,
            IBaseService<Category> cateService,
            IBaseService<Book> bookService,
            IBaseService<Cart> cartService,
            IMapper mapper)
        {
            _mapper = mapper;
            _userConfig = userConfig;
            _bookService = bookService;
            _cateService = cateService;
            _cartService = cartService;
        }
       
        public async   Task<IActionResult> Index()
        {
            var userId = _userConfig.GetUserId();

            ViewBag.ToastType = Constants.None;

            if (TempData["ToastMessage"] != null && TempData["ToastType"] != null)
            {
                ViewBag.ToastMessage = TempData["ToastMessage"];
                ViewBag.ToastType = TempData["ToastType"];

                TempData.Remove("ToastMessage");
                TempData.Remove("ToastType");
            }

            var cartList = await _cartService.GetList(x => x.UserId == userId);

            var model = await GetCartModel(cartList);

            ViewBag.CartCount = cartList.Count;
            ViewBag.ErrorProduct = model.CartItems.Count(x => !string.IsNullOrEmpty(x.ErrorMessage));

            return View(model);
        }
        private async Task<CartModel> GetCartModel(List<Cart> cartList)
        {
            var model = new CartModel();

            if (cartList != null && cartList.Any())
            {
                var books = await _bookService.GetList(x => cartList.Select(x => x.BookId).Contains(x.Id));

                var joinBook = from c in cartList
                               join b in books on c.BookId equals b.Id
                               join ca in _cateService.GetDbSet().ToList()
                               on b.CategoryId equals ca.Id
                               select new CartItemModel
                               {
                                   Id = c.Id,
                                   UserId = c.UserId,
                                   BookId = c.BookId,
                                   Quantity = c.Quantity,
                                   BookImage = b.BookImage,
                                   BookName = b.BookName,
                                   MaxQuantity = b.Quantity,
                                   Price = (b.PriceDiscount != null && b.PriceDiscount != 0 ? (int)b.PriceDiscount : b.Price),
                                   PriceOriginal = (b.PriceDiscount != null && b.PriceDiscount != 0 ? b.Price : null),
                                   ErrorMessage = ((b.Quantity <= 0 || !b.IsActive || !ca.IsActive) ? "Sản phẩm đã bán hết hoặc không khả dụng"
                                                    : (c.Quantity > b.Quantity ? "Số lượng sản phẩm vượt quá số lượng có sẵn" : string.Empty))
                               };

                model.CartItems = joinBook.ToList();
            }

            return model;
        }
        [HttpGet]
        public async Task<IActionResult> AddToCart(int bookId, int? quantity)
        {
            var userId = _userConfig.GetUserId();
            var book = await _bookService.GetEntityById(bookId);
            var bookUser = await _cartService.Get(x => x.UserId == userId && x.BookId == bookId);

            if (bookUser != null)
            {
                bookUser.Quantity += ((quantity != null && quantity > 0) ? quantity.Value : 1);
                bookUser.Quantity = bookUser.Quantity > book.Quantity ? book.Quantity : bookUser.Quantity;

                await _cartService.Update(bookUser);
            }
            else
            {
                var newCart = new Cart()
                {
                    BookId = bookId,
                    UserId = userId,
                    Quantity = (quantity != null && quantity > 0) ? quantity.Value : 1
                };

                newCart.Quantity = newCart.Quantity > book.Quantity ? book.Quantity : newCart.Quantity;

                await _cartService.Insert(newCart);
            }

            TempData["ToastMessage"] = "Đã thêm sản phẩm vào giỏ hàng.";
            TempData["ToastType"] = Constants.Success;

            return RedirectToAction("Index");
        }
        [HttpPut]
        public async Task<IActionResult> ChangeQuantity(int id, int quantity)
        {
            var redirectUrl = Url.Action("Index", "Cart");
            var cart = await _cartService.GetEntityById(id);

            cart.Quantity = quantity;
            await _cartService.Update(cart);

            return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveBook(int id)
        {
            var redirectUrl = Url.Action("Index", "Cart");

            await _cartService.Delete(id);

            return Json(new { redirectToUrl = redirectUrl, status = Constants.Success });
        }

        [HttpPost]
        public async Task<IActionResult> Index(string voucherCode, int deliveryId)
        {
            var userId = _userConfig.GetUserId();
            var cartList = await _cartService.GetList(x => x.UserId == userId);
            ViewBag.CartCount = cartList?.Count ?? 0;

            var model = await GetCartModel(cartList);
            ViewBag.ErrorProduct = model.CartItems.Count(x => !string.IsNullOrEmpty(x.ErrorMessage));

            return View(model);
        }
    }
}
