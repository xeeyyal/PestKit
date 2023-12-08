using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Models
{
    public class Project
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectImage>? ProjectImages { get; set; }
    }
}
