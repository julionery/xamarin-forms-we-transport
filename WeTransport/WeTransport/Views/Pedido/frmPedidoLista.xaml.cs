using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.ViewModels.Pedido;
using WeTransport.Views.Check;
using WeTransport.Views.Modals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Pedido
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmPedidoLista : ContentPage
    {

        #region + Variáveis Globais

        PedidoViewModel viewModel;

        #endregion

        #region + Construtores

        public frmPedidoLista()
        {
            InitializeComponent();
            BindingContext = viewModel = new PedidoViewModel();
        }

        #endregion

        #region + Métodos

        public void ListarTodos(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
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

        private void GrdVeiculos_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {

            try
            {
                if (e.ItemData == null) return;

                var itemGrid = e.ItemData as PedidoModel;
                if (itemGrid == null)
                    return;

                if (Settings.isService)
                {
                    var itemGrid2 = e.ItemData as vwPedido;
                    frmPedidoDetail frm = new frmPedidoDetail(itemGrid2);
                    frm.Disappearing += frmPedido_Disappearing;
                    frm.ListarTodos += ListarTodos;
                    Navigation.PushAsync(frm, true);
                }
                else
                {
                    switch (itemGrid.STATUS)
                    {
                        case 0:
                        case 1:
                            frmSolicitarReserva frm = new frmSolicitarReserva(itemGrid);
                            frm.Disappearing += frmPedido_Disappearing;
                            frm.ListarTodos += ListarTodos;
                            Navigation.PushPopupAsync(frm, true);
                            break;
                        case 2:
                        case 3:
                            frmCheckMap frmMapa = new frmCheckMap(itemGrid);
                            frmMapa.Disappearing += frmPedido_Disappearing;
                            Navigation.PushPopupAsync(frmMapa);
                            break;
                        case 4:
                        case 5:
                        default:
                            var itemGrid2 = e.ItemData as vwPedido;
                            frmPedidoDetail frmDetail = new frmPedidoDetail(itemGrid2);
                            frmDetail.Disappearing += frmPedido_Disappearing;
                            frmDetail.ListarTodos += ListarTodos;
                            Navigation.PushAsync(frmDetail);
                            break;
                    }
                }

                grdVeiculos.SelectedItems.Clear();
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
                grdVeiculos.SelectedItems.Clear();
            }
            finally { }
        }

        private void frmPedido_Disappearing(object sender, EventArgs e)
        {
            grdVeiculos.SelectedItems.Clear();
        }
        
        private async void PullToRefresh_Refreshing(object sender, EventArgs e)
        {
            pullToRefresh.IsRefreshing = true;
            await viewModel.ExecuteLoadItemsCommand();
            pullToRefresh.IsRefreshing = false;
        }

        private async void BtnExcluir_Tapped(object sender, EventArgs e)
        {
            try
            {
                var answer = await DisplayAlert("Arquivar? ", "Deseja realmente ARQUIVAR este item?", "Sim", "Não");

                if (answer)
                {
                    ToastProgress.Show();
                    var mi = (TappedEventArgs)e;
                    var item = mi.Parameter as PedidoModel;
                    if (item == null)
                        return;

                    if (Settings.isUser)
                    {
                        item.INATIVO_USER = true;
                    }

                    if (Settings.isService)
                    {
                        item.INATIVO_PRESTADOR = true;
                    }

                    await PedidoHelper.UpdatePedido(item);
                    await viewModel.ExecuteLoadItemsCommand();
                }

            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                MessagingCenter.Send<object, string>(this, "Error", ex.Message);
            }
            finally
            {
                ToastProgress.Hide();
            }

        }

        private void BtnArquives_Clicked(object sender, EventArgs e)
        {
            try
            {
                ToastProgress.Show();
                frmPedidosArquivados frm = new frmPedidosArquivados();
                frm.Disappearing += ListarTodos;
                Navigation.PushPopupAsync(frm, true);
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                MessagingCenter.Send<object, string>(this, "Error", ex.Message);
            }
            finally
            {
                ToastProgress.Hide();
            }
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