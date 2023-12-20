//using MessagePack;
using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class GroupCourse
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int CourseId { get; set; }

        public virtual Group? Group { get; set; }

        public virtual Course? Course { get; set; }
    }
}
