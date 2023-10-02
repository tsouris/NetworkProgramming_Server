using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NetworkProgramming_Server
{
    internal class Program
    {
        static void Main()
        {
            UdpClient udpServer = new UdpClient(8888);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 8888);

            Console.WriteLine("Server is running...");

            while (true)
            {
                try
                {
                    byte[] requestData = udpServer.Receive(ref remoteEndPoint);
                    string componentName = Encoding.UTF8.GetString(requestData);

                    string price = GetComponentPrice(componentName);

                    byte[] responseData = Encoding.UTF8.GetBytes(price);
                    udpServer.Send(responseData, responseData.Length, remoteEndPoint);

                    Console.WriteLine($"Request received for {componentName}. Sent response: {price}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static string GetComponentPrice(string componentName)
        {
            Random random = new Random();
            int price = random.Next(100, 1000);

            return $"${price}";
        }
    }
}