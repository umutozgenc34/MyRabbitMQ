using RabbitMQ.Client;
using System.Text;


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://lifgdraw:v8CrUeSM4NT8z2eQlXEuhcdPdARo2EXh@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
//channel.QueueDeclare("first-queue", true, false, false);
channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);
    channel.QueueBind(queueName, "logs-direct",routeKey, null);
});

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1,5);

    string message = $" log-type {log}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";

    channel.BasicPublish("logs-direct", routeKey, null, messageBody);

    Console.WriteLine($"Mesaj gönderildi. {message}");
});


Console.ReadLine();
