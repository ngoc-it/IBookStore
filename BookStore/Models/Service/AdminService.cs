using AutoMapper;
using BookStore.Models.Data;
using BookStore.Models.Model;
using System.Drawing;
using System.Drawing.Imaging;
using static BookStore.Constant.Enumerations;

namespace BookStore.Models.Service
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IBaseService<User> _userService;
        private readonly IBaseService<Book> _bookService;
        private readonly IBaseService<Order> _orderService;
        private readonly IBaseService<Voucher> _voucherService;
        private readonly IBaseService<Category> _categoryService;
        private readonly IBaseService<OrderDetail> _orderDetailService;

        public AdminService(
            IBaseService<User> userService,
            IBaseService<Book> bookService,
            IBaseService<Order> orderService,
            IBaseService<Voucher> voucherService,
            IBaseService<Category> categoryService,
            IBaseService<OrderDetail> orderDetailService,
            IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _bookService = bookService;
            _orderService = orderService;
            _voucherService = voucherService;
            _categoryService = categoryService;
            _orderDetailService = orderDetailService;
        }

        public async Task<DashboardModel> GetDashboardOverview(int? viewType)
        {
            var model = new DashboardModel();

            model.ViewType = viewType ?? (int)DashboardViewType.Week;
            model.TotalBook = await _bookService.Count(x => x.IsActive);
            model.TotalCategory = await _categoryService.Count(x => x.IsActive);
            model.TotalVoucher = await _voucherService.Count(x => x.IsActive);
            model.OrderWaiting = await _orderService.Count(x => x.Status == OrderStatus.Waiting);

            var parseDate = GetStartDateEndDate(model.ViewType);
            model.StartDate = parseDate.Item1;
            model.EndDate = parseDate.Item2;

            model.TotalOrder.Waiting = await _orderService.Count(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Waiting);
            model.TotalOrder.Delivery = await _orderService.Count(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Shipping);
            model.TotalOrder.Complete = await _orderService.Count(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Complete);
            model.TotalOrder.Cancel = await _orderService.Count(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Cancel);

            model.TotalMoney.Waiting = (await _orderService.GetList(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Waiting)).Sum(x => x.TotalMoney -x.ShipCost);
            model.TotalMoney.Delivery = (await _orderService.GetList(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Shipping)).Sum(x => x.TotalMoney- x.ShipCost);
            model.TotalMoney.Complete = (await _orderService.GetList(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Complete)).Sum(x => x.TotalMoney- x.ShipCost);
            model.TotalMoney.Cancel = (await _orderService.GetList(x => x.CreatedDate >= model.StartDate && x.CreatedDate <= model.EndDate && x.Status == OrderStatus.Cancel)).Sum(x => x.TotalMoney- x.ShipCost);

            var totalOrder = _orderDetailService.GetDbSet();

            var groupOrder = totalOrder.GroupBy(x => new { x.BookId, x.BookName }).Select(x => new BookBestSeller
            {
                BookId = x.Key.BookId,
                BookName = x.Key.BookName,
                TotalSold = x.Sum(t => t.Quantity)
            });

            model.BestSeller = groupOrder.OrderByDescending(x => x.TotalSold).ThenBy(x => x.BookName).Skip(0).Take(10).ToList();

            return model;
        }
        public object UploadImage(UploadModel upload)
        {
            // Tạo tên hình ảnh ngẫu nhiên sử dụng GUID
            var imgName = Guid.NewGuid().ToString();

            // Chuyển đổi chuỗi Base64 thành đối tượng Image
            using (Image image = Base64ToImage(upload.BookImageUri))
            {
                // Đường dẫn tương đối đến thư mục uploads trong wwwroot
                string bookDetailImageUri = "\\wwwroot\\uploads\\" + imgName + ".jpg";

                // Đường dẫn tuyệt đối đến tệp sẽ được lưu
                string strFileName = Directory.GetCurrentDirectory() + bookDetailImageUri;

                // Lưu hình ảnh vào hệ thống tệp với định dạng JPEG
                image.Save(strFileName, ImageFormat.Jpeg);

                // Kiểm tra xem tệp đã được lưu thành công hay chưa
                if (System.IO.File.Exists(strFileName))
                {
                    // Trả về đối tượng thành công với tên tệp đã lưu
                    return new { Success = true, FileName = imgName + ".jpg" };
                }
                else
                {
                    // Trả về đối tượng không thành công
                    return new { Success = false };
                }
            }


           
        }
        private Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            Bitmap tempBmp;
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                using (Image image = Image.FromStream(ms, true))
                {
                    //Create another object image for dispose old image handler
                    tempBmp = new Bitmap(image.Width, image.Height);
                    Graphics g = Graphics.FromImage(tempBmp);
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                }
            }
            return tempBmp;
        }
        private Tuple<DateTime, DateTime> GetStartDateEndDate(int viewType)
        {
            switch (viewType)
            {
                case (int)DashboardViewType.Week:
                    DateTime firstDayOfWeek = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek).Date;
                    DateTime lastDayOfWeek = firstDayOfWeek.AddDays(7).AddSeconds(-1);

                    return Tuple.Create(firstDayOfWeek, lastDayOfWeek);
                case (int)DashboardViewType.Month:
                    DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);

                    return Tuple.Create(firstDayOfMonth, lastDayOfMonth);
                case (int)DashboardViewType.Quarter:
                    int quarterNumber = (DateTime.Now.Month - 1) / 3 + 1;
                    DateTime firstDayOfQuarter = new DateTime(DateTime.Now.Year, (quarterNumber - 1) * 3 + 1, 1).Date;
                    DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddSeconds(-1);

                    return Tuple.Create(firstDayOfQuarter, lastDayOfQuarter);
            }

            return Tuple.Create(DateTime.Now, DateTime.Now);
        }
    } }
