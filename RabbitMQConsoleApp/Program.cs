using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Hello World!{args.Length}");
			if (args.Length < 1)
				Console.WriteLine("パラメータは、sかrだよ。");
			else if (args[0] == "s")
				Send.Run();
			else if (args[0] == "r")
				Receive.Run();
			else
				Console.WriteLine("パラメータは、sかrだよ。");
		}
	}
	class Send
	{
		public static void Run()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				ConsoleKeyInfo ck = new ConsoleKeyInfo();
				while (ck.Key != ConsoleKey.E)
				{
					channel.QueueDeclare(queue: "hello",
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);
					string message = $"Hello World!{ck.Key}";
					var body = Encoding.UTF8.GetBytes(message);
					channel.BasicPublish(exchange: "",
									 routingKey: "hello",
									 basicProperties: null,
									 body: body);
					Console.WriteLine(" [x] Sent {0}", message);
					ck = Console.ReadKey();
				} 
			}

			Console.WriteLine(" Press [enter] to exit.");
		}
	}

	class Receive
	{
		public static void Run()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "hello",
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine(" [x] Received {0}", message);
				};
				channel.BasicConsume(queue: "hello",
									 autoAck: true,
									 consumer: consumer);

				Console.WriteLine(" Press [enter] to exit.");
				Console.ReadLine();
			}
		}
	}
}
