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
    } }
