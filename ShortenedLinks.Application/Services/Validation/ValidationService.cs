using ShortenedLinks.Application.Interfaces;
using System;
namespace ShortenedLinks.Application.Services.Validation
{
    public class ValidationService : IValidationService
    {
        public void IsValidLink(string link)
        {
            if (!Uri.TryCreate(link, UriKind.Absolute, out Uri uriResult) ||
                       (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException("The provided Link is not valid.");
            }
        }
        public void IsValidId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID.");
            }
        }
    }
}
