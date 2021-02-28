using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService redis;

        private readonly IDatabase db;

        private string hashKey = "setnames";
        public SetTypeController(RedisService redisService)
        {
            redis = redisService;
            db = redis.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(hashKey))
            {
                db.SetMembers(hashKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            db.KeyExpire(hashKey, DateTime.Now.AddMinutes(5));//sliding expression özelliği olarak düşünülebilir.
            db.SetAdd(hashKey, name);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(hashKey, name);

            return RedirectToAction("Index");
        }
    }
}
