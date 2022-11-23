using Microsoft.AspNetCore.Mvc;
using OYan.EF.MySQL.Common;
using OYan.EF.MySQL.Models;

namespace OYan.EF.WebApi.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [Route("api/post")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PostController : Controller
    {/// <summary>
     /// 
     /// </summary>
        public readonly OYanDbContext dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_context"></param>
        public PostController(OYanDbContext _context)
        {
            dbContext = _context;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public IActionResult List()
        {
            return Ok(dbContext.Posts.ToList());
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="post">文章信息</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            dbContext.Posts.Attach(post);
            return Ok(dbContext.SaveChanges());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="postId">文章Id</param>
        /// <returns>操作结果</returns>
        [HttpDelete, Route("delete")]
        public IActionResult Delete(int postId)
        {
            var post = dbContext.Posts.SingleOrDefault(b => b.PostId == postId);
            if (post == null)
            {
                return Ok("该列表不存在！");
            }
            else
            {
                dbContext.Posts.Remove(post);
                return Ok(dbContext.SaveChanges());
            }

        }
    }
}
