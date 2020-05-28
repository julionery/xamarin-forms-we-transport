
using WeTransport.Components.Interfaces;
using Xamarin.Forms;

namespace WeTransport.Components
{
    public class ToastProgress
    {
        public static void Show(string title = "")
        {
            if (title == "")
                DependencyService.Get<IProgressInterface>().Show();
            else
                DependencyService.Get<IProgressInterface>().Show(title);
        }

        public static void ShowToast(string title = "")
        {
            if (title == "")
                DependencyService.Get<IProgressInterface>().ShowToast();
            else
                DependencyService.Get<IProgressInterface>().ShowToast(title);

        }
        public static void Hide()
        {
            DependencyService.Get<IProgressInterface>().Hide();
        }
    }
}
