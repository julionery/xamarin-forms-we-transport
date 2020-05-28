using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Models;
using WeTransport.ViewModels.Notificacao;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Notificacao
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmNotificacaoLista : ContentPage
	{
        #region + Variáveis Globais

        NotificacaoViewModel viewModel;

        #endregion

        #region + Construtores

        public frmNotificacaoLista()
        {
            InitializeComponent();
            BindingContext = viewModel = new NotificacaoViewModel();
        }

        #endregion

        #region + Eventos

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtPesquisa.Text;
            var items = viewModel.Items.Where(x => x.Pessoa.NOME.ToLower().Contains(texto.ToLower()));
            grdVeiculos.ItemsSource = items;
            viewModel.SetQtdTotalItens(items.Count());
        }

        private void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;

                var itemGrid = e.SelectedItem as vwChat;
                if (itemGrid == null)
                    return;

                frmChat frm = new frmChat(itemGrid.Pessoa, itemGrid.ID);
                Navigation.PushAsync(frm, true);
                grdVeiculos.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
                grdVeiculos.SelectedItem = null;
            }
            finally { }
        }

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

    }
}