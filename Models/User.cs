//using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DistanceEducation.Models
{
    public class User
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }
        [Required]
        [Display(Name = "UserId")]
        public string? UserId { get; set; }

    }
}
