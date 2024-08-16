
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShortenedLinks.Domain.Entities
{
    public class LinkStatistic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        public int LinkId { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitorIp { get; set; }
        public string Country { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public virtual Link Link { get; set; }
    }
}
