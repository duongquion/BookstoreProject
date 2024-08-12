using System.ComponentModel.DataAnnotations;

namespace ProjectBookStores.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
