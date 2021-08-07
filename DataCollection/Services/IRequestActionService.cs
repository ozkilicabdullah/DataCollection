
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using System.Threading.Tasks;

namespace DataCollection.Services
{
    public interface IRequestActionService
    {
        Task<ResponseModel> Request(ActionRequest Action);
    }
}
