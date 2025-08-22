namespace TodoApp.Models.Common
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }
        public ApiResponse(T data, bool success = true, string message = "")
        {
            Data = data;
            Success = success;
            Message = message;
        }

        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
