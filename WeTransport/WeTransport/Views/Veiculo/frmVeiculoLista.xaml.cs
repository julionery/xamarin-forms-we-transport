using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.ViewModels.Veiculo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Veiculo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmVeiculoLista : ContentPage
    {

        #region + Variáveis Globais

        VeiculoViewModel viewModel;

        #endregion

        #region + Construtores

        public frmVeiculoLista()
        {
            InitializeComponent();
            BindingContext = viewModel = new VeiculoViewModel();
        }

        #endregion

        #region + Eventos

        private void BtnNovo_Clicked(object sender, EventArgs e)
        {
            try
            {
                frmVeiculoCad frm = new frmVeiculoCad();
                frm.Disappearing += frmVeiculoCad_Disappearing;
                Navigation.PushAsync(frm, true);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
            finally { }
        }

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtPesquisa.Text;
            var items = viewModel.Items.Where(x => x.NOME.ToLower().Contains(texto.ToLower()));
            grdVeiculos.ItemsSource = items;
            viewModel.SetQtdTotalItens(items.Count());
        }

        private void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;

                var itemGrid = e.SelectedItem as VeiculoModel;
                if (itemGrid == null)
                    return;

                frmVeiculoCad frm = new frmVeiculoCad(itemGrid);
                frm.Disappearing += frmVeiculoCad_Disappearing;
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

        private void frmVeiculoCad_Disappearing(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                VeiculoModel itemGrid = mi.CommandParameter as VeiculoModel;

                var answer = await DisplayAlert("Deletar? ", "Deseja realmente deletar: " + itemGrid.NOME, "Sim", "Não");
                ToastProgress.Show();

                if (answer)
                {
                    await VeiculoHelper.DeleteVeiculo(itemGrid.ID);
                    viewModel.LoadItemsCommand.Execute(null);
                    UserDialogs.Instance.Toast("Veículo deletado com sucesso!", TimeSpan.FromSeconds(3));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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