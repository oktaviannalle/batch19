namespace PendataanBarang.Helpers
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = "Berhasil")
            => new ServiceResult<T> { Success = true, Message = message, Data = data };

        public static ServiceResult<T> FailResult(string message)
            => new ServiceResult<T> { Success = false, Message = message, Data = default };
    }
}