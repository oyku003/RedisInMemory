using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService redis;

        protected readonly IDatabase db;

        public BaseController(RedisService redisService)
        {
            redis = redisService;
            db = redis.GetDb(1);
        }
      
    }
}
