using System.ComponentModel.DataAnnotations;

namespace OnlinePaymentSite.Web.ViewModels.User
{
    public class UpdatePasswordViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(256, ErrorMessage = "Password cannot exceed 256 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
