using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sync_client
{
    class Program
    {
        static string address = "127.0.0.1"; 
        static int port = 8080; 

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                UdpClient client = new UdpClient();

                string message = "";

                while (message.ToLower() != "end")
                {
                    Console.Write("Введіть  повідомлення: ");
                    message = Console.ReadLine();

                    if (!string.IsNullOrEmpty(message))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        client.Send(data, data.Length, ipPoint);

                        IPEndPoint remoteIpPoint = new IPEndPoint(IPAddress.Any, 0);
                        data = client.Receive(ref remoteIpPoint);
                        string response = Encoding.UTF8.GetString(data);

                        Console.WriteLine("Відповідь  сервера: " + response);
                    }
                    else
                    {
                        Console.WriteLine("Будь ласка,  введіть непусте повідомлення.");
                    }
                }

                client.Close();
            }
            catch (SocketException se)
            {
                Console.WriteLine($"Socket exception: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General exception: {ex.Message}");
            }
        }
    }
}
