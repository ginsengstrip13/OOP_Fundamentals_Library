using System.Collections.Generic;

namespace SOLID_Fundamentals
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "CreditCard";
        public List<string> Items { get; set; } = new List<string>();
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}