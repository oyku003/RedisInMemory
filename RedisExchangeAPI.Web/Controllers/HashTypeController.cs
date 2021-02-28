using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashkey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService) :base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            if (db.KeyExists(hashkey))
            {
                db.HashGetAll(hashkey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string val)
        {
            db.HashSet(hashkey, name, val);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.HashDeleteAsync(hashkey, name);

            return RedirectToAction("Index");
        }
    }
}
