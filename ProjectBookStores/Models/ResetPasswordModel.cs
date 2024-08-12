using System.ComponentModel.DataAnnotations;

namespace ProjectBookStores.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [StringLength(16, ErrorMessage = "Mật khẩu nên từ 8 đến 16 ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu!")]
        [StringLength(16, ErrorMessage = "Mật khẩu nên từ 8 đến 16 ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Mật khẩu vừa nhập không khớp")]
        public string confirmpassword { get; set; }
    }
}
