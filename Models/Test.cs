//using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class Test
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int CourseId { get; set; }
        public bool Status { get; set; }
        public virtual Course? Course { get; set; }

    }
}
