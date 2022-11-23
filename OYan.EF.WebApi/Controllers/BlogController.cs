using Microsoft.AspNetCore.Mvc;
using OYan.EF.MySQL.Common;
using OYan.EF.MySQL.Models;

namespace OYan.EF.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BlogController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly OYanDbContext dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_context"></param>
        public BlogController(OYanDbContext _context)
        {
            dbContext = _context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public IActionResult List()
        {
            return Ok(dbContext.Blogs.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public IActionResult Create(Blog blog)
        {
            dbContext.Blogs.Attach(blog);
            return Ok(dbContext.SaveChanges());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("delete")]
        public IActionResult Delete(int blogId)
        {
            var blog = dbContext.Blogs.SingleOrDefault(b => b.BlogId == blogId);
            if (blog == null)
            {
                return Ok("该博客不存在！");
            }
            else
            {
                dbContext.Blogs.Remove(blog);
                return Ok(dbContext.SaveChanges());
            }
            
        }
    }
}
