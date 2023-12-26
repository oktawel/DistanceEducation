using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class Option
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public int QuestionId { get; set; }
        public bool Correct { get; set; }

        public virtual Question? Question { get; set; }
    }
}
