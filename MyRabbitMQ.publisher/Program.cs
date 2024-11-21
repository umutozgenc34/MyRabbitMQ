using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://lifgdraw:***************************@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
channel.QueueDeclare("first-queue", true, false, false);

string message = "Hello World";
var messageBody = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(string.Empty, "first-queue",null,messageBody);

Console.WriteLine("Mesaj gönderildi.");
Console.ReadLine();