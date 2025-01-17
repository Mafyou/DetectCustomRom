#if ANDROID
using Android.Content;

namespace MauiDetectStuff.Services;

public interface IRootedDeviceService
{
    Task<bool> IsRootedAsync(Context context);
}
#endif