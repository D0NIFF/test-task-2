using DeliveryService.Controllers;
using System;
using System.Collections.Generic;
using System.IO;

namespace DeliveryService
{
    internal class Program
    {
        public static Dictionary<string, string> LoadConfig()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            var path = AppDomain.CurrentDomain.BaseDirectory + ".env";
            foreach (string line in File.ReadLines(path))
            {
                var values = line.Split('=');
                config.Add(values[0], values[1]);
            }
            return config;
        }

        static void Main(string[] args)
        {
            
            string inputFilePath, outputFilePath, logFilePath, district;
            DateTime firstDeliveryTime;
            if (args.Length < 4)
            {
                // Загрузка конфигурации из файла.env
                Dictionary<string, string> config = LoadConfig();
                inputFilePath = config["inputFilePath"];
                outputFilePath = config["outputFilePath"];
                logFilePath = config["logFilePath"];
                district = config["district"];
                firstDeliveryTime = DateTime.Parse(config["firstDeliveryTime"]);
            }
            else
            {
                inputFilePath = args[0];
                outputFilePath = args[1];
                logFilePath = args[2];
                district = args[3];
                firstDeliveryTime = DateTime.Parse(args[4]);
            }
            Logger.filePath = logFilePath ?? AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            if (inputFilePath == null) inputFilePath = AppDomain.CurrentDomain.BaseDirectory + "input.csv";
            if (outputFilePath == null) outputFilePath = AppDomain.CurrentDomain.BaseDirectory + "output.csv";

            //OrderController.RandomFill(inputFilePath, 100);

            Logger.Alert("Запуск программы");
            var orders = OrderController.ReadFromFile(inputFilePath);
            Logger.Alert($"{orders.Count} заказов было получено из файла");
            var filteredOrders = OrderController.Filter(orders, district, firstDeliveryTime);
            Logger.Alert($"{filteredOrders.Count} заказов фильтровано для города {district}");
            OrderController.WriteToFile(filteredOrders, outputFilePath);
            Logger.Alert("Отфильтрованные заказы записаны в файл");
            Logger.Alert("Завершение программы");
        }
    }
}
