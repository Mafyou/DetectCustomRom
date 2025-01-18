using MauiDetectStuff.Services;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace MauiDetectStuff;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    public string? RootedStatus { get; set; }
    public string? VPNStatus { get; set; } = "No VPN Detected";
#if ANDROID
    private readonly IRootedDeviceService _rootDeviceService;

    public MainPage(IRootedDeviceService rootDeviceService)
    {
        InitializeComponent();
        _rootDeviceService = rootDeviceService;
        BindingContext = this;    
    }
#endif
#if !ANDROID
    public MainPage()
    {
        InitializeComponent();

    }
#endif
    protected override async void OnAppearing()
    {
#if ANDROID
        var isRooted = await _rootDeviceService.IsRootedAsync(Android.App.Application.Context).ConfigureAwait(false);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RootedStatus = isRooted ? "Rooted" : "Not Rooted";
                OnPropertyChanged(nameof(RootedStatus));
            });
#endif
        CheckForVpn();
    }
    private void CheckForVpn()
    {
        var profiles = Connectivity.Current.ConnectionProfiles;
        foreach (var profile in profiles)
        {
            bool hasInternet = profile == ConnectionProfile.WiFi || profile == ConnectionProfile.Cellular;
            if (hasInternet)
            {
                // Check for VPN by looking for specific network interfaces
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var ni in interfaces)
                {
                    if (IsUnderVPN(ni.Description))
                    {
                        VPNStatus = "VPN Detected";
                        OnPropertyChanged(nameof(VPNStatus));
                    }
                }
            }
        }
    }

    private bool IsUnderVPN(string text)
        => text.Contains("VPN", StringComparison.CurrentCultureIgnoreCase) ||
        text.Contains("tun0", StringComparison.CurrentCultureIgnoreCase) ||
        text.Contains("TAP-Windows Adapter", StringComparison.CurrentCultureIgnoreCase);
}