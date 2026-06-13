using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

const int port = 5000;

using var client = new TcpClient();
try
{
    client.Connect(IPAddress.Loopback, port);
    Console.WriteLine("[КЛІЄНТ] Успішно підключено до сервера!");
}
catch (Exception ex)
{
    Console.WriteLine($"[КЛІЄНТ] Помилка підключення: {ex.Message}");
    return;
}

using var stream = client.GetStream();


var receiveTask = Task.Run(() =>
{
    var buffer = new byte[1024];
    try
    {
        while (true)
        {
            int bytes = stream.Read(buffer, 0, buffer.Length);
            if (bytes == 0) break; 

            var message = Encoding.UTF8.GetString(buffer, 0, bytes);
            Console.WriteLine($"\n[СЕРВЕР]: {message}");
            Console.Write("Ви (Клієнт): ");
        }
    }
    catch
    {
        Console.WriteLine("\n[КЛІЄНТ] Помилка читання або сервер завершив роботу.");
    }
});


while (true)
{
    Console.Write("Ви (Клієнт): ");
    var message = Console.ReadLine();

    if (string.IsNullOrEmpty(message) || message == "exit")
        break;

    try
    {
        var data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }
    catch
    {
        Console.WriteLine("[КЛІЄНТ] Не вдалося відправити повідомлення.");
        break;
    }
}

Console.WriteLine("[КЛІЄНТ] Вихід з чату.");