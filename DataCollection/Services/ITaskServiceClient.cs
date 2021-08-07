using Polly.Retry;
using DataCollection.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services
{

    public interface ITaskServiceClient
    {
        Task<ResponseModel> SendRequestAsync(Dictionary<string, object> request, string url);
        Task<ResponseModel> SendRequestAsyncWithPolicy(Dictionary<string, object> request, string url, AsyncRetryPolicy retryPolicy);
    }

}
    