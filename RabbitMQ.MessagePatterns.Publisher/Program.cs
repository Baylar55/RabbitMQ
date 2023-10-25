using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://auoikmme:f5lpInfFbr3y9XJgyKVIEoNOZXhQPeDQ@fish.rmq.cloudamqp.com/auoikmme");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region Point-to-Point (P2P) Pattern

//string queueName = "example-p2p-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//byte[] message = Encoding.UTF8.GetBytes("Hello World!");
//channel.BasicPublish(
//    exchange: string.Empty,
//    routingKey: queueName,
//    body: message);

#endregion

#region Publish/Subscribe (Pub/Sub) Pattern

//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

//for (int i = 0; i < 20; i++)
//{
//    await Task.Delay(200);

//    byte[] message = Encoding.UTF8.GetBytes("Hello World" + i);

//    channel.BasicPublish(
//        exchange: exchangeName,
//        routingKey: string.Empty,
//        body: message);
//}

#endregion

#region Work Queue Pattern

//string queueName = "example-work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for (int i = 0; i < 30; i++)
//{
//    await Task.Delay(200);

//    byte[] message = Encoding.UTF8.GetBytes("Hello World " + i);

//    channel.BasicPublish(
//        exchange: string.Empty,
//        routingKey: queueName,
//        body: message);
//}

#endregion

#region Request/Reply (RPC) Pattern

string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

#region Create and Send Request Message

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Hello World" + i);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}
#endregion

#region Reply Queue Listening
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
};

#endregion

#endregion

Console.Read();