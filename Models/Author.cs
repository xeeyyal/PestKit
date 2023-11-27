using Microsoft.Build.Framework;

namespace PestKitAB104.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
