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
            return Ok($"This Message From {_config["ip"]}:{_config["port"]},ServiceName:Store!");
        }
    }
}
