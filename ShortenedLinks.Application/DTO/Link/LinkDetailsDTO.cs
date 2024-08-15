namespace ShortenedLinks.Application.DTO.Link
{
    public class LinkDetailsDTO
    {
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
        public string CreatedAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}
