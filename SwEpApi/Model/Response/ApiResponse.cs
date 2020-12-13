namespace SwEpApi.Model.Response
{
    public class ApiResponse : ModelBase
    {
        public ApiResponse() { }
        public ApiResponse(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
    }

}
