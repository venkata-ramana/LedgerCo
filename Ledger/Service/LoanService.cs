using Ledger.Models;
using System.Threading.Tasks;

namespace Ledger.Service
{
    public class LoanService : BaseService, ILoanService
    {
        public async Task<LoanDetail> GetLoanDetailsAsync(string bankName, string borrowerName)
        {
            return await DataStore.GetLoanDetailsAsync(bankName, borrowerName);
        }

        public async Task<bool> SaveLoanDetailsAsync(LoanDetail loanDetail)
        {
            return await DataStore.SaveLoanDetailsAsync(loanDetail);
        }
    }
}
