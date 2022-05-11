using Luck.Framework.Infrastructure.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Module.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisDemoController : ControllerBase
    {

        //private readonly IRedisList _redislist;

        //public RedisDemoController(IRedisList redislist)
        //{
        //    _redislist = redislist;
        //}
        //[HttpGet]
        //public bool GetRedis()
        //{
        //    _redislist.LPush("key_ladsaist", "asdasda","adasdas","asdasda","sadadass");

        //    var test = _redislist.LPop("key_list");
        //    return true;
        //}
    }

    public class RedisDemo
    {
        public string Name { get; set; } = default!;
        public string Password { get; set; }=default!;
    }
}
