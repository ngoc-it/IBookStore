using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookStore.Models.Data
{
    public class Book : BaseData
    {
        [Required(ErrorMessage = "Mã sách không để trống")]
        public int BookId { get; set; } //Mã sách

        [Required(ErrorMessage ="Tên sách không được để trống")]
        [MaxLength(100,ErrorMessage ="Không vượt quá 100 ký tự")]
        public string BookName { get; set; } //Tên sách

        [Required(ErrorMessage = "Vui lòng chọn ảnh bìa sách")]
        public string BookImage { get; set; }

        [AllowNull]
        [MaxLength(100, ErrorMessage = "Tác giả chứa tối đa 100 ký tự")]
        public string? Author { get; set; } //Tác giả

        [AllowNull]
        [MaxLength(200, ErrorMessage = "Nhà xuất bản chứa tối đa 200 ký tự")]
        public string? Publisher { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại sách")]
        public int CategoryId { get; set; } //Mã loại sách
        public string CategoryName { get; set; } //Tên loại sách
        public int Quantity { get; set; } //Số lượng sách
        public int SoldQuantity { get; set; } = 0; //Số lượng đã bán

        [Required(ErrorMessage = "Giá bán không được để trống")]
        public int Price { get; set; } //Giá bán

        [AllowNull]
        public int? PriceDiscount { get; set; } //Khuyến mãi
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true; //Trạng thái

    }
}
