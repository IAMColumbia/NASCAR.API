namespace Nascar.Api.Models
{
    public class ResponseDto<T> : ResponseDto
    {
        public T? Data { get; set; }
        public ResponseDto(T data) 
        {
            Data = data;
        }
        public ResponseDto() { }
    }

    public class ResponseDto 
    {
        public List<string>? Errors { get; set; }
        public static ResponseDto<T> Success<T>(T data)
        {
            return new ResponseDto<T>(data);
        }
    }
}
