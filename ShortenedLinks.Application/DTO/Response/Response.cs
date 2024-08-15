

using System.Text.Json.Serialization;

namespace ShortenedLinks.Application.DTO.Response
{
    public class Response<T>
    {
        public bool status { get; set; }
        public T? value { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? msg { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? errors { get; set; }
    }
}
