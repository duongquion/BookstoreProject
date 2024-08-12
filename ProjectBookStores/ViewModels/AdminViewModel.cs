using System.ComponentModel.DataAnnotations;

namespace ProjectBookStores.ViewModels
{
    public class AdminViewModel
    {
        public long Id { get; set; }
        [Display(Name ="Họ và tên")]
        public string UserName { get; set; }
        
        public string PassWord { get; set; }

        public string Email { get; set; }

        [Display(Name = "Ngày và giờ tạo")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [Display(Name = "Công việc")]
        public string PermissionName { get; set; }
    }
}
