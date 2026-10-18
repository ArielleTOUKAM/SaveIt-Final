using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace SaveIt
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Window?.SetStatusBarColor(Android.Graphics.Color.White);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var decorView = Window?.DecorView;
                if (decorView is not null)
                    decorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            }
        }
    }
}
