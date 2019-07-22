using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using MapsLiveUpdate.Resources;
using Plugin.Geolocator;

namespace MapsLiveUpdate
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        MapFragment mapFragment;
        MappingPermissionsHelper mappingPermissionsHelper;
        GoogleMap maps;
        Task<bool> getLocationPermissionAsync;
        LocationManager locationManager;
        Location currentLocation;
        string provider;
        double lang;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            mappingPermissionsHelper = new MappingPermissionsHelper(this);
            getLocationPermissionAsync = mappingPermissionsHelper.CheckAndRequestPermissions();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.content_main);
            mapFragment = FragmentManager.FindFragmentById(Resource.Id.map) as MapFragment;

            InitializeLocationManager();
             
            //currentLocation = locationManager.GetLastKnownLocation(provider);

            mapFragment.GetMapAsync(this);
        }
        private void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine,
                PowerRequirement = Power.NoRequirement
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                provider = acceptableLocationProviders.First();
            }
            else
            {
                provider = string.Empty;
            }
            
            currentLocation = locationManager.GetLastKnownLocation(provider);
        }
        public async void OnMapReady(GoogleMap googleMap)
        {
            maps = googleMap;
            maps.MapType = GoogleMap.MapTypeHybrid;
            maps.UiSettings.MyLocationButtonEnabled = true;
            maps.UiSettings.CompassEnabled = true;
            maps.UiSettings.ZoomGesturesEnabled = true;
            maps.UiSettings.ZoomControlsEnabled = true;
            maps.UiSettings.MapToolbarEnabled = true;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            LatLng location = new LatLng(position.Latitude, position.Longitude);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(17);
            builder.Bearing(60);
            //builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            maps.MoveCamera(cameraUpdate);

            LatLng locationMarker = new LatLng(position.Latitude, position.Longitude);
            Marker marker = maps.AddMarker(new MarkerOptions().SetTitle($"You're in Latitude {position.Latitude} & {position.Longitude} Location").SetPosition(locationMarker).SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRose)));
            

            var hasLocationPermission = await getLocationPermissionAsync;
            maps.MyLocationEnabled = hasLocationPermission;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(provider, 0, 0, this);
        }
        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {

            currentLocation = location;

            MarkerOptions mo = new MarkerOptions();
            mo.SetPosition(new LatLng(currentLocation.Latitude, currentLocation.Longitude));

            mo.SetTitle("You are here!");
            maps.AddMarker(mo);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(currentLocation.Latitude, currentLocation.Longitude));
            CameraPosition camPos = builder.Build();
            CameraUpdate camUpdate = CameraUpdateFactory.NewCameraPosition(camPos);
            maps.MoveCamera(camUpdate);
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new System.NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new System.NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new System.NotImplementedException();
        }
    }
}

