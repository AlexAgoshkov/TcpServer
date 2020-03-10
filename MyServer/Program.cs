using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyServer
{
    class Program
    {
        //http://127.0.0.1:23451
        static async Task Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            
            TcpListener  listener = new TcpListener(address, 23451);
            listener.Start();
            var bytes = new byte[5120];
            
            while (true)
            {
                try
                {
                    string message = "Hello World!";

                    Console.WriteLine("Waiting for Connection");
                    var client = listener.AcceptTcpClient();

                    NetworkStream stream = client.GetStream();
                    
                    Console.WriteLine($"Accepted connection from {client.Client.RemoteEndPoint}");
                    var bytesReadCount = await stream.ReadAsync(bytes, 0, bytes.Length);
                    var data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesReadCount);
                    
                    string res = $"HTTP/1.1 200 OK\r\n" +
                                 $"Server: {address}\r\n" +
                                 $"Content-Type: text/plain\r\n" +
                                 $"Content-Length: {message.Length}\r\n" +
                                 $"Connection: keep-alive\r\n\r\n" + message;

                    var response = System.Text.Encoding.ASCII.GetBytes(res);
                    await stream.WriteAsync(response, 0, response.Length);
                    
                    Console.WriteLine(data);
                    await stream.FlushAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
