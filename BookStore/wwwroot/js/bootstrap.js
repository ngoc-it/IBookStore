// Nhập toàn bộ thư viện Bootstrap
import * as bootstrap from 'bootstrap'

// Mở rộng chức năng của Tooltip để thêm tùy chọn màu
bootstrap.Tooltip.prototype.show = (original => {
    return function addTooltipColor() {
        // Kiểm tra xem tooltip có được kích hoạt hay không
        if (this._config.toggle === 'tooltip') {
            // Kiểm tra xem phần tử có thuộc tính 'data-color' hay không
            if (this._element.getAttribute('data-color')) {
                // Lấy giá trị của thuộc tính 'data-color' và tạo lớp CSS tương ứng
                const str = `tooltip-${this._element.getAttribute('data-color')}`
                // Thêm lớp màu vào phần tử tooltip
                this.getTipElement().classList.add(str)
            }
        }
        // Gọi hàm gốc để hiển thị tooltip
        original.apply(this)
    }
})(bootstrap.Tooltip.prototype.show)

// Mở rộng chức năng của Popover để thêm tùy chọn màu
bootstrap.Popover.prototype.show = (original => {
    return function addPopoverColor() {
        // Kiểm tra xem popover có được kích hoạt hay không
        if (this._config.toggle === 'popover') {
            // Kiểm tra xem phần tử có thuộc tính 'data-color' hay không
            if (this._element.getAttribute('data-color')) {
                // Lấy giá trị của thuộc tính 'data-color' và tạo lớp CSS tương ứng
                const str = `popover-${this._element.getAttribute('data-color')}`
                // Thêm lớp màu vào phần tử popover
                this.getTipElement().classList.add(str)
            }
        }
        // Gọi hàm gốc để hiển thị popover
        original.apply(this)
    }
})(bootstrap.Popover.prototype.show)

// Xuất đối tượng bootstrap để sử dụng ở nơi khác trong mã
export { bootstrap }
