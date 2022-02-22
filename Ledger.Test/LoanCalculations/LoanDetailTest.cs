using Ledger.Models;
using System.Collections.Generic;
using Xunit;

namespace Ledger.Test.LoanCalculations
{
    public class LoanDetailTest
    {
        [Fact]
        public void ShouldReturn_TotalAmountToBeRepaid_Zero_When_LoanTenureIsZero()
        {
            var loanDetail = new LoanDetail();
            loanDetail.BankName = "IDIDI";
            loanDetail.BorrowerName = "DALE";
            loanDetail.LoanTenure = 0;

            Assert.Equal(decimal.Zero, loanDetail.TotalAmountToBeRepaid);
        }

        [Fact]
        public void ShouldReturn_TotalAmountToBeRepaid_When_LoanTenureIsNotZero()
        {
            var loanDetail = new LoanDetail();
            loanDetail.BankName = "IDIDI";
            loanDetail.BorrowerName = "DALE";
            loanDetail.PrincipalAmount = 100;
            loanDetail.RateOfInterest = 10;
            loanDetail.LoanTenure = 1;

            Assert.Equal(110, loanDetail.TotalAmountToBeRepaid);
        }

        [Fact]
        public void ShouldReturn_EmiAmount_Zero_When_RepaidAmountIsZero()
        {
            var loanDetail = new LoanDetail();
            loanDetail.BankName = "IDIDI";
            loanDetail.BorrowerName = "DALE";
            loanDetail.PrincipalAmount = 100;
            loanDetail.RateOfInterest = 10;
            loanDetail.LoanTenure = 0;

            Assert.Equal(decimal.Zero, loanDetail.EmiAmount);
        }

        [Fact]
        public void ShouldReturn_EmiAmount_When_RepaidAmount_GreatherThanZero()
        {
            var loanDetail = new LoanDetail();
            loanDetail.BankName = "IDIDI";
            loanDetail.BorrowerName = "DALE";
            loanDetail.PrincipalAmount = 100;
            loanDetail.RateOfInterest = 10;
            loanDetail.LoanTenure = 1;

            Assert.Equal(10, loanDetail.EmiAmount);
        }

        [Fact]
        public void ShouldReturn_LumpSumPaidTillEmiNumber_Zero_When_NoPaymentsMade()
        {
            var loanDetail = new LoanDetail();
            loanDetail.BankName = "IDIDI";
            loanDetail.BorrowerName = "DALE";
            loanDetail.PrincipalAmount = 100;
            loanDetail.RateOfInterest = 10;
            loanDetail.LoanTenure = 1;

            Assert.Equal(decimal.Zero, loanDetail.LumpSumPaidTillEmiNumber(1));
        }

        [Fact]
        public void ShouldReturn_LumpSumPaidTillEmiNumber_When_PaymentsMade()
        {
            var loanDetail = new LoanDetail();
            loanDetail.BankName = "IDIDI";
            loanDetail.BorrowerName = "DALE";
            loanDetail.PrincipalAmount = 100;
            loanDetail.RateOfInterest = 10;
            loanDetail.LoanTenure = 1;
            loanDetail.Payments = new List<Payment>();
            loanDetail.Payments.Add(new Payment(3, 200));
            loanDetail.Payments.Add(new Payment(5, 200));
            loanDetail.Payments.Add(new Payment(9, 200));

            Assert.Equal(200, loanDetail.LumpSumPaidTillEmiNumber(4));
            Assert.Equal(400, loanDetail.LumpSumPaidTillEmiNumber(6));
            Assert.Equal(600, loanDetail.LumpSumPaidTillEmiNumber(9));
        }
    }
}
