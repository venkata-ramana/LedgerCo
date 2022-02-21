using Ledger.Request;
using Ledger.Response;
using Ledger.Service;
using System;
using System.Threading.Tasks;

namespace Ledger.Handlers
{
    public class BalanceRequestHandler : IRequestHandler
    {
        public BalanceRequest BalanceRequest { get; set; }

        private ILoanService LoanService { get; set; }

        public BalanceRequestHandler(ILoanService loanService)
        {
            this.LoanService = loanService;
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var existingLoanRecord = await LoanService.GetLoanDetailsAsync(BalanceRequest.BankName, BalanceRequest.BorrowerName);
            if (existingLoanRecord == null)
                throw new ArgumentException(Constants.ErrorMessages.LoanRecordNotFound);

            var emiAmount = existingLoanRecord.EmiAmount();
            var totalAmountByEmi = BalanceRequest.Emi * emiAmount;
            var totalLumpSumPaid = existingLoanRecord.LumpSumPaidTillEmiNumber(BalanceRequest.Emi);

            var totalAmountPaidTillEmi = totalAmountByEmi + totalLumpSumPaid;

            var amountPending = existingLoanRecord.TotalAmountToBeRepaid() - totalAmountPaidTillEmi;
            var remainingEmis = Math.Ceiling(amountPending / emiAmount);

            BalanceResponse balanceResponse = new BalanceResponse()
            {
                AmountPaid = totalAmountPaidTillEmi,
                BankName = existingLoanRecord.BankName,
                BorrowerName = existingLoanRecord.BorrowerName,
                RemainingEmis = remainingEmis > 0 ? (int)remainingEmis : 0,
                IsSuccess = true
            };

            return balanceResponse;
        }
    }
}
