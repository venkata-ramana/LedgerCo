using Ledger.Response;
using System.Threading.Tasks;

namespace Ledger.Handlers
{
    public interface IRequestHandler
    {
        Task<BaseResponse> ProcessAsync();
    }
}
