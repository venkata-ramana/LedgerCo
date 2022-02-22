using FluentAssertions;
using Ledger.Handlers;
using Ledger.Models;
using Ledger.Request;
using Ledger.Service;
using Ledger.Test.Mocks;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ledger.Test.Handlers
{
    public class LoanRequestHandlerTest
    {
        private LoanRequestHandler loanRequestHandler;
        private Mock<ILoanService> loanService;
        private LoanRequest loanRequest;

        public LoanRequestHandlerTest()
        {
            InitializeTest();
        }

        internal void InitializeTest()
        {
            loanService = new Mock<ILoanService>();
            loanRequest = MockRequests._loanFaker.Generate(1).First();
            loanRequestHandler = new LoanRequestHandler(loanService.Object);
            loanRequestHandler.SetRequest(loanRequest);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidBankNameProvided()
        {
            loanRequest.BankName = string.Empty;

            var ex = Assert.Throws<ArgumentException>(() => loanRequestHandler.ValidateRequest());

            Assert.Equal($"{Constants.Actions.Loan}: {Constants.ErrorMessages.BankNameRequired}", ex.Message);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidLoanTenureProvided()
        {
            loanRequest.LoanTenure = 0;

            var ex = Assert.Throws<ArgumentException>(() => loanRequestHandler.ValidateRequest());

            Assert.Equal($"{Constants.Actions.Loan}: {Constants.ErrorMessages.LoanTenureMustBeAtleastOneYear}", ex.Message);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidPrincipalAmountProvided()
        {
            loanRequest.PrincipalAmount = 0;

            var ex = Assert.Throws<ArgumentException>(() => loanRequestHandler.ValidateRequest());

            Assert.Equal($"{Constants.Actions.Loan}: {Constants.ErrorMessages.PrincipleAmountShouldNotBeZero}", ex.Message);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_InvalidRateOfInterestProvided()
        {
            loanRequest.RateOfInterest = 0;

            var ex = Assert.Throws<ArgumentException>(() => loanRequestHandler.ValidateRequest());

            Assert.Equal($"{Constants.Actions.Loan}: {Constants.ErrorMessages.RateOfInterestShouldNotBeZero}", ex.Message);
        }

        [Fact]
        public void Should_Return_True_When_ValidPaymentRequestProvided()
        {
            Assert.True(loanRequestHandler.ValidateRequest());
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_LoanRecordsNotFoundAsync()
        {
            Func<Task> func = async () => { await loanRequestHandler.ProcessAsync(); };

            func.Should().ThrowAsync<ArgumentException>().WithMessage(Constants.ErrorMessages.LoanRecordNotFound);
        }

        [Fact]
        public async Task Should_Succeed_WhenValidLoanDetails_ProvidedAsync()
        {
            loanService.Setup(x => x.GetLoanDetailsAsync(loanRequest.BankName, loanRequest.BorrowerName)).ReturnsAsync((LoanDetail)null);
            loanService.Setup(x => x.SaveLoanDetailsAsync(It.IsNotNull<LoanDetail>())).ReturnsAsync(true);

            var response = await loanRequestHandler.ProcessAsync();

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }
    }
}
