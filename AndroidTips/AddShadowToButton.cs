using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace AndroidTips
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class AddShadowToButton : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.shadow_layout);
        }
    }
}
