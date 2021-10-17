using AutoMapper;
using CheckOutTest.Data.BDO;
using CheckOutTest.Data.DTO;
using CheckOutTest.Data.Entities;

namespace CheckOutTest.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProcessPaymentRequestDto, PaymentBdo>();
            CreateMap<PaymentBdo, Payment>();
            CreateMap<Payment, PaymentResponseDto>()
                .AfterMap((src, dest) =>
                {
                    var cardNumber = src.CardNumber;
                    dest.CardNumber = $"xxxx-xxxx-xxxx-{cardNumber.Substring(cardNumber.Length - 4, 4)}";
                });
        }
    }
}