using Ledger.Handlers;
using Ledger.Service;
using SimpleInjector;

namespace Ledger.Test
{
    public static class RegisterDependency
    {
        private static Container container;

        public static void Register()
        {
            container = new Container();
            container.Register<ILoanService, LoanService>();
            container.Register<IPaymentService, PaymentService>();
            container.Register<LoanRequestHandler>();
            container.Register<PaymentRequestHandler>();
            container.Register<BalanceRequestHandler>();
            DependencyResolver.container = container;
        }
    }
}
