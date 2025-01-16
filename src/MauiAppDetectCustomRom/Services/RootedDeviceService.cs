#if ANDROID
using Android.Content;
#endif

namespace MauiAppDetectCustomRom.Service;

#if ANDROID
public class RootedDeviceService : IRootedDeviceService
{
    /// <summary>
    /// Checks if the device is rooted.
    /// </summary>
    /// <param name="context">The context of the application.</param>
    /// <returns>True if the device is rooted, otherwise false.</returns>
    public async Task<bool> IsRootedAsync(Context context)
    {
        return await MainActivity.IsRootedAsync(context);
    }
}
#endif