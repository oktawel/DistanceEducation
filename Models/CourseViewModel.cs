namespace DistanceEducation.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public List<int> SelectedGroupIds { get; set; }
        public List<Group>? Groups { get; set; }
    }
}
