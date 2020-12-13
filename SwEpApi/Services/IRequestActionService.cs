
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using System.Threading.Tasks;

namespace SwEpApi.Services
{
    public interface IRequestActionService
    {
        Task<ResponseModel> Request(ActionRequest Action);
    }
}
