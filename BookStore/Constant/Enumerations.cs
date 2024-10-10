namespace BookStore.Constant
{
    public class Enumerations
    {
        // Định nghĩa các loại thông báo hiển thị cho người dùng
        public enum ToastType : int
        {
            None = 0,      // Không có thông báo
            Success = 1,   // Thông báo thành công
            Error = 2,     // Thông báo lỗi
            Warning = 3,   // Thông báo cảnh báo
        }
        // Trạng thái của đơn hàng trong quá trình xử lý
        public enum OrderStatus : int
        {
            Waiting = 1,   // Đơn hàng đang chờ xử lý
            Shipping = 2,  // Đơn hàng đang được giao
            Complete = 3,  // Đơn hàng đã hoàn thành
            Cancel = 4,    // Đơn hàng đã bị hủy
        }

        public enum EditMode : int
        {
            Add = 1,       // Thêm dữ liệu mới
            Edit = 2,      // Sửa dữ liệu hiện có
            Delete = 3,    // Xóa dữ liệu
        }

        // Phương thức sắp xếp danh sách sản phẩm
        public enum SortType : int
        {
            New = 1,       // Sắp xếp theo sản phẩm mới nhất
            Sell = 2,      // Sắp xếp theo sản phẩm bán chạy
            Cheap = 3,     // Sắp xếp theo giá thấp
            Expensive = 4, // Sắp xếp theo giá cao
        }
        // Lý do khách hàng hủy đơn hàng
        public enum ReasonCancel : int
        {
            ChangeInfo = 1, // Thay đổi thông tin giao hàng
            NotBuy = 2,     // Đổi ý, không muốn mua nữa
            WrongOrder = 3, // Đặt nhầm sản phẩm
            NotVoucher = 4, // Không áp dụng mã giảm giá
            Other = 5,      // Lý do khác
        }
        // Lý do cửa hàng hủy đơn hàng
        public enum ReasonCancelShop : int
        {
            SoldOut = 1,    // Hết hàng
            NoContact = 2,  // Không liên hệ được khách hàng
            Other = 3,      // Lý do khác
        }
        // Các chế độ xem trên dashboard
        public enum DashboardViewType : int
        {
            Week = 1,       // Xem theo tuần
            Month = 2,      // Xem theo tháng
            Quarter = 3,    // Xem theo quý
        }
        // Định nghĩa giới tính người dùng
        public enum GenderEnum : int
        {
            None = 0,       // Không xác định giới tính
            Male = 1,       // Nam
            Female = 2,     // Nữ
            Other = 3,      // Giới tính khác
        }

        // Các phương thức thanh toán
        public enum PaymentType : int
        {
            Cod = 1,        // Thanh toán khi nhận hàng (Cash on Delivery)
            Online = 2,     // Thanh toán trực tuyến
        }
        // Trạng thái duyệt (phê duyệt) yêu cầu hoặc đơn hàng
        public enum ApproveStatus : int
        {
            None = 0,       // Chưa xử lý
            Approve = 1,    // Đã được duyệt
            Reject = 2,     // Không được duyệt
        }

    }
}
