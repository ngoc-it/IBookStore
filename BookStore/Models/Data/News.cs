using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Data
{
    public class News : BaseData
    {
        [Required(ErrorMessage = "Vui lòng chọn ảnh bìa tin tức")]
        public string NewsImage { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(255, ErrorMessage = "Tiêu đề chứa tối đa 255 ký tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tóm tắt không được để trống")]
        [MaxLength(255, ErrorMessage = "Tóm tắt chứa tối đa 255 ký tự")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; }

    }
}
