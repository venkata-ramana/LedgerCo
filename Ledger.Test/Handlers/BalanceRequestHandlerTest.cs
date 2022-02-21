using FluentAssertions;
using Ledger.Handlers;
using Ledger.Mapper;
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
        public void Should_Throw_ArgumentException_When_LoanRecordsNotFoundAsync()
        {
            Func<Task> func = async () => { await balanceRequestHandler.ProcessAsync(); };

            func.Should().ThrowAsync<ArgumentException>().WithMessage(Constants.ErrorMessages.LoanRecordNotFound);
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
