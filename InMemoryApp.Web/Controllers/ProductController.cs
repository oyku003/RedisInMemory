using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            //1.yol
            //if (String.IsNullOrEmpty(memoryCache.Get<string>("zaman")))
            //{
            //    memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            //2.yol burada eger cachede varsa  true dönüp zamanCache'a atar
            if (!memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions//zaman isimli key oluşursa cache tarafında ömrü 30 saniye olacak.
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                    //SlidingExpiration = TimeSpan.FromSeconds(10),
                    Priority = CacheItemPriority.High
                };

                //bir cache silindiğinde hangi degr ne sebeple silindi onu tutacagız
                //options.RegisterPostEvictionCallback((key,value,reason,state)=> {
                //    memoryCache.Set("callback", $"{key}->{value} -> sebep:{reason}");
                //});
                memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            }

            var p = new Product { Id = 1, Name = "Kalem", Price = 200 };
            memoryCache.Set<Product>("product:1", p);
            return View();
        }

        public IActionResult Show()
        {
            //cache almaya calışıp alamazsa cachei create edip döner func içinde ekstra bişeyler de yapılabilir.
            //memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});


            //memoryCache.Remove("zaman");//cachedeki veriyi siler

            //ViewBag.zaman= memoryCache.Get<string>("zaman");

            memoryCache.TryGetValue<string>("zaman", out string zamanCache);
            memoryCache.TryGetValue("callback", out string callback);
           // ViewBag.zaman = zamanCache;
            ViewBag.callback = callback;
            ViewBag.product = memoryCache.Get<Product>("product:1");
            return View();
        }
    }
}
