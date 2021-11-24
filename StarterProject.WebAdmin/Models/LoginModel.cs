using System.ComponentModel.DataAnnotations;

namespace StarterProject.WebAdmin.Models
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool KeepConnected { get; set; }

        public string ReturnUrl { get; set; }
    }
}