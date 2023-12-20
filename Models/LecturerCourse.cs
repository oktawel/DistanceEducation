//using MessagePack;
using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class LecturerCourse
    {
        public int Id { get; set; }
        public string? LecturerId { get; set; }
        public int CourseId { get; set; }

        public virtual Lecturer? Lecturer { get; set; }

        public virtual Course? Course { get; set; }
    }
}
