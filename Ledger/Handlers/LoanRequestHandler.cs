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
        private LoanRequest loanRequest { get; set; }

        private readonly ILoanService loanService;

        public LoanRequestHandler(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        public void SetRequest(BaseRequest request)
        {
            this.loanRequest = (LoanRequest)request;
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var loanDetails = await loanService.GetLoanDetailsAsync(loanRequest.BankName, loanRequest.BorrowerName);
            if (loanDetails != null)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.DuplicateLoanRecord, loanRequest.BankName, loanRequest.BorrowerName)) ;
            }

            var loanDetail = loanRequest.ToLoanDetailModel();
            var isSaved = await loanService.SaveLoanDetailsAsync(loanDetail);
            return new BaseResponse() { IsSuccess = isSaved };
        }
    }
}
