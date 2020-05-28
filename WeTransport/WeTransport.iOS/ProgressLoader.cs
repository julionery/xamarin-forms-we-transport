using BigTed;
using WeTransport.Components.Interfaces;
using WeTransport.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProgressLoader))]
namespace WeTransport.iOS
{
    public class ProgressLoader : IProgressInterface
    {
        public ProgressLoader()
        {
        }

        public void Hide()
        {
            BTProgressHUD.Dismiss();
        }

        public void Show(string tittle = "Carregando...")
        {
            BTProgressHUD.Show(tittle, maskType: ProgressHUD.MaskType.Black);
        }

        public void ShowToast(string tittle = "Carregando...")
        {
            BTProgressHUD.ShowToast(tittle, maskType: ProgressHUD.MaskType.Clear);
        }
    }
}