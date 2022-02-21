using Ledger.Models;
using System.Threading.Tasks;

namespace Ledger.Service
{
    public class PaymentService : BaseService, IPaymentService
    {
        public async Task<bool> SavePaymentAsync(string bankName, string borrowerName, Payment payment)
        {
            return await DataStore.SavePaymentAsync(bankName, borrowerName, payment);
        }
    }
}
