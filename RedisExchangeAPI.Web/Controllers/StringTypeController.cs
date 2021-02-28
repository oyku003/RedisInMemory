using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService redis;

        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            redis = redisService;
            db = redis.GetDb(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name3", "oyku bilen");
            db.StringSet("ziyaretci2", 100);
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name3");
            value = db.StringGetRange("name3", 0, 3);
            value = db.StringLength("name3");
            db.StringIncrement("ziyaretci", 1);
            db.StringDecrementAsync("ziyaretci", 1).Wait();
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }

            return View();
        }

    }
}
