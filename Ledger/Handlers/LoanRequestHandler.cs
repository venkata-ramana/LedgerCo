using Ledger.Mapper;
using Ledger.Request;
using Ledger.Response;
using Ledger.Service;
using System;
using System.Threading.Tasks;

namespace Ledger.Handlers
{
    public class LoanRequestHandler : IRequestHandler
    {
        private readonly LoanRequest loanRequest;
        private readonly LoanService loanService;

        public LoanRequestHandler(LoanRequest loanRequest)
        {
            this.loanRequest = loanRequest;
            loanService = new LoanService();
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var loanDetails = await loanService.GetLoanDetailsAsync(loanRequest.BankName, loanRequest.BorrowerName);
            if (loanDetails != null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoanRecordNotFound);
            }

            var loanDetail = loanRequest.ToLoanDetailModel();
            var isSaved = await loanService.SaveLoanDetailsAsync(loanDetail);
            return new BaseResponse() { IsSuccess = isSaved };
        }
    }
}
