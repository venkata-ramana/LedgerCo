using FluentAssertions;
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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ledger.Test.Handlers
{
    public class PaymentRequestHandlerTest
    {
        private PaymentRequestHandler paymentRequestHandler;
        private PaymentRequest paymentRequest;
        private LoanRequest loanRequest;
        private Mock<IPaymentService> paymentService;
        private Mock<ILoanService> loanService;

        public PaymentRequestHandlerTest()
        {
            InitializeTest();
        }

        private void InitializeTest()
        {
            loanService = new Mock<ILoanService>();
            paymentService = new Mock<IPaymentService>();
            paymentRequestHandler = new PaymentRequestHandler(loanService.Object, paymentService.Object);
            loanRequest = MockRequests._loanFaker.Generate(1).First();
            paymentRequest = MockRequests._paymentFaker.Generate(1).First();
            paymentRequestHandler.SetRequest(paymentRequest);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_LoanLoanRecordsNotFoundAsync()
        {
            loanService.Setup(x => x.GetLoanDetailsAsync(It.IsNotNull<string>(), It.IsNotNull<string>())).ReturnsAsync((LoanDetail)null);
            Func<Task> func = async () => { await paymentRequestHandler.ProcessAsync(); };

            func.Should().ThrowAsync<ArgumentException>().WithMessage(Constants.ErrorMessages.LoanRecordNotFound);
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_Emis_Invalid()
        {
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanDetail.LoanTenure = 0;
            loanService.Setup(x => x.GetLoanDetailsAsync(It.IsNotNull<string>(), It.IsNotNull<string>())).ReturnsAsync(loanDetail);
            Func<Task> func = async () => { await paymentRequestHandler.ProcessAsync(); };

            func.Should().ThrowAsync<ArgumentException>().WithMessage(Constants.ErrorMessages.InvalidEmi);
        }

        [Fact]
        public async Task Should_Return_Response_When_ValidLoan_Details_Found()
        {
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanService.Setup(x => x.GetLoanDetailsAsync(It.IsNotNull<string>(), It.IsNotNull<string>())).ReturnsAsync(loanDetail);
            paymentService.Setup(x => x.SavePaymentAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsNotNull<Payment>())).ReturnsAsync(true);
            var response = await paymentRequestHandler.ProcessAsync();
            response.Should().NotBeNull();
            response.IsSuccess.Should().Be(true);
        }

        [Fact]
        public async Task Should_Return_Response_When_InValidLoan_Details_Found()
        {
            var loanDetail = loanRequest.ToLoanDetailModel();
            loanService.Setup(x => x.GetLoanDetailsAsync(It.IsNotNull<string>(), It.IsNotNull<string>())).ReturnsAsync(loanDetail);
            paymentService.Setup(x => x.SavePaymentAsync(It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsNotNull<Payment>())).ReturnsAsync(false);
            var response = await paymentRequestHandler.ProcessAsync();
            response.Should().NotBeNull();
            response.IsSuccess.Should().Be(false);
        }
    }
}
