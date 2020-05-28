using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Start
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmCadastroUsuario : ContentPage
	{

        #region + Variáveis Globais

        UsuarioHelper firebaseHelper = new UsuarioHelper();

        #endregion

        #region + Construtores

        public frmCadastroUsuario()
		{
			InitializeComponent ();
		}

        #endregion

        #region + Métodos

        private bool ValidaDados()
        {
            if (txtNome.Text == null || txtNome.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Nome!", "OK");
                txtNome.Focus();
                return false;
            }

            if (txtEmail.Text == null || txtNome.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o E-mail!", "OK");
                txtEmail.Focus();
                return false;
            }

            if (txtTelefone.Value == null)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Telefone!", "OK");
                txtTelefone.Focus();
                return false;
            }

            if (txtCPF.Value == null)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o CPF!", "OK");
                txtCPF.Focus();
                return false;
            }

            if (txtSenha.Text == null || txtSenha.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Senha!", "OK");
                txtSenha.Focus();
                return false;
            }

            if (txtSenha.Text.Length < 6)
            {
                DisplayAlert("Ooopss...", "A Senha deve conter no mínimo 6 caracteres!", "OK");
                txtConfirmaSenha.Focus();
                return false;
            }

            if (txtConfirmaSenha.Text == null || txtConfirmaSenha.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Confirmação de Senha!", "OK");
                txtConfirmaSenha.Focus();
                return false;
            }

            if (!txtSenha.Text.Equals(txtConfirmaSenha.Text))
            {
                DisplayAlert("Ooopss...", "A Confirmação de Senha deve ser igual a Senha!", "OK");
                txtConfirmaSenha.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region + Eventos

        private async void BtnSalvar_Clicked(object sender, EventArgs e)
        {
            if (!ValidaDados())
                return;

            try
            {
                ToastProgress.Show();
                string email = txtEmail.Text.Trim();
                await firebaseHelper.VerificaEmail(email);
                await firebaseHelper.AddUser(email, txtSenha.Text);
                await firebaseHelper.AddPerson(txtNome.Text.Trim(), email, txtTelefone.Value.ToString(), txtCPF.Value.ToString().Replace(',', '.'));
                LoginViewModel viewModel = new LoginViewModel();
                await viewModel.Login(email, txtSenha.Text);
                ToastProgress.Hide();
                UserDialogs.Instance.Toast("Cadastro realizado com sucesso!", TimeSpan.FromSeconds(3));
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                string error = Validations.ErrorValidator.ValidaErrosAuth(ex.Message);
                await DisplayAlert("Ooopss...", error + "\n\nTente novamente!", "OK");
            }
        }

        #endregion

    }
}