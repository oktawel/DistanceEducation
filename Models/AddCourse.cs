using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class AddCourse
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Название курса")]
        public string? Name { get; set; }

        [Display(Name = "Описание курса")]
        public string? Text { get; set; }

    }
}
