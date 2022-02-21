using Ledger.Handlers;
using Ledger.Request;
using System.Linq;

namespace Ledger.Factories
{
    public class RequestHandlerFactory
    {
        public IRequestHandler GetRequestHandler(string handlerName, string[] args)
        {
            IRequestHandler handler = null;
            switch (handlerName)
            {
                case Constants.Actions.Loan:
                    handler = GetLoanHandler(args);
                    break;
                case Constants.Actions.Balance:
                    handler = GetBalanceHandler(args);
                    break;
                case Constants.Actions.Payment:
                    handler = GetPaymentHandler(args);
                    break;
                default:
                    handler = null;
                    break;
            }

            return handler;
        }

        private IRequestHandler GetLoanHandler(string[] args)
        {
            var bankName = args.ElementAtOrDefault(1);
            var borrowerName = args.ElementAtOrDefault(2);
            var principal = args.ElementAtOrDefault(3);
            decimal.TryParse(principal, out decimal principalAmount);
            var tenure = args.ElementAtOrDefault(4);
            int.TryParse(tenure, out int loanTenure);
            var rateOfInterest = args.ElementAtOrDefault(5);
            decimal.TryParse(rateOfInterest, out decimal roi);

            LoanRequest loanRequest = new LoanRequest(bankName, borrowerName, principalAmount, loanTenure, roi);
            return new LoanRequestHandler(loanRequest);
        }

        private IRequestHandler GetPaymentHandler(string[] args)
        {
            return new PaymentRequestHandler();
        }

        private IRequestHandler GetBalanceHandler(string[] args)
        {
            return new BalanceRequestHandler();
        }
    }
}
