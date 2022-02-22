using Ledger.Request;
using Ledger.Response;
using System.Threading.Tasks;

namespace Ledger.Handlers
{
    public interface IRequestHandler
    {
        void SetRequest(BaseRequest request);

        bool ValidateRequest();

        Task<BaseResponse> ProcessAsync();
    }
}
