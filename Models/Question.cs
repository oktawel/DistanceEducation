using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int QuestionTypeId { get; set; }
        public int TestId { get; set; }

        public virtual QuestionType? QuestionType { get; set; }
        public virtual Test? Test { get; set; }
    }
}
