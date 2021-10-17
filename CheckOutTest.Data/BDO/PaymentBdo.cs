namespace CheckOutTest.Data.BDO
{
    public class PaymentBdo
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public int CVV { get; set; }
    }
}