using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache distributedCache;//basit redis işlemleri için bu interface'i kullanabiliriz.

        public ProductsController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };

            //await distributedCache.SetStringAsync("name2", "oyku", cacheEntryOptions);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };

            //json olarak kaydetme
            string jsonProduct = JsonConvert.SerializeObject(product);
            //await distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            //binary oılarak kaydetme
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            distributedCache.Set("product:1", byteProduct);

        
            return View();
        }

        public IActionResult Show()
        {
            //string name = distributedCache.GetString("name2");

            //json deserilize
            //string jsonProduct = distributedCache.GetString("product:1");
            //Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            //binary deserilize
            Byte[] byteProduct = distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.name = p.Name;

            return View();
        }

        public IActionResult Remove()
        {
            distributedCache.Remove("name2");

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

            Byte[] imageByte = System.IO.File.ReadAllBytes(path);

            distributedCache.Set("resim", imageByte);
            return View();
        }

        public IActionResult ImageUrl()
        {
            Byte[] resimByte = distributedCache.Get("resim");

            return File(resimByte, "image/jpg");
        }
    }
}
