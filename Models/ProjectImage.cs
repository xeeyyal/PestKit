namespace PestKitAB104.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public bool? IsPrimary { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}
