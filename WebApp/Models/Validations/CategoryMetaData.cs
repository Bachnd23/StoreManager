using System.ComponentModel.DataAnnotations;

namespace COCOApp.Models.Validations
{
    public class CategoryMetaData
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Độ dài tên sản phẩm không được vượt quá 100 ký tự")]
        [MinLength(3, ErrorMessage = "Phải có ít nhất 3 ký tự")]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "Đơn vị đo lường là bắt buộc")]
        [StringLength(50, ErrorMessage = "Độ dài đơn vị đo lường không được vượt quá 50 ký tự")]
        [MinLength(3, ErrorMessage = "Phải có ít nhất 3 ký tự")]
        public string Description { get; set; } = null!;
    }
}
