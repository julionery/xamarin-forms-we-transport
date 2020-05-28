using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeTransport.Views;
using WeTransport.ViewModels;
using WeTransport.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WeTransport
{
    public partial class App : Application
    {
        public string AuthToken { get; set; }
        public string KeyUsuario { get; set; }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAwODYzQDMxMzcyZTMxMmUzMFBBRzNOVThNRnh5ZmxoTjlHUWxxYnl0enArZ2pZdFRsaGdtWXJFTUNRT289");

            InitializeComponent();
            InitializaMessagingCenter();
            VerifyLogin();
        }

        #region + Métodos

        private void InitializaMessagingCenter()
        {
            MessagingCenter.Subscribe<EventArgs>(this, "GoToLogin", args =>
            {
                VerifyLogin();
            });

        }

        private void VerifyLogin()
        {
            if (Settings.UserToken.Length > 0 && Settings.UserType.Length > 0)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new NavigationPage(new Views.Start.frmLogin());
            }
        }

        #endregion

        #region + Overrides

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion
    }
}
