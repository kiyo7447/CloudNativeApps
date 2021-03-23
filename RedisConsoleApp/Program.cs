using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;
using System;

namespace RedisConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
            var prog = new Program();

            prog.BasicCreation();
		}


        //[TestMethod]
        public void BasicCreation()
        {
            try
            {
                string server = "localhost";
                string port = "6379";
                string cnstring = $"{server}:{port}";

                var redisOptions = new RedisCacheOptions
                {
                    ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                };
                redisOptions.ConfigurationOptions.EndPoints.Add(cnstring);
//                var opts = Options.Create<RedisCacheOptions>(redisOptions);

                IDistributedCache cache = new Microsoft.Extensions.Caching.Redis.RedisCache(redisOptions);
//                    .StackExchangeRedis.RedisCache(opts);
                string expectedStringData = "Hello world";
                cache.Set("key003", System.Text.Encoding.UTF8.GetBytes(expectedStringData));
                var dataFromCache = cache.Get("key00000003");
                var actualCachedStringData = System.Text.Encoding.UTF8.GetString(dataFromCache);
                Console.WriteLine(expectedStringData);
                Console.WriteLine(actualCachedStringData);

                for (var i = 0; i < 10000000;i++)
				{
                    cache.Set($"key{i:00000000}", System.Text.Encoding.UTF8.GetBytes(expectedStringData));

                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
