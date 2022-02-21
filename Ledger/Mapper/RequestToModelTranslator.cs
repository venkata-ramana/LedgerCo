using Ledger.Models;
using Ledger.Request;
using System;

namespace Ledger.Mapper
{
    public static class RequestToModelMapper
    {
        public static LoanDetail ToLoanDetailModel(this LoanRequest loanRequest)
        {
            if (loanRequest == null)
                return null;
            return new LoanDetail()
            {
                BankName = loanRequest.BankName,
                BorrowerName = loanRequest.BorrowerName,
                LoanTenure = loanRequest.LoanTenure,
                PrincipalAmount = loanRequest.PrincipalAmount,
                RateOfInterest = loanRequest.RateOfInterest,
                CreateDate = DateTime.UtcNow
            };
        }

        public static Payment ToPaymentModel(this PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
                return null;
            return new Payment()
            {
                Amount = paymentRequest.LumpsumAmount,
                EmiNumber = paymentRequest.Emi
            };
        }
    }
}
