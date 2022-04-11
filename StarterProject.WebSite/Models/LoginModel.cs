using System.ComponentModel.DataAnnotations;

namespace StarterProject.WebSite.Models
{
    public class LoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool KeepConnected { get; set; }

        public string ReturnUrl { get; set; }
    }
}