using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Interfaces
{
    public interface IValidationService
    {
        void IsValidId(int id);
        void IsValidLink(string link);
    }
}
