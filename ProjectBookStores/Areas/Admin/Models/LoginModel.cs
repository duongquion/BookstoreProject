using System.ComponentModel.DataAnnotations;

namespace ProjectBookStores.Areas.Admin.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Vui lòng nhập Email!.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!.")]
        [MinLength(1, ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự.")]
        [MaxLength(18, ErrorMessage = "Mật khẩu nhiều nhất 18 ký tự.")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
