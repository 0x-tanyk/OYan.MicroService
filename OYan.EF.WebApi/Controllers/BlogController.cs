﻿using Microsoft.AspNetCore.Mvc;
using OYan.EF.MySQL.Common;
using OYan.EF.MySQL.Models;

namespace OYan.EF.WebApi.Controllers
{
    /// <summary>
    /// 博客
    /// </summary>
    [Route("api/blog")]
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
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list")]
        public IActionResult List()
        {
            return Ok(dbContext.Blogs.ToList());
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="blog">博客信息</param>
        /// <returns>操作结果</returns>
        [HttpPost, Route("create")]
        public IActionResult Create(Blog blog)
        {
            dbContext.Blogs.Attach(blog);
            return Ok(dbContext.SaveChanges());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="blogId">博客Id</param>
        /// <returns>操作结果</returns>
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
