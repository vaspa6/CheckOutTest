using System;

namespace CheckOutTest.Data.Entities
{
    public class Payment
    {
        public Guid ID { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Created { get; set; }
    }
}