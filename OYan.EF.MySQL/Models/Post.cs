using System.ComponentModel.DataAnnotations;

namespace OYan.EF.MySQL.Models
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Post
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        [Required]
        public int PostId { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int BlogId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Blog Blog { get; set; }
    }
}
