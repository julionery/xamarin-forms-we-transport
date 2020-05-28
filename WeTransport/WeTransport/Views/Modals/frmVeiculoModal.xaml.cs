using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Models;
using WeTransport.ViewModels.Veiculo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmVeiculoModal : PopupPage
    {

        #region + Variáveis Globais

        VeiculoViewModel viewModel;
        public EventHandler<VeiculoModel> _RegistroSelecionado;

        #endregion

        #region + Construtores

        public frmVeiculoModal(VeiculoModel _Registro)
        {
            InitializeComponent();
            BindingContext = viewModel = new VeiculoViewModel();
            CarregaDados(_Registro);
        }

        #endregion

        #region + Métodos

        private void CarregaDados(VeiculoModel _Registro)
        {
            if (_Registro != null && _Registro.NOME != null && _Registro.NOME.Trim() != "")
            {
                grpSelecionado.IsVisible = true;
                lblRegistroSelecionado.Text = _Registro.NOME;
            }
        }

        #endregion

        #region + Eventos

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = txtPesquisa.Text;
            var items = viewModel.Items.Where(x => x.NOME.ToLower().Contains(texto.ToLower()));
            grdVeiculos.ItemsSource = items;
            viewModel.SetQtdTotalItens(items.Count());
        }

        private async void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null) return;

                var itemGrid = e.SelectedItem as VeiculoModel;
                if (itemGrid == null)
                    return;

                _RegistroSelecionado?.Invoke(this, itemGrid);
                await Navigation.PopPopupAsync();

                grdVeiculos.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
            finally { }
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

        #endregion

        #region + Overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion

    }
}