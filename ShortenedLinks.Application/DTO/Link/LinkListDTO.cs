namespace ShortenedLinks.Application.DTO.Link
{
    public class LinkListDTO
    {
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
        public string CreatedAt { get; set; }
    }
}
