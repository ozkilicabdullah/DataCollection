using Polly.Retry;
using SwEpApi.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwEpApi.Services
{

    public interface ISwTaskServiceClient
    {
        Task<ResponseModel> SendRequestAsync(Dictionary<string, object> request, string url);
        Task<ResponseModel> SendRequestAsyncWithPolicy(Dictionary<string, object> request, string url, AsyncRetryPolicy retryPolicy);
    }

}
    