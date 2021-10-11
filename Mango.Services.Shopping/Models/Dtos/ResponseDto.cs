namespace Mango.Services.Shopping.Models.Dtos
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; } = true;
        public T Result { get; set; }
        public string DisplayMessage { get; set; }
        public List<string> Errors { get; set; }
    }
}
