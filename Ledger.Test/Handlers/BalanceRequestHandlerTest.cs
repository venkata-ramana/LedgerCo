using FluentAssertions;
using Ledger.Exceptions;
using Ledger.Handlers;
using Ledger.Mapper;
using Ledger.Models;
using Ledger.Request;
using Ledger.Service;
using Ledger.Test.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ledger.Test.Handlers
{
    public class BalanceRequestHandlerTest
    {
        private BalanceRequestHandler balanceRequestHandler;
        private Mock<ILoanService> loanService;
        private BalanceRequest balanceRequest;

        public BalanceRequestHandlerTest()
        {
            loanService = new Mock<ILoanService>();
            balanceRequestHandler = new BalanceRequestHandler(loanService.Object);
            balanceRequest = Mocks.MockRequests._balanceFaker.Generate(1).First();
            balanceRequestHandler.SetRequest(balanceRequest);
        }

        [Fact]
        public void Should_Throw_RecordNotFoundException_When_LoanRecordNotFound()
        {
            loanService.Setup(x => x.GetLoanDetailsAsync(balanceRequest.BankName, balanceRequest.BorrowerName)).ReturnsAsync((LoanDetail)null);
            var exception = Assert.ThrowsAsync<RecordNotFoundException>(async () => await balanceRequestHandler.ProcessAsync()).GetAwaiter().GetResult();
            Assert.Equal(String.Format(Constants.ErrorMessages.LoanRecordNotFound), exception.Message);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidBankNameProvided()
        {
            balanceRequest.BankName = string.Empty;
            var ex = Assert.Throws<ArgumentException>(() => balanceRequestHandler.ValidateRequest());
            Assert.Equal($"{Constants.Actions.Balance}: {Constants.ErrorMessages.BankNameRequired}", ex.Message);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidBorrowerProvided()
        {
            balanceRequest.BorrowerName = string.Empty;
            var ex = Assert.Throws<ArgumentException>(() => balanceRequestHandler.ValidateRequest());
            Assert.Equal($"{Constants.Actions.Balance}: {Constants.ErrorMessages.BorrowerNameRequired}", ex.Message);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidEmiProvided()
        {
            balanceRequest.Emi = -1;
            var ex = Assert.Throws<ArgumentException>(() => balanceRequestHandler.ValidateRequest());
            Assert.Equal($"{Constants.Actions.Balance}: {Constants.ErrorMessages.InvalidEmi}", ex.Message);
        }

        [Fact]
        public void Should_Return_True_When_ValidBalanceRequestProvided()
        {
            Assert.True(balanceRequestHandler.ValidateRequest());
        }

        [Fact]
        public async Task HandleAsync_ValidBalanceRequest_Returns_ValidBalanceReponse()
        {
            var bankName = "IDIDI";
            var borrowerName = "Dale";
            var loanRequest = new LoanRequest(bankName, borrowerName, 10000, 5, 4);
            var loanDetail = loanRequest.ToLoanDetailModel();
            var balanceRequest = new BalanceRequest(bankName, borrowerName, 5);
            balanceRequestHandler.SetRequest(balanceRequest);
            var validBalanceResponse = MockResponse.GetValidBalanceResponseWithoutPayment();
            loanService.Setup(x => x.GetLoanDetailsAsync(bankName, borrowerName)).ReturnsAsync(loanDetail);
            var response = await balanceRequestHandler.ProcessAsync();
            response.Should().NotBeNull();
            response.IsSuccess.Should().Be(true);
            response.Should().BeEquivalentTo(validBalanceResponse);
        }

        [Fact]
        public async Task HandleAsync_ValidBalanceRequestWithLumpsumPayment_Returns_ValidBalanceReponse()
        {
            var bankName = "IDIDI";
            var borrowerName = "Dale";
            var loanRequest = new LoanRequest(bankName, borrowerName, 5000, 1, 6);
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanDetail.Payments = new List<Models.Payment>();
            loanDetail.Payments.Add(new Models.Payment() { Amount = 1000, EmiNumber = 5 });

            var balanceRequest = new BalanceRequest(bankName, borrowerName, 6);
            balanceRequestHandler.SetRequest(balanceRequest);
            var validBalanceResponse = MockResponse.GetValidBalanceResponseWithPayment();
            loanService.Setup(x => x.GetLoanDetailsAsync(bankName, borrowerName)).ReturnsAsync(loanDetail);
            var response = await balanceRequestHandler.ProcessAsync();
            response.Should().NotBeNull();
            response.IsSuccess.Should().Be(true);
            response.Should().BeEquivalentTo(validBalanceResponse);
        }
    }
}
