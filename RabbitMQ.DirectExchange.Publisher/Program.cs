using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://auoikmme:f5lpInfFbr3y9XJgyKVIEoNOZXhQPeDQ@fish.rmq.cloudamqp.com/auoikmme");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Message : ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "direct-exchange-example", routingKey: "direct-queue-example", body: byteMessage);
}