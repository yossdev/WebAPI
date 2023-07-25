namespace WebAPI.DTOs
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public string Status { get; set; }
        public int Code { get; set; }
    }
}
