using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Start
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmLogin : ContentPage
    {
        #region + Variáveis Globais

        LoginViewModel viewModel;

        #endregion

        #region + Construtores

        public frmLogin()
        {
            InitializeComponent();
            BindingContext = viewModel = new LoginViewModel();
            LoadComponent();
            CarregaDados();
        }

        #endregion

        #region + Componentes

        private void LoadComponent()
        {
            txtUsuario.Completed += txtUsuario_Completed;
            txtSenha.Completed += async (s, e) => await txtSenha_Completed(s, e);
        }

        #endregion

        #region + Métodos

        private void txtUsuario_Completed(object sender, EventArgs e)
        {
            txtSenha.Focus();
        }

        private async Task txtSenha_Completed(object sender, EventArgs e)
        {
            btnLogin_Clicked(null, null);
        }

        private void CarregaDados()
        {
            if (Settings.UserEmail.Length > 0)
                txtUsuario.Text = Settings.UserEmail;
        }

        private bool ValidaDados()
        {
            if (txtUsuario.Text == null || txtUsuario.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Usuário!", "OK");
                return false;
            }

            if (txtSenha.Text == null || txtSenha.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Senha!", "OK");
                return false;
            }

            return true;
        }

        private async Task<bool> Login()
        {
            try
            {
                ToastProgress.Show();
                await viewModel.Login(txtUsuario.Text, txtSenha.Text);
                ToastProgress.Hide();
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                string error = Validations.ErrorValidator.ValidaErrosAuth(ex.Message);
                await DisplayAlert("Ooopss...", error + "\n\nTente novamente!", "OK");
                return false;
            }
            return true;
        }

        #endregion

        #region + Eventos

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (!ValidaDados())
                return;

            await Login();
        }

        private async void btnCadastro_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Start.frmSelectCadastro());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
        }

        #endregion

        #region + Overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion
      
    }
}