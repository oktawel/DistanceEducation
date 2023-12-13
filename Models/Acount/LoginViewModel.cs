using System.ComponentModel.DataAnnotations;

namespace DistanceEducation.Models.Acount
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
