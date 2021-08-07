using DataCollection.Contracts;

namespace DataCollection.Model.Response
{
    public class ApiResponse : ModelBase
    {
        public ApiResponse() { }
        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
    }

}
