using System.Data;

namespace BookStore.Models.Model
{
    public class DashboardModel
    {
        // Mô hình đại diện cho tổng quan đơn hàng
        public class OrderOverview
        {
            // Số đơn hàng đang chờ xử lý
            public int Waiting { get; set; }

            // Số đơn hàng đang được giao
            public int Delivery { get; set; }

            // Số đơn hàng đã hoàn tất
            public int Complete { get; set; }

            // Số đơn hàng đã bị hủy
            public int Cancel { get; set; }
        }
        /*// Mô hình đại diện cho thông tin sách bán chạy
        public class BookBestSeller
        {
            // ID của sách
            public int BookId { get; set; }

            // Tên của sách
            public string BookName { get; set; }

            // Tổng số lượng sách đã được bán
            public int TotalSold { get; set; }
        }
*/
        //Số sách trong hệ thống
        public int TotalBook { get; set; }
        //Số loại sách
        public int TotalCategory { get; set; }
        //Số voucher có sẵn
        public int TotalVoucher { get; set; }
        //Tổng số đơn hàng chờ xử lý

        public int OrderWaiting { get; set; }

        //Ngày bắt đầu
        public DataSetDateTime StartDate { get; set; }
        //Ngày kết thúc

        public DateTime EndDate { get; set; }

        // Thông tin tổng quan về các đơn hàng (chờ, giao, hoàn tất, hủy)
        public OrderOverview TotalOrder { get; set; } = new OrderOverview();
        //Thông tin tổng quan về danh thu
        public OrderOverview TotalMoney { get; set; } = new OrderOverview();

        /*// Danh sách các sách bán chạy nhất
        public List<BookBestSeller> BestSeller { get; set; } = new List<BookBestSeller>();

        // Loại hiển thị dữ liệu (ví dụ: dạng danh sách hoặc dạng lưới)
        public int ViewType { get; set; }*/
    }
}
