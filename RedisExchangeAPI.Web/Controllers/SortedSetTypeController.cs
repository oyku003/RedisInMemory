using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService redis;

        private readonly IDatabase db;

        private string sortedset = "sortedsetnames";
        public SortedSetTypeController(RedisService redisService)
        {
            redis = redisService;
            db = redis.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if (db.KeyExists(sortedset))
            {
                //db.SortedSetScan(sortedset).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});

                db.SortedSetRangeByRank(sortedset, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, int score)
        {       
            db.SortedSetAdd(sortedset, name, score);
            db.KeyExpire(sortedset, DateTime.Now.AddMinutes(1));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(sortedset, name);

            return RedirectToAction("Index");
        }
    }
}
