using MauiAppDetectCustomRom.Service;
using System.ComponentModel;

namespace MauiAppDetectCustomRom
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
#if ANDROID
        private readonly IRootedDeviceService _rootDeviceService;
        public string? RootedStatus { get; set; }

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
        protected override void OnAppearing()
        {
#if ANDROID
            Task.Run(async () =>
            {
                var isRooted = await _rootDeviceService.IsRootedAsync(Android.App.Application.Context).ConfigureAwait(false);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RootedStatus = isRooted ? "Rooted" : "Not Rooted";
                    OnPropertyChanged(nameof(RootedStatus));
                });
            });
#endif
        }
    }
}
