using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace ProjectBookStores.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản Email!")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Email không hơp lệ")]
        [RegularExpression(@"^([A-Za-z0-9][^'!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ][a-zA-z0-9-._][^!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ]*\@[a-zA-Z0-9][^!&@\\#*$%^?<> ()+=':;~`.\[\]{}|/,₹€ ]*\.[a-zA-Z]{2,6})$", ErrorMessage = "Tài khoản Email không hợp lệ")]
        public string email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [StringLength(16, ErrorMessage = "Mật khẩu nên từ 8 đến 16 ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu!")]
        [StringLength(16, ErrorMessage = "Mật khẩu nên từ 8 đến 16 ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage ="Mật khẩu vừa nhập không khớp")]
        public string confirmpassword { get; set; }
    }
}
