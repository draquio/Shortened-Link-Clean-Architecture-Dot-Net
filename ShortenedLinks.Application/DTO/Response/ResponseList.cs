

namespace ShortenedLinks.Application.DTO.Response
{
    public class ResponseList<T> : Response<T>
    {
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
