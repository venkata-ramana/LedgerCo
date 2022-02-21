using Ledger.Models;
using System.Threading.Tasks;

namespace Ledger.Service
{
    public interface IPaymentService
    {
        Task<bool> SavePaymentAsync(string bankName, string borrowerName, Payment payment);
    }
}
