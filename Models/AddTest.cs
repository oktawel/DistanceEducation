using System.ComponentModel.DataAnnotations;

namespace DistanceEducation.Models
{
    public class AddTest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Length { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime TimeEnd { get; set; }
        public int CourseId { get; set; }

    }
}
