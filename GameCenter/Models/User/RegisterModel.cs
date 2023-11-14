using System.ComponentModel.DataAnnotations;

namespace GameCenter.Models.User
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Imie jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Wymagany jest prawidłowy format adresu email")]
        [Required(ErrorMessage = "Email jest wymagany")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }
    }
}