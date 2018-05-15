using Cactus.Interfaces;
using System;

namespace Cactus
{
    public class Logger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[Info] {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"[Warning] {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[Error] {message}");
        }
    }
}
