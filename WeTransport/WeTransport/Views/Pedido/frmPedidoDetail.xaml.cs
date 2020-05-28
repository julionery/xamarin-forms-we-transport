using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Pedido
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmPedidoDetail : ContentPage
    {

        #region + Variáveis Globais

        public vwPedido Item { get; set; }
        public EventHandler ListarTodos;

        #endregion

        #region + Construtores

        public frmPedidoDetail(vwPedido registro)
        {
            InitializeComponent();
            Item = registro;
            Title = "Solicitação de Frete";
            CarregaDados();
            BindingContext = this;
        }

        #endregion

        #region + Métodos

        public void CarregaDados()
        {
            lblContato.Text = string.Format("Contato: {0}", Item.Pessoa.NOME);

            if (Settings.isService)
            {
                grdStatus.IsVisible = true;
                btnSalvar.Icon = "ic_save.png";
            }
            else
            {
                grdStatus.IsVisible = false;
                btnSalvar.Icon = "ic_archive.png";
            }

            #region + Usuario

            PessoaModel pessoa;

            if (Settings.isService)
            {
                pessoa = UsuarioHelper.ShowPerson(Item.COD_USUARIO);
                lblUsuario.Text = "CONTATO";
            }
            else
            {
                pessoa = UsuarioHelper.ShowPerson(Item.COD_PRESTADOR);
                lblUsuario.Text = "PRESTADOR DE SERVIÇO";
            }

            lblNome.Text = string.Format("Nome: {0}", pessoa.NOME);
            lblEmail.Text = string.Format("E-mail: {0}", pessoa.EMAIL);
            lblTelefone.Text = string.Format("Telefone: {0}", pessoa.TELEFONE);

            #endregion
        }

        #endregion


        #region + Eventos

        private async void BtnSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                ToastProgress.Show();

                if (Settings.isService)
                {
                    string _dscStatus = "";
                    switch (Item.STATUS)
                    {
                        case 0:
                            _dscStatus = "Em Aberto";
                            break;
                        case 1:
                            _dscStatus = "Reservado";
                            break;
                        case 2:
                            _dscStatus = "A Caminho";
                            break;
                        case 3:
                            _dscStatus = "Saio para Entrega";
                            break;
                        case 4:
                            _dscStatus = "Concluído";
                            break;
                        case 5:
                            _dscStatus = "Cancelado";
                            break;
                        default:
                            _dscStatus = "";
                            break;
                    }

                    await PedidoHelper.UpdatePedido(Item);
                    UserDialogs.Instance.Toast(string.Format("Solicitação alterada para: '{0}'", _dscStatus), TimeSpan.FromSeconds(3));
                }
                else
                {
                    Item.INATIVO_USER = true;
                    await PedidoHelper.UpdatePedido(Item);
                    UserDialogs.Instance.Toast("Arquivado com Sucesso!", TimeSpan.FromSeconds(3));
                }

                ListarTodos?.Invoke(this, EventArgs.Empty);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
            finally
            {
                ToastProgress.Hide();
            }
        }

        #endregion

    }
}