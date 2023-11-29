using Microsoft.Build.Framework;
using PestKitAB104.Models;
using System.ComponentModel.DataAnnotations;

namespace PestKitAB104.Areas.Admin.ViewModels
{
    public class CreateBlogVM
    {
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        [MaxLength(50,ErrorMessage ="uzunlugu 250-de cox olmamalidir")]
        public string Description { get; set; }
        public int ReplyCount { get; set; }
        public int AuthorId { get; set; }
        public List<Author>? Authors { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
