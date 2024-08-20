using ShortenedLinks.Application.Services.DeviceInfo;

namespace ShortenedLinks.Application.Interfaces
{
    public interface IDeviceInfoService
    {
        DeviceInfo GetDeviceType(string userAgent);
    }
}
