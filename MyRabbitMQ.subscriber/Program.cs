using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


//fanout exchange kendisine bağlı olan tüm kuyruklara publisher(producer)dan aldığı mesajı filtreleme yapmaksızın iletir.
//consumerlar genellikle* kendi kuyruklarını kendi oluşturur.


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps:*******************************@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();
//channel.QueueDeclare("first-queue", true, false, false);

var randomQueueName = channel.QueueDeclare().QueueName;
channel.QueueBind(randomQueueName, "logs-fanout", "", null);

channel.BasicQos(0, 1, false);

var consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine("Loglar dinleniyor.");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(1000);

    Console.WriteLine($"Gelen mesaj : {message} ");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();
