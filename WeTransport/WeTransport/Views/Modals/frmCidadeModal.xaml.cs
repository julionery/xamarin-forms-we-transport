using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Modals
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmCidadeModal : PopupPage
    {
		public frmCidadeModal ()
		{
			InitializeComponent ();
		}

        private async void btnFechar_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopPopupAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}