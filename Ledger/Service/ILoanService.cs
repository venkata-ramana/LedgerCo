using Ledger.Models;
using System.Threading.Tasks;

namespace Ledger.Service
{
    public interface ILoanService
    {
        Task<LoanDetail> GetLoanDetailsAsync(string bankName, string borrowerName);

        Task<bool> SaveLoanDetailsAsync(LoanDetail loanDetail);
    }
}