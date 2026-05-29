namespace CoworkingManager.Models
{
    public class InsertResult<T>
    {
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess => ErrorMessage == null;
    }
}