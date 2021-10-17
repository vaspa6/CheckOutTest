using CheckOutTest.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CheckOutTest.Data.DTO
{
    public class ProcessPaymentRequestDto
    {
        [BankCardAnotation]
        public string CardNumber { get; set; }
        [ExpiryDateAnotation]
        public string ExpiryDate { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Amount must be a valid number")]
        public int Amount { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "Currency must be a currency code")]
        public string Currency { get; set; }
        [Required]
        [Range(0, 9999, ErrorMessage = "CVV must be a valid 3 digit number (4 for Amex)")]
        public int CVV { get; set; }
    }
}