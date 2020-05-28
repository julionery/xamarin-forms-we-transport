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
using WeTransport.ViewModels.Frete;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Frete
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmFreteLista : ContentPage
	{
        #region + Variáveis Globais

        FreteViewModel viewModel;

        #endregion

        #region + Construtores

        public frmFreteLista ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new FreteViewModel();
            VerificaUser();
        }

        #endregion

        #region + Métodos
        
        private void VerificaUser()
        {
            if (Settings.isService)
            {
                btnNovo.IsVisible = true;
            }
            else
                btnNovo.IsVisible = false;
        }

        #endregion

        #region + Eventos

        private async void BtnNovo_Clicked(object sender, EventArgs e)
        {
            try
            {
                frmFreteCad frm = new frmFreteCad();
                frm.Disappearing += frmFreteCad_Disappearing;
                await Navigation.PushAsync(frm, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
            finally { }
        }


        private void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;

                ToastProgress.Show();

                if (Settings.isService)
                {
                    var itemEdit = e.SelectedItem as FreteModel;

                    if (itemEdit == null)
                        return;

                    frmFreteCad frm = new frmFreteCad(itemEdit);
                    frm.Disappearing += frmFreteCad_Disappearing;
                    Navigation.PushAsync(frm, true);
                }
                else
                {
                    var itemDetail = e.SelectedItem as vwFrete;

                    if (itemDetail == null)
                        return;

                    frmFreteDetail frm = new frmFreteDetail(itemDetail);
                    frm.Disappearing += frmFreteCad_Disappearing;
                    Navigation.PushAsync(frm, true);
                }

                grdFretes.SelectedItem = null;
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
                grdFretes.SelectedItem = null;
            }
            finally {
                ToastProgress.Hide();
            }
        }


        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtPesquisa.Text;
            var items = viewModel.Items.Where(x => x.DSC_TIPO.ToLower().Contains(texto.ToLower()));
            grdFretes.ItemsSource = items;
            viewModel.SetQtdTotalItens(items.Count());
        }

        private void frmFreteCad_Disappearing(object sender, EventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                FreteModel itemGrid = mi.CommandParameter as FreteModel;

                var answer = await DisplayAlert("Deletar? ", "Deseja realmente deletar: " + itemGrid.TIPO, "Sim", "Não");
                ToastProgress.Show();

                if (answer)
                {
                    await FreteHelper.DeleteFrete(itemGrid.ID);
                    viewModel.LoadItemsCommand.Execute(null);
                    UserDialogs.Instance.Toast("Frete deletado com sucesso!", TimeSpan.FromSeconds(3));
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