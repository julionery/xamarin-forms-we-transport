using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Perfil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmEditUsuario : ContentPage
	{

        #region + Variáveis Globais

        UsuarioHelper firebaseHelper = new UsuarioHelper();
        PessoaModel Registro;

        #endregion

        #region + Construtores

        public frmEditUsuario()
		{
			InitializeComponent();
            Registro = UsuarioHelper.ShowPerson(Settings.UserKey);
            CarregaDados();
        }

        #endregion

        #region + Métodos

        private void CarregaDados()
        {
            Title = Registro.NOME;
            txtNome.Text = Registro.NOME;
            txtEmail.Text = Registro.EMAIL;
            txtTelefone.Value = Registro.TELEFONE;
            txtCPF.Value = Registro.CPF;
        }

        private bool ValidaDados()
        {
            if (txtNome.Text == null || txtNome.Text.Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Nome!", "OK");
                txtNome.Focus();
                return false;
            }

            //if (txtEmail.Text == null || txtNome.Text.Length == 0)
            //{
            //    DisplayAlert("Ooopss...", "Você precisa informar o E-mail!", "OK");
            //    txtEmail.Focus();
            //    return false;
            //}

            if (txtTelefone.Value == null)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Telefone!", "OK");
                txtTelefone.Focus();
                return false;
            }

            //if (txtCPF.Value == null)
            //{
            //    DisplayAlert("Ooopss...", "Você precisa informar o CPF!", "OK");
            //    txtCPF.Focus();
            //    return false;
            //}

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
                Registro.NOME = txtNome.Text.Trim();
                Registro.TELEFONE = txtTelefone.Value.ToString();
                Settings.UserName = Registro.NOME;

                await firebaseHelper.UpdatePerson(Registro);
                await Navigation.PopAsync();
                ToastProgress.Hide();
                UserDialogs.Instance.Toast("Cadastro atualizado com sucesso!", TimeSpan.FromSeconds(3));
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