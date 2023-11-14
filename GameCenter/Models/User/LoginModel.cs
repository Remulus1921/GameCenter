using System.ComponentModel.DataAnnotations;

namespace GameCenter.Models.User
{

    public class LoginModel
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }
    }
}
