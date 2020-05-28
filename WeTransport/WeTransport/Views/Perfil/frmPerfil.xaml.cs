using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Perfil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmPerfil : ContentPage
    {
        public frmPerfil()
        {
            InitializeComponent();
            CarregaDados();
        }

        private void CarregaDados()
        {
            PessoaModel pessoa = UsuarioHelper.ShowPerson(Settings.UserKey);

            lblNome.Text = string.Format("Nome: {0}", pessoa.NOME);
            lblEmail.Text = string.Format("E-mail: {0}", pessoa.EMAIL);
            lblTelefone.Text = string.Format("Telefone: {0}", pessoa.TELEFONE);
            lblCPF.Text = string.Format("CPF: {0}", pessoa.CPF);

            if (Settings.isService)
            {
                lblCNH.IsVisible = true;
                lblCNH.Text = string.Format("Carteira de Motorista: {0}", pessoa.CNH);
            }
            else
            {
                lblCNH.IsVisible = false;
            }
        }

        private void BtnEditar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Settings.isUser)
                {
                    frmEditUsuario frm = new frmEditUsuario();
                    frm.Disappearing += frmPerfil_Disappearing;
                    Navigation.PushAsync(new frmEditUsuario());
                }

                if (Settings.isService)
                {
                    frmEditService frm = new frmEditService();
                    frm.Disappearing += frmPerfil_Disappearing;
                    Navigation.PushAsync(frm);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
        }

        private void frmPerfil_Disappearing(object sender, EventArgs e)
        {
            CarregaDados();
        }
    }
}