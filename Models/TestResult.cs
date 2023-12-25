using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace DistanceEducation.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string? StudentId { get; set; }

        public double? Mark { get; set; }


        public virtual Test? Test { get; set; }
        public virtual Student? Student { get; set; }


    }
}
