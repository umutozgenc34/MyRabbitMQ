using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps:**************************************@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
//channel.QueueDeclare("first-queue", true, false, false);
channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $" log {x}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish("logs-fanout", "", null, messageBody);

    Console.WriteLine($"Mesaj gönderildi. {message}");
});


Console.ReadLine();