using System;
using System.IO;

namespace DeliveryService
{
    public class Logger
    {
        public static string filePath { get; set; }

        public static void Alert(string message)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Путь к файлу с логами не может быть пустым!", nameof(filePath));
            }

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}]: {message}");

            }
        }
    }
}
