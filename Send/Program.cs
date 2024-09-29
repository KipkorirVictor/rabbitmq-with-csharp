using System;
using System.Text;
using System.Timers; // Import the Timer namespace
using RabbitMQ.Client;

class Program
{
    private static IModel? _channel;
    private static System.Timers.Timer? _timer;
    private static string _queueName = "hello";

    static void Main()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Create a timer with a 10-second interval
        _timer = new System.Timers.Timer(10000); // 10 seconds = 10000 milliseconds
        _timer.Elapsed += SendMessage; // Hook up the Elapsed event for the timer.
        _timer.AutoReset = true; // Repeat the event
        _timer.Enabled = true; // Enable the timer

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();

        // Cleanup
        _timer.Stop();
        _timer.Dispose();
        _channel.Close();
        connection.Close();
    }

    private static void SendMessage(Object? source, ElapsedEventArgs e)
    {
        const string message = "Victor and Nathan live in Lembus Apartments";
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: string.Empty, routingKey: _queueName, basicProperties: null, body: body);

        Console.WriteLine($"[X] Sent {message} at {DateTime.Now}");
    }
}


// using System.Text;
// using RabbitMQ.Client;

// var factory = new ConnectionFactory {HostName = "localhost"};
// using var connection  = factory.CreateConnection();
// using var channel = connection.CreateModel();


// channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments:null);

// const string message = "Hello World!";
// var body = Encoding.UTF8.GetBytes(message);

// channel.BasicPublish(exchange: string.Empty, routingKey: "hello", basicProperties: null, body: body);

// Console.WriteLine($"[X] Sent {message}");

// Console.WriteLine("Press [enter] to exit");
// Console.ReadLine();