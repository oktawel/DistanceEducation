using System.ComponentModel.DataAnnotations;

namespace DistanceEducation.Models
{
    public class AddTest
    {

        public int? Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название теста")]
        public string? Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Описание теста")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Статус теста")]
        public bool Status { get; set; }
        [Required]
        [Display(Name = "Продолжительность(мин.)")]
        public int Length { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата начала тестирования")]
        public DateTime DateStart { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Время начала тестирования")]
        public DateTime TimeStart { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата окончания тестирования")]
        public DateTime DateEnd { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Время окончания тестирования")]
        public DateTime TimeEnd { get; set; }
        public int CourseId { get; set; }

    }
}
