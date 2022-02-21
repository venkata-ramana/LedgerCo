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
        public LoanRequest loanRequest { get; set; }

        private readonly ILoanService loanService;

        public LoanRequestHandler(ILoanService loanService)
        {
            this.loanService = loanService;
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
