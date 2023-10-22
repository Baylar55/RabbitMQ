using RabbitMQ.Client;
using System.Text;


ConnectionFactory factory = new();
factory.Uri = new("amqps://auoikmme:f5lpInfFbr3y9XJgyKVIEoNOZXhQPeDQ@fish.rmq.cloudamqp.com/auoikmme");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

byte[] message = Encoding.UTF8.GetBytes("Hello World!");
channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties: properties);

Console.Read();