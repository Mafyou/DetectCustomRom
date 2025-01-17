using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace MauiDetectStuff;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    /// <summary>
    /// Checks if the device is rooted.
    /// </summary>
    /// <param name="context">The context of the application.</param>
    /// <returns>True if the device is rooted, otherwise false.</returns>
    public static async Task<bool> IsRootedAsync(Context context)
    {
        try
        {
            return await Task.Run(() => CheckRootMethod1() || CheckRootMethod2() || CheckRootMethod3());
        }
        catch (IOException ioEx)
        {
            // Log the IO exception details
            Android.Util.Log.Error("RootCheck", $"IO Exception: {ioEx}");
        }
        catch (UnauthorizedAccessException uaEx)
        {
            // Log the unauthorized access exception details
            Android.Util.Log.Error("RootCheck", $"Unauthorized Access: {uaEx}");
        }
        catch (Exception ex)
        {
            // Log the general exception details
            Android.Util.Log.Error("RootCheck", $"Exception: {ex}");
        }
        return false;
    }

    /// <summary>
    /// Checks if the /proc/net/tcp file is readable.
    /// </summary>
    /// <returns>True if the file is not readable, indicating a rooted device.</returns>
    private static bool CheckRootMethod1()
    {
        var file = new Java.IO.File("/proc/net/tcp");
        return !file.CanRead();
    }


    /// <summary>
    /// Checks for the presence of su binary.
    /// </summary>
    /// <returns>True if su binary is found, indicating a rooted device.</returns>
    private static bool CheckRootMethod2()
    {
        var paths = new string[] { "/sbin/", "/system/bin/", "/system/xbin/", "/data/local/xbin/", "/data/local/bin/", "/system/sd/xbin/", "/system/bin/failsafe/", "/data/local/" };
        return paths.Any(path => new Java.IO.File(path + "su").Exists());
    }

    /// <summary>
    /// Checks for the presence of dangerous properties.
    /// </summary>
    /// <returns>True if dangerous properties are found, indicating a rooted device.</returns>
    private static bool CheckRootMethod3()
    {
        var buildTags = Build.Tags;
        return buildTags != null && buildTags.Contains("test-keys");
    }
}
