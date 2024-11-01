using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace np_sync_sockets
{
    class Program
    {
        static string address = "127.0.0.1";
        static int port = 8080;

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            UdpClient listener = new UdpClient(ipPoint);

            Dictionary<string, string> responses = new Dictionary<string, string>
            {
                { "Hello", "Привіт! Як я можу допомогти?" },
                { "How are you doing?", "У мене все чудово! Дякую, що запитали." },
                { "What can you do?", "Я можу відповідати на прості запитання!" },
                { "Goodbye", "До побачення! Гарного дня!" }
            };

            Console.WriteLine("Сервер запущено! Очікування підключення...");

            try
            {
                while (true)
                {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = listener.Receive(ref remoteEndPoint);
                    string msg = Encoding.UTF8.GetString(data);

                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: отримано '{msg}' від {remoteEndPoint}");

                    string response = responses.ContainsKey(msg) ? responses[msg] : "Вибачте, я не знаю відповіді на це запитання.";
                    data = Encoding.UTF8.GetBytes(response);

                    listener.Send(data, data.Length, remoteEndPoint);
                    Console.WriteLine("Відповідь надіслана: " + response);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"Socket exception: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server exception: {ex.Message}");
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
