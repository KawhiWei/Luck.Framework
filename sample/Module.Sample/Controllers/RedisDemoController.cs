using Luck.Framework.Infrastructure.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Module.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisDemoController : ControllerBase
    {

        private readonly IRedisList _redislist;

        public RedisDemoController(IRedisList redislist)
        {
            _redislist = redislist;
        }
        [HttpGet]
        public bool GetRedis()
        {
            _redislist.LPush("key_list", new[] { "adsadas", "adasda" });

            var test = _redislist.LPop("key_list");
            return true;
        }
    }
}
