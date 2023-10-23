using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://auoikmme:f5lpInfFbr3y9XJgyKVIEoNOZXhQPeDQ@fish.rmq.cloudamqp.com/auoikmme");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout);

string queueName = Console.ReadLine();
channel.QueueDeclare(queueName, exclusive: false);
channel.QueueBind(queueName,"fanout-exchange-example",routingKey: string.Empty);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queueName,autoAck: true, consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();