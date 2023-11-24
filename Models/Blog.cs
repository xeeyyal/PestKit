namespace PestKitAB104.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReplyCount { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
