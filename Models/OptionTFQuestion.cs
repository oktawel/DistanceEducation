using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class OptionTFQuestion
    {
        public int Id { get; set; }
        public int OptionTrueFalseId { get; set; }
        public int QuestionId { get; set; }

        public virtual Question? Question { get; set; }
        public virtual OptionTrueFalse? OptionTrueFalse { get; set; }
        
    }
}
