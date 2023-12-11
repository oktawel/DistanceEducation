using Microsoft.AspNetCore.Identity;

namespace DistanceEducation.Data.Indentity
{
    public class ApplicationIndentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
