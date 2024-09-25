using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory {HostName ="localhost"};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [X] Received {message} at {DateTime.Now}");
};

channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);

Console.WriteLine("Press: [Enter] to exit");
Console.ReadLine();