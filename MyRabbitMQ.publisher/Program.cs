using RabbitMQ.Client;
using SharedLib;
using System.Text;
using System.Text.Json;


// header key value **

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://******************@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
//channel.QueueDeclare("first-queue", true, false, false);
channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();

headers.Add("format", "pdf");
headers.Add("shape2", "a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;
properties.Persistent = true;

var product = new Product { Id = 1, Name = "Kitap", Price = 200, Stock = 10 };

var productJsonString = JsonSerializer.Serialize(product);

channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));

Console.WriteLine("Mesaj gönderildi.");
Console.ReadLine();
