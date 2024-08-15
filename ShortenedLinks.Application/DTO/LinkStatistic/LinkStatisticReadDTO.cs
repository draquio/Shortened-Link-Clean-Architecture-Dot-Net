namespace ShortenedLinks.Application.DTO.LinkStatistic
{
    public class LinkStatisticReadDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int LinkId { get; set; }
        public string Link { get; set; }
        public DateTime AccessedAt { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
