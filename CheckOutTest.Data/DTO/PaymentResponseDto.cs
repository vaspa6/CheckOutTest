namespace CheckOutTest.Data.DTO
{
    public class PaymentResponseDto
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}