using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.DTO.LinkStatistic
{
    public class LinkClicksStatisticTopDTO
    {
        public int LinkId { get; set; }
        public int ClickCount { get; set; }
        public LinkListDTO Link { get; set; }
        public PeriodType PeriodType { get; set; }
    }
}
