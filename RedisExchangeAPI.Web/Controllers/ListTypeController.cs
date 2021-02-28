using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService redis;

        private readonly IDatabase db;

        private string listKey="names";
        public ListTypeController(RedisService redisService)
        {
            redis = redisService;
            db = redis.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();

            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey, name).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);

            return RedirectToAction("Index");
        }
    }
}
