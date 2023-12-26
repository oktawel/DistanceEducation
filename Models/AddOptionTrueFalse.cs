using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class AddOptionTrueFalse
    {
        public int Id { get; set; }
        public int OptionTrueFalseId { get; set; }
        public int QuestionId { get; set; }

    }
}
