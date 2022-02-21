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
        public LoanRequest LoanRequest { get; set; }

        private readonly ILoanService loanService;

        public LoanRequestHandler(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var loanDetails = await loanService.GetLoanDetailsAsync(LoanRequest.BankName, LoanRequest.BorrowerName);
            if (loanDetails != null)
            {
                throw new ArgumentException(Constants.ErrorMessages.LoanRecordNotFound);
            }

            var loanDetail = LoanRequest.ToLoanDetailModel();
            var isSaved = await loanService.SaveLoanDetailsAsync(loanDetail);
            return new BaseResponse() { IsSuccess = isSaved };
        }
    }
}
