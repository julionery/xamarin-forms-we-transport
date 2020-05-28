using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Start
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmSelectCadastro : ContentPage
	{
		public frmSelectCadastro ()
		{
			InitializeComponent ();
		}

        private async void BtnUsuario_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Start.frmCadastroUsuario());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
        }

        private async void BtnPrestadorDeServico_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Start.frmCadastroPrestadorDeServico());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
        }
    }
}