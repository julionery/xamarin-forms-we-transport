using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Acr.UserDialogs;
using Xamarin.Forms.GoogleMaps.Android;
using Android;
using Android.Support.V4.Content;
using Plugin.Permissions;
using System.Collections.Generic;

namespace WeTransport.Droid
{
    [Activity(Label = "WeTransport", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        List<string> _permission = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            RequestPermissionsManually();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };

            Xamarin.FormsGoogleMaps.Init(this, bundle, platformConfig);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            FirebaseApp.InitializeApp(Application.Context);
            UserDialogs.Init(this);

            LoadApplication(new App());
        }

        private void RequestPermissionsManually()
        {
            try
            {
                //if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                //    _permission.Add(Manifest.Permission.ReadExternalStorage);

                //if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                //    _permission.Add(Manifest.Permission.WriteExternalStorage);

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted)
                    _permission.Add(Manifest.Permission.AccessFineLocation);

                if (_permission.Count > 0)
                {
                    string[] array = _permission.ToArray();

                    RequestPermissions(array, array.Length);
                }

            }
            catch (Exception)
            {
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == 2)
            {
                if (grantResults.Length == _permission.Count)
                {
                    for (int i = 0; i < requestCode; i++)
                    {
                        if (grantResults[i] != Permission.Granted)
                        {
                            _permission = new List<string>();
                            RequestPermissionsManually();
                            break;
                        }
                    }
                }
            }

            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}