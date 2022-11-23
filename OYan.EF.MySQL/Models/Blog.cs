using System.ComponentModel.DataAnnotations;

namespace OYan.EF.MySQL.Models
{
    /// <summary>
    /// 博客
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// 博客Id
        /// </summary>
        [Required]
        public int BlogId { get; set; }

        /// <summary>
        /// 博客Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文章集合
        /// </summary>
        public virtual List<Post> Posts { get; set; }
    }
}
