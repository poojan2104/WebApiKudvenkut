using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiKudvenkut.Models
{
    public class Orders
    {
        public double Total { get; set; }
        public List<Order> OrdersList { get; set; }
    }

    public class Order
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
        public double Total { get; set; }
    }
}