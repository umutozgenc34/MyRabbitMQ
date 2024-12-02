using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLib;
using System.Text;
using System.Text.Json;


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://********************@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new Dictionary<string, object>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "any");
channel.QueueBind(queueName, "header-exchange", String.Empty, headers);

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor.");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Product product = JsonSerializer.Deserialize<Product>(message);

    Thread.Sleep(1000);

    Console.WriteLine($"Gelen mesaj : {product.Id} - {product.Name} - {product.Price} - {product.Stock}");

    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();
