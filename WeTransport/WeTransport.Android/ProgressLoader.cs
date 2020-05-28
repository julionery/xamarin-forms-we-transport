using System;
using AndroidHUD;
using WeTransport.Components.Interfaces;
using WeTransport.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProgressLoader))]
namespace WeTransport.Droid
{
    public class ProgressLoader : IProgressInterface
    {
        public ProgressLoader()
        {
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.Dismiss();
            });
        }

        public void Show(string tittle = "Carregando...")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.Show(Forms.Context, status: tittle, maskType: MaskType.Black);
            });
        }

        public void ShowToast(string tittle = "Carregando...")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.ShowToast(Forms.Context, status: tittle, maskType: MaskType.Clear, TimeSpan.FromSeconds(1), false);
            });
        }
    }
}