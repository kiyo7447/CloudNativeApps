using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using StackExchange.Redis;
using System;
using System.Text;

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

		//https://medium.com/@saurabh.dasgupta1/about-7fb96fb1f80d
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
					ConfigurationOptions = new ConfigurationOptions()
					
				};
				redisOptions.ConfigurationOptions.EndPoints.Add(cnstring);
				//var opts = Options.Create<RedisCacheOptions>(redisOptions);

				IDistributedCache cache = new RedisCache(redisOptions);
				//.StackExchangeRedis.RedisCache(opts);
				
				//セット
				string expectedStringData = "Hello world";
				cache.Set($"key{3:D8}", Encoding.UTF8.GetBytes(expectedStringData));
				
				//ゲット
				var dataFromCache = cache.Get($"key{3:D8}");
				var actualCachedStringData = Encoding.UTF8.GetString(dataFromCache);
				Console.WriteLine(expectedStringData);
				Console.WriteLine(actualCachedStringData);

				Console.WriteLine(DateTime.Now.ToString());

#if false
				//1000万件の値のセット
				for (var index = 0; index < 10000000; index++)
				{
					cache.Set($"key{index:D8}", Encoding.UTF8.GetBytes(expectedStringData + $"{index:00000000}"));
					if (index % 10000 == 0)
					{
						Console.WriteLine($"index={index}, key{index:D8}"
						);

					}
				}
#endif

				Console.WriteLine(DateTime.Now.ToString());
				var random = new Random();
				for (var index = 0; index < 1000; index++)
				{
					var r = random.Next(0, 1000000);
					var key = $"key{r:D8}";
					var byteValue = cache.Get(key);
					if (byteValue == null) continue;
					var value = Encoding.UTF8.GetString(byteValue);
					Console.WriteLine($"index={index:D3}, key={key}, value={value}");
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
