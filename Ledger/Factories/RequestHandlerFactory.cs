﻿using Ledger.Handlers;
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
            var loanRequestHandler = DependencyResolver.Resolve<LoanRequestHandler>();
            loanRequestHandler.LoanRequest = loanRequest;
            return loanRequestHandler;
        }

        private IRequestHandler GetPaymentHandler(string[] args)
        {
            var bankName = args.ElementAtOrDefault(1);
            var borrowerName = args.ElementAtOrDefault(2);
            var amount = args.ElementAtOrDefault(3);
            decimal.TryParse(amount, out decimal lumpSumAmount);
            var emi = args.ElementAtOrDefault(4);
            int.TryParse(emi, out int emiNo);

            PaymentRequest paymentRequest = new PaymentRequest(bankName, borrowerName, lumpSumAmount, emiNo);
            var paymentRequestHandler = DependencyResolver.Resolve<PaymentRequestHandler>();
            paymentRequestHandler.PaymentRequest = paymentRequest;
            return paymentRequestHandler;
        }

        private IRequestHandler GetBalanceHandler(string[] args)
        {
            var bankName = args.ElementAtOrDefault(1);
            var borrowerName = args.ElementAtOrDefault(2);
            var emi = args.ElementAtOrDefault(3);
            int.TryParse(emi, out int emiNo);

            BalanceRequest balanceRequest = new BalanceRequest(bankName, borrowerName, emiNo);
            var balanceHandler = DependencyResolver.Resolve<BalanceRequestHandler>();
            balanceHandler.BalanceRequest = balanceRequest;
            return balanceHandler;
        }
    }
}
