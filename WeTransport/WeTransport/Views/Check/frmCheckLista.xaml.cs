using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Models;
using WeTransport.ViewModels.Pedido;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Check
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmCheckLista : ContentPage
	{
        #region + Variáveis Globais

        PedidoViewModel viewModel;

        #endregion

        #region + Construtores

        public frmCheckLista ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new PedidoViewModel(true);
        }

        #endregion

        #region + Eventos

        private void frmPedido_Disappearing(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtPesquisa.Text;
            var items = viewModel.Items.Where(x => x.Pessoa.NOME.ToLower().Contains(texto.ToLower()));
            grdCheck.ItemsSource = items;
            viewModel.SetQtdTotalItens(items.Count());
        }

        private void GrdCheck_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;

                ToastProgress.Show();

                var itemGrid = e.SelectedItem as PedidoModel;
                if (itemGrid == null)
                    return;

                frmCheckMap frmMapa = new frmCheckMap(itemGrid);
                frmMapa.Disappearing += frmPedido_Disappearing;
                Navigation.PushPopupAsync(frmMapa);

                ToastProgress.Hide();
                grdCheck.SelectedItem = null;
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
                grdCheck.SelectedItem = null;
            }
            finally { }
        }
    }
}