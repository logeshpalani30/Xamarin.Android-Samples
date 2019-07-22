using System;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace MapsLiveUpdate.Resources
{
    public class MappingPermissionsHelper
    {
        readonly Activity currentActivity;
        const int MappingPermissionsRequestCode = 1;
        static string[] requiredPermissions = new[] { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };

        readonly TaskCompletionSource<bool> hasPermissionAsyncSource;

        public MappingPermissionsHelper(Activity activity)
        {
            currentActivity = activity;
            hasPermissionAsyncSource = new TaskCompletionSource<bool>();
        }

        public bool HasMappingPermissions()
        {
            return requiredPermissions.All(permission => currentActivity.CheckSelfPermission(permission) == Permission.Granted);
        }
        public bool ShouldShowPermissionRationale()
        {
            // Has user previously rejected a permission we need. We should show something to explain, with an option that will ask for permission again.
            return requiredPermissions.Any(permission => currentActivity.ShouldShowRequestPermissionRationale(permission));
        }

        void RequestPermissions()
        {
            currentActivity.RequestPermissions(requiredPermissions, MappingPermissionsRequestCode);
        }

        public Task<bool> CheckAndRequestPermissions()
        {
            // Only required for Android Marshmallow and newer.
            if ((int)Build.VERSION.SdkInt < 23)
            {
                hasPermissionAsyncSource.TrySetResult(true);
                return hasPermissionAsyncSource.Task;
            }

            if (HasMappingPermissions())
            {
                hasPermissionAsyncSource.TrySetResult(true);
                return hasPermissionAsyncSource.Task;
            }

            if (ShouldShowPermissionRationale())
            {
                ShowPermissionRationale();
            }
            else
            {
                RequestPermissions();
            }

            return hasPermissionAsyncSource.Task;
        }

        void ShowPermissionRationale()
        {
            // Show something to explain, with an option that will ask for permission again.
            var permissionExplanationAlert = new Android.Support.V7.App.AlertDialog.Builder(currentActivity)
                .SetMessage($"{nameof(MapsLiveUpdate)} needs location permission to map your position. Can we have permission to use your location?")
                .SetPositiveButton("Allow location use", (sender, args) =>
                {
                    // User convinced to let us ask again. Have Android ask for permissions we need.
                    RequestPermissions();
                })
                .SetNegativeButton("No thanks", (sender, args) =>
                {
                    var cannotProceedWithoutPermissionAlert = new Android.App.AlertDialog.Builder(currentActivity)
                        .SetMessage("Without permission to use your location, you won't see your position on the map.")
                        .SetPositiveButton("Okay", (s, a) => { })
                        .Create();
                    cannotProceedWithoutPermissionAlert.Show();
                })
                .Create();
            permissionExplanationAlert.Show();
        }

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode != MappingPermissionsRequestCode) { return; }

            if (permissions.Length != requiredPermissions.Length
                || grantResults.Any(grantResult => grantResult == Permission.Denied))
            {
                var cannotProceedWithoutPermissionAlert = new Android.App.AlertDialog.Builder(currentActivity)
                    .SetMessage("Without permission to use your location, you won't see your position on the map.")
                    .SetPositiveButton("Okay", (s, a) => { })
                    .Create();
                cannotProceedWithoutPermissionAlert.Show();
                hasPermissionAsyncSource.TrySetResult(false);
            }
            else
            {
                hasPermissionAsyncSource.TrySetResult(true);
            }
        }
    }
}
