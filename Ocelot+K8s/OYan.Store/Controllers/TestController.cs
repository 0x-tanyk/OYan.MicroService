using Microsoft.AspNetCore.Mvc;

namespace OYan.Store.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/store/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        IConfiguration _config;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="config"></param>
        public TestController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// getValues
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getValues")]
        public IActionResult GetVaules()
        {
            var conn = Request.HttpContext.Connection;
            var ip = conn?.LocalIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";
            var port = conn?.LocalPort.ToString() ?? "";
            return Ok($"This Message From {ip}:{port},ServiceName:Store!");
        }
    }
}
