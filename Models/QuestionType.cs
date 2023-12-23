//using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class QuestionType
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

    }
}
