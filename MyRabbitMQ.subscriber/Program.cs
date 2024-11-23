﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://********************@chimpanzee.rmq.cloudamqp.com/lifgdraw");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();


channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

var queueName = channel.QueueDeclare().QueueName;
var routekey = "Info.#";
channel.QueueBind(queueName, "logs-topic", routekey);

channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor.");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(1000);

    Console.WriteLine($"Gelen mesaj : {message} ");

    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadLine();
