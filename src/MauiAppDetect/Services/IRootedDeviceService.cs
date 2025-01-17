#if ANDROID
using Android.Content;

namespace MauiMauiDetectCustomRom.Services;

public interface IRootedDeviceService
{
    Task<bool> IsRootedAsync(Context context);
}
#endif