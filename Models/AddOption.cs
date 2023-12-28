using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class AddOption
    {
        public int? Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Текст ответа")]
        public string? Text { get; set; }
        public int QuestionId { get; set; }

        [Required]
        [Display(Name = "Тип ответа")]
        public bool Correct { get; set; }

    }
}
