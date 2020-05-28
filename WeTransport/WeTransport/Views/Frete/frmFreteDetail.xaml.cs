using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.Views.Modals;
using WeTransport.Views.Notificacao;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Frete
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmFreteDetail : ContentPage
    {
        #region + Variáveis Globais

        public vwFrete Item { get; set; }
        public PessoaModel pessoa { get; set; }
         
        #endregion

        #region + Construtores

        public frmFreteDetail(vwFrete registro)
        {
            InitializeComponent();
            Item = registro;
            Title = "Detalhes do Frete";
            BindingContext = this;
            CarregaCombos();
        }

        #endregion

        #region + Métodos

        private void CarregaCombos()
        {
            lblCode.Text = string.Format("Code: #{0}", Item.ID.ToString().Split('-')[0].ToUpper());

            #region + Geral

            lblLocalPartida.Text = string.Format("De: {0} / {1}", Item.CIDADE_PARTIDA, Item.ESTADO_PARTIDA.UF);
            lblDisponibilidade.Text = string.Format("Data da Coleta: {0}.", Item.DSC_DISPONIBILIDADE);
            lblValor.Text = string.Format("Valor: {0}", Item.DSC_VALOR);

            if (Item.CIDADE_DESTINO != null && Item.CIDADE_DESTINO.Trim() != "")
            {
                lblLocalDestino.IsVisible = true;
                lblLocalDestino.Text = string.Format("Para: {0} / {1}", Item.CIDADE_DESTINO, Item.ESTADO_DESTINO.UF);
            }

            #endregion

            #region + Veículo

            if (Item.VEICULO.MODELO != null && Item.VEICULO.MODELO.Trim() != "")
                lblModelo.Text = string.Format("Modelo: {0}", Item.VEICULO.MODELO);
            else
                lblModelo.IsVisible = false;

            if (Item.VEICULO.COR != null && Item.VEICULO.COR.Trim() != "")
                lblCor.Text = string.Format("Cor: {0}", Item.VEICULO.COR);
            else
                lblCor.IsVisible = false;

            if (Item.VEICULO.DESCRICAO == null || Item.VEICULO.DESCRICAO.Trim() == "")
                lblDescricao.IsVisible = false;
            else
                lblDescricao.Text = string.Format("Descrição: {0}", Item.VEICULO.DESCRICAO);

            if (Item.VEICULO.OBS == null || Item.VEICULO.OBS.Trim() == "")
                lblObs.IsVisible = false;
            else
                lblObs.Text = string.Format("Obs.: {0}", Item.VEICULO.OBS);

            #endregion

            #region + Usuario

            pessoa = UsuarioHelper.ShowPerson(Item.COD_USUARIO);

            lblNome.Text = string.Format("Nome: {0}", pessoa.NOME);
            lblEmail.Text = string.Format("E-mail: {0}", pessoa.EMAIL);
            lblTelefone.Text = string.Format("Telefone: {0}", pessoa.TELEFONE);

            #endregion
        }

        #endregion

        private async void BtnSolicitarReserva_Clicked(object sender, EventArgs e)
        {
                PedidoModel pedidoFrete = PedidoHelper.ShowPedidoByFrete(Item.ID);

                if(pedidoFrete == null)
                {
                    frmSolicitarReserva frm = new frmSolicitarReserva(Item, pessoa.COD_USUARIO);
                    await Navigation.PushPopupAsync(frm);
                }
                else
                {
                    UserDialogs.Instance.Toast("Solicitação já cadastrada!", TimeSpan.FromSeconds(3));
                }
        }

        private async void BtnEntrarEmContato_Clicked(object sender, EventArgs e)
        {
            try
            {
                var chatFrete = ChatHelper.ShowChatByFrete(Item.ID);
                if(chatFrete == null)
                {
                    await ChatHelper.AddChat(new ChatModal { COD_FRETE = Item.ID, COD_PRESTADOR = pessoa.COD_USUARIO });
                    chatFrete = ChatHelper.ShowChatByFrete(Item.ID);
                }

                frmChat frm = new frmChat(pessoa, chatFrete.ID);
                await Navigation.PushAsync(frm, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
            finally { }
        }
    }
}