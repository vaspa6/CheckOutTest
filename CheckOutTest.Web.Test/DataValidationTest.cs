using CheckOutTest.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CheckOutTest.Web.Test
{
    public class DataValidationTest
    {
        public DataValidationTest()
        {
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(12)]
        [InlineData(-0.209d)]
        [InlineData(11355)]
        [InlineData(1)]
        public void ProcessPaymentRequestDtoTest_BadCVV(int cvv)
        {
            var mockRequest = new ProcessPaymentRequestDto()
            {
                CardNumber = "4407559483963993",
                Amount = 120,
                Currency = "GBP",
                ExpiryDate = "12/23",
                CVV = cvv
            };

            var validationContext = new ValidationContext(mockRequest, null, null);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(mockRequest, validationContext, validationResults, true);

            Assert.False(validationResult);
            Assert.Single(validationResults);
            Assert.Equal("CVV must be a valid 3 digit number (4 for Amex)", validationResults[0].ErrorMessage);
        }

        [Theory]
        [InlineData("1111111111111111")]
        [InlineData("123213")]
        [InlineData("5555-1223-5554-1234")]
        [InlineData("1234/1235/1326/1237")]
        public void ProcessPaymentRequestDtoTest_BadCardNumber(string cardNumber)
        {
            var mockRequest = new ProcessPaymentRequestDto()
            {
                CardNumber = cardNumber,
                Amount = 120,
                Currency = "GBP",
                ExpiryDate = "12/23",
                CVV = 895
            };

            var validationContext = new ValidationContext(mockRequest, null, null);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(mockRequest, validationContext, validationResults, true);

            Assert.False(validationResult);
            Assert.Single(validationResults);
            Assert.Equal("Not a valid card number", validationResults[0].ErrorMessage);
        }

        [Theory]
        [InlineData("12/12/12")]
        [InlineData("12/18")]
        [InlineData("12/12/2021")]
        [InlineData("0a/12s")]
        public void ProcessPaymentRequestDtoTest_BadExpDate(string expiryDate)
        {
            var mockRequest = new ProcessPaymentRequestDto()
            {
                CardNumber = "4407559483963993",
                Amount = 120,
                Currency = "GBP",
                ExpiryDate = expiryDate,
                CVV = 895
            };

            var validationContext = new ValidationContext(mockRequest, null, null);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(mockRequest, validationContext, validationResults, true);

            Assert.False(validationResult);
            Assert.Single(validationResults);
            Assert.Equal("Not valid expiration date", validationResults[0].ErrorMessage);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-0.209d)]
        [InlineData(-26.45232323d)]
        public void ProcessPaymentRequestDtoTest_BadAmount(int amount)
        {
            var mockRequest = new ProcessPaymentRequestDto()
            {
                CardNumber = "4407559483963993",
                Amount = amount,
                Currency = "GBP",
                ExpiryDate = "12/23",
                CVV = 895
            };

            var validationContext = new ValidationContext(mockRequest, null, null);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(mockRequest, validationContext, validationResults, true);

            Assert.False(validationResult);
            Assert.Single(validationResults);
            Assert.Equal("Amount must be a valid positive number", validationResults[0].ErrorMessage);
        }

        [Theory]
        [InlineData("asdasd")]
        [InlineData("asdd")]
        public void ProcessPaymentRequestDtoTest_BadCurrency(string currency)
        {
            var mockRequest = new ProcessPaymentRequestDto()
            {
                CardNumber = "4407559483963993",
                Amount = 120,
                Currency = currency,
                ExpiryDate = "12/23",
                CVV = 895
            };

            var validationContext = new ValidationContext(mockRequest, null, null);
            var validationResults = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject(mockRequest, validationContext, validationResults, true);

            Assert.False(validationResult);
            Assert.Single(validationResults);
            Assert.Equal("Currency must be a currency code", validationResults[0].ErrorMessage);
        }
    }
}