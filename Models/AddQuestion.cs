using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class AddQuestion
    {
        public int? Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Текст вопроса")]
        public string? Name { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        public int TestId { get; set; }
        [Required]
        [Display(Name = "Баллы")]
        public int Cost { get; set; }
        
        public List<AddOption>? Options { get; set; }
        public List<AddOptionTrueFalse>? OptionsTrueFalse { get; set; }
    }
}
