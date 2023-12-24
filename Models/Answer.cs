using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId{ get; set; }
        public int? OptionId { get; set; }
        public int? OptionTFId { get; set; }
        public string? TextAnswer { get; set; }
        public bool? Correct { get; set; }

        public virtual Question? Question { get; set; }
        public virtual Option? Option { get; set; }
        public virtual OptionTrueFalse? OptionTrueFalse { get; set; }


    }
}
