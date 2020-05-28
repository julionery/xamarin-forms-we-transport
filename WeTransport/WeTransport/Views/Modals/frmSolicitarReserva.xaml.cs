using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmSolicitarReserva : PopupPage
    {
        public EventHandler ListarTodos;
        public vwFrete Frete { get; set; }
        public PedidoModel Item { get; set; }
        public bool editando;

        public frmSolicitarReserva(vwFrete registro, string _cod_prestador)
        {
            InitializeComponent();
            Frete = registro;
            Item = new PedidoModel();
            Item.COD_PRESTADOR = _cod_prestador;
            Item.COD_FRETE = registro.ID;
            Item.STATUS = 0;
            Item.DATA_ENVIO = DateTime.Now;
            BindingContext = this;
            editando = false;
        }


        public frmSolicitarReserva(PedidoModel registro)
        {
            InitializeComponent();
            Item = registro;
            BindingContext = this;
            editando = true;

            if(Item.STATUS != 0)
            {
                txtDescricao.IsEnabled = false;
                txtLocalDestino.IsEnabled = false;
                txtDataEnvio.IsEnabled = false;
                txtObs.IsEnabled = false;
            }

        }

        private bool ValidaDados()
        {
            if (Item.DESCRICAO == null || Item.DESCRICAO.Trim() == "")
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Descrição!", "OK");
                txtDescricao.Focus();
                return false;
            }

            return true;
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

        private async void BtnSalvar_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!ValidaDados())
                    return;

                if (editando)
                {
                    await PedidoHelper.UpdatePedido(Item);
                    await Navigation.PopPopupAsync();
                    UserDialogs.Instance.Toast("Solicitação atualizada com sucesso!", TimeSpan.FromSeconds(3));
                }
                else
                {

                    bool result = await DisplayAlert("SOLICITAR RESERVA", "Deseja realmente\nSOLICITAR A RESERVA\ndeste frete?", "SIM", "NÃO");
                    if (result)
                    {
                        PedidoModel pedidoFrete = PedidoHelper.ShowPedidoByFrete(Item.ID);

                        if (pedidoFrete == null)
                        {
                            Item.DATA_SOLICITACAO = DateTime.Now;
                            await PedidoHelper.AddPedido(Item);
                            await Navigation.PopPopupAsync();
                            UserDialogs.Instance.Toast("Solicitação enviada com sucesso!", TimeSpan.FromSeconds(3));
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Solicitação já cadastrada!", TimeSpan.FromSeconds(3));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ListarTodos?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}