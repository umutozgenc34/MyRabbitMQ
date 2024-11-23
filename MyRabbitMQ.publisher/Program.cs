using RabbitMQ.Client;
using System.Text;


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


channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("header message"));

Console.WriteLine("Mesaj gönderildi.");
Console.ReadLine();
