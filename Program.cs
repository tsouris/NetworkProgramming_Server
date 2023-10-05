using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.VisualBasic;

namespace NetworkProgramming_Server
{
    //TASK 5
    //Add a logging mechanism to the server. This mechanism should store information about clients, their requests, connection time, etc.
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

                    // Log the request
                    LogRequest(remoteEndPoint.Address.ToString(), componentName);

                    Console.WriteLine($"Request received for {componentName}. Sent response: {price}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    LogError(ex.Message);
                }
            }
        }

        private static string GetComponentPrice(string componentName)
        {
            Random random = new Random();
            int price = random.Next(100, 1000);

            return $"${price}";
        }

        private static void LogRequest(string clientAddress, string componentName)
        {
            string logMessage = $"{DateTime.Now}: Request from {clientAddress} for {componentName}";
            WriteToLogFile(logMessage, "requests.txt");
        }

        private static void LogError(string errorMessage)
        {
            string logMessage = $"{DateTime.Now}: Error - {errorMessage}";
            WriteToLogFile(logMessage, "errors.txt");
        }

        private static void WriteToLogFile(string message, string logFileName)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}