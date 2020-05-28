using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.ViewModels.Pedido;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmPedidosArquivados : PopupPage
    {
        #region + Variáveis Globais

        PedidoViewModel viewModel;

        #endregion

        #region + Construtores

        public frmPedidosArquivados()
        {
            InitializeComponent();
            BindingContext = viewModel = new PedidoViewModel(true, true);
        }

        #endregion

        #region + Eventos

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

        private async void GrdPedidosArquivados_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;
                var answer = await DisplayAlert("Arquivar? ", "Deseja RESTAURAR este item?", "Sim", "Não");

                if (answer)
                {
                    ToastProgress.Show();

                    var itemGrid = e.SelectedItem as PedidoModel;
                    if (itemGrid == null)
                        return;

                    if (Settings.isUser)
                    {
                        itemGrid.INATIVO_USER = false;
                    }

                    if (Settings.isService)
                    {
                        itemGrid.INATIVO_PRESTADOR = false;
                    }

                    await PedidoHelper.UpdatePedido(itemGrid);
                    await Navigation.PopPopupAsync();
                }
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
                grdPedidosArquivados.SelectedItem = null;
            }
            finally
            {
                grdPedidosArquivados.SelectedItem = null;
                ToastProgress.Hide();
            }
        }

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtPesquisa.Text;
            var items = viewModel.Items.Where(x => x.Pessoa.NOME.ToLower().Contains(texto.ToLower()));
            grdPedidosArquivados.ItemsSource = items;
            viewModel.SetQtdTotalItens(items.Count());
        }

        #endregion

        #region + Overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        #endregion

    }
}