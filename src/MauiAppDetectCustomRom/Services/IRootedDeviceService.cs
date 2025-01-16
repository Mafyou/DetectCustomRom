#if ANDROID
using Android.Content;

namespace MauiAppDetectCustomRom.Service
{
    public interface IRootedDeviceService
    {
        Task<bool> IsRootedAsync(Context context);
    }
}
#endif