using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [MaxLength(100)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(200)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
