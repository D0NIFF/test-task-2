using System;

namespace DeliveryService.Models
{
    public class Order
    {
        public string Id { get; set; }

        public double Weight { get; set; }

        public string District { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
