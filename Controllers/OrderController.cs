using DeliveryService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace DeliveryService.Controllers
{
    public class OrderController
    {
        public static List<Order> ReadFromFile(string filePath)
        {
            var orders = new List<Order>();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length != 4)
                    {
                        Logger.Alert($"Неверный формат данных: {line}");
                        continue;
                    }
                    try
                    {
                        orders.Add(new Order
                        {
                            Id = Convert.ToString(parts[0]),
                            Weight = double.Parse(parts[1]),
                            District = parts[2],
                            CreatedAt = DateTime.Parse(parts[3])
                        });
                    }
                    catch (FormatException ex)
                    {
                        Logger.Alert($"Ошибка при парсинге данных: {line} - {ex.Message}");
                        continue;
                    }
                }
            }
            return orders;
        }

        public static void WriteToFile(List<Order> orders, string filePath)
        {
            using (var stream = new StreamWriter(filePath))
            {
                stream.WriteLine("Id,Weight,City,CreatedAt");
                foreach (var order in orders)
                {
                    stream.WriteLine($"{Convert.ToString(order.Id)},{order.Weight},{order.District},{Convert.ToString(order.CreatedAt)}");
                }
            }
        }

        public static List<Order> Filter(List<Order> orders, string _cityDistrict, DateTime _firstDeliveryDateTime)
        {
            return orders.Where(o => 
            o.District == _cityDistrict && 
            o.CreatedAt >= _firstDeliveryDateTime &&
            o.CreatedAt <= _firstDeliveryDateTime.AddMinutes(30)
            ).ToList();
        }

        public static void RandomFill(string filePath, int count = 10)
        {
            List<Order> orders = new List<Order>();
            string[] cities = { "Moscow", "Peterburg", "Saratov" };
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                orders.Add(new Order
                {
                    Id = Convert.ToString(i),
                    Weight = random.NextDouble(),
                    District = cities[random.Next(0, 2)],
                    CreatedAt = DateTime.Now.AddMinutes(-random.Next(1, 60 * 24))
                });
            }
            WriteToFile(orders, filePath);
        }
    }
}
