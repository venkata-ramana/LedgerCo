using Ledger.Models;
using System.Threading.Tasks;

namespace Ledger.Service
{
    internal class LoanService : BaseService
    {
        internal async Task<LoanDetail> GetLoanDetailsAsync(string bankName, string borrowerName)
        {
            return await DataStore.GetLoanDetailsAsync(bankName, borrowerName);
        }

        internal async Task<bool> SaveLoanDetailsAsync(LoanDetail loanDetail)
        {
            return await DataStore.SaveLoanDetailsAsync(loanDetail);
        }
    }
}
