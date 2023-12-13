using System.ComponentModel.DataAnnotations;

namespace DistanceEducation.Models.Acount
{
    public class RegisterStudent
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
