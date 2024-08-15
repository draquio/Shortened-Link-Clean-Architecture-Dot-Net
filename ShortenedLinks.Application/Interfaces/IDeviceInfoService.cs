
using ShortenedLinks.Application.DTO.Device;

namespace ShortenedLinks.Application.Interfaces
{
    public interface IDeviceInfoService
    {
        DeviceInfoDTO GetDeviceType(string userAgent);
    }
}
