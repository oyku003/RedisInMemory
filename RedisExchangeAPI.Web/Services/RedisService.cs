using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string redisHost;
        private readonly string redisPort;
        private ConnectionMultiplexer redis;//redisle haberleşecek ana sınıfımız
        public IDatabase db { get; set; }
        public RedisService(IConfiguration configuration)
        {
            redisHost = configuration["Redis:Host"];
            redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{redisHost}:{redisPort}";

            redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db)
        {
            return redis.GetDatabase(db);
        }
    }
}
