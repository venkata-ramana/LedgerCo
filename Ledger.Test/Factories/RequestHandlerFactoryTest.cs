using FluentAssertions;
using Ledger.Factories;
using Ledger.Handlers;
using Xunit;

namespace Ledger.Test.Factories
{
    public class RequestHandlerFactoryTest
    {
        private RequestHandlerFactory factory;

        public RequestHandlerFactoryTest()
        {
            factory = new RequestHandlerFactory();
        }

        [Fact]
        public void Should_Return_Null_When_HandlerNotFound()
        {
            var handler = factory.GetRequestHandler(string.Empty, new string[] { });

            handler.Should().BeNull();
        }

        [Fact]
        public void Should_Return_LoanHandler_When_Loan_Handler_Name_Provided()
        {
            var handler = factory.GetRequestHandler(Constants.Actions.Loan, new string[] { });

            handler.Should().NotBeNull();
            handler.GetType().Should().Be(typeof(LoanRequestHandler));
        }

        [Fact]
        public void Should_Return_PaymentHandler_When_Payment_Handler_Name_Provided()
        {
            var handler = factory.GetRequestHandler(Constants.Actions.Payment, new string[] { });

            handler.Should().NotBeNull();
            handler.GetType().Should().Be(typeof(PaymentRequestHandler));
        }

        [Fact]
        public void Should_Return_BalanceHandler_When_Balance_Handler_Name_Provided()
        {
            var handler = factory.GetRequestHandler(Constants.Actions.Balance, new string[] { });

            handler.Should().NotBeNull();
            handler.GetType().Should().Be(typeof(BalanceRequestHandler));
        }
    }
}
