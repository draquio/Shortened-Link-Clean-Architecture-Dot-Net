using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShortenedLinks.Domain.Entities
{
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public virtual User User {  get; set; }
        public virtual ICollection<LinkStatistic> LinkStatistics { get; set; } = new List<LinkStatistic>();
    }
}
