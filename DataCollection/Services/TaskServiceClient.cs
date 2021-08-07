using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestSharp;
using DataCollection.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DataCollection.Services
{
    public class TaskServiceClient : ITaskServiceClient
    {

        private readonly IConfiguration Configuration;

        public TaskServiceClient(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// girişten sonra 3 saniye aralıkla istenen kere denereyerek istek yapar..
        /// hepsinden verilen policy hatası ya da sonucu dönerse cevap yok demektir.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseModel> SendRequestAsyncWithPolicy(Dictionary<string, object> request, string url, AsyncRetryPolicy retryPolicy)
        {

            var response = await retryPolicy.ExecuteAsync(async () =>
            {
                return await SendRequestAsync(request, url);
            });

            return response;
        }

        public static AsyncRetryPolicy HttpExceptionPolicy(int Attempts, TimeSpan pauseBetweenFailures)
        {

            return Policy
                 .Handle<Exception>()
                 .WaitAndRetryAsync(Attempts, i => pauseBetweenFailures);

        }

        private RestRequest GetRestRequest(Dictionary<string, object> request, string url)
        {
         
            RestRequest restRequest = new RestRequest(url, Method.POST);
            restRequest.AddParameter("application/json", JsonConvert.SerializeObject(request), ParameterType.RequestBody);

            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            return restRequest;

        }

        public async Task<ResponseModel> SendRequestAsync(Dictionary<string, object> request, string url)
        {

            var response = new ResponseModel
            {
                Errors = new List<string>(),
                Success = false
            };

            RestClient client = new RestClient(url);
            var restRequest = GetRestRequest(request, url);
            var result = await client.ExecuteAsync(restRequest);

            if (!result.IsSuccessful)
            {
                switch (result.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        //token bitmiş olabilir.
                        break;
                    case HttpStatusCode code when ((int)result.StatusCode >= 500 && (int)result.StatusCode < 600):
                        throw new Exception("Üzgünüz :( Beklenmeyen bir hata oluştu. En kısa sürede çözümü için çalışacağız.");
                }
            }

            response = JsonConvert.DeserializeObject<ResponseModel>(result.Content);
            return response;

        }

    }
}
