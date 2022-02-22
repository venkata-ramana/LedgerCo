using Ledger.Exceptions;
using Ledger.Request;
using Ledger.Response;
using Ledger.Service;
using System;
using System.Threading.Tasks;

namespace Ledger.Handlers
{
    public class BalanceRequestHandler : IRequestHandler
    {
        private BalanceRequest balanceRequest { get; set; }

        private ILoanService LoanService { get; set; }

        public BalanceRequestHandler(ILoanService loanService)
        {
            this.LoanService = loanService;
        }

        public void SetRequest(BaseRequest request)
        {
            this.balanceRequest = (BalanceRequest)request;
        }

        public bool ValidateRequest()
        {
            if (String.IsNullOrEmpty(balanceRequest.BankName))
                throw new ArgumentException($"{Constants.Actions.Balance}: {Constants.ErrorMessages.BankNameRequired}");

            if (String.IsNullOrEmpty(balanceRequest.BorrowerName))
                throw new ArgumentException($"{Constants.Actions.Balance}: {Constants.ErrorMessages.BorrowerNameRequired}");

            if (balanceRequest.Emi < 0)
                throw new ArgumentException($"{Constants.Actions.Balance}: {Constants.ErrorMessages.InvalidEmi}");

            return true;
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var existingLoanRecord = await LoanService.GetLoanDetailsAsync(balanceRequest.BankName, balanceRequest.BorrowerName);
            if (existingLoanRecord == null)
                throw new RecordNotFoundException(Constants.ErrorMessages.LoanRecordNotFound);

            var emiAmount = existingLoanRecord.EmiAmount;
            var totalAmountByEmi = balanceRequest.Emi * emiAmount;
            var totalLumpSumPaid = existingLoanRecord.LumpSumPaidTillEmiNumber(balanceRequest.Emi);

            var totalAmountPaidTillEmi = totalAmountByEmi + totalLumpSumPaid;

            var amountPending = existingLoanRecord.TotalAmountToBeRepaid- totalAmountPaidTillEmi;
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
