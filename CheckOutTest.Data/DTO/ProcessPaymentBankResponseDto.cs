using CheckOutTest.Data.Helper;
using System;

namespace CheckOutTest.Data.DTO
{
    public class ProcessPaymentBankResponseDto
    {
        public Guid Id { get; set; }
        public PaymentStatusEnum Status { get; set; }
    }
}