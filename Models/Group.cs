using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class Group
    {
        public int? Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название группы")]
        public string? Name { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
