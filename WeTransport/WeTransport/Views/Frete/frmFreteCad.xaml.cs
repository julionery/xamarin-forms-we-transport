using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.Views.Modals;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Frete
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmFreteCad : ContentPage
    {

        #region + Variáveis Globais

        public FreteModel Item { get; set; }
        private bool editando = false;
        private VeiculoModel _Veiculo = new VeiculoModel();

        #endregion

        #region + Construtores

        public frmFreteCad()
        {
            InitializeComponent();
            Item = new FreteModel();
            Item.DISPONIBILIDADE_INICIAL = DateTime.Now;
            Item.DISPONIBILIDADE_FINAL = DateTime.Now;
            Title = "Novo Frete";
            this.editando = false;
            BindingContext = this;

        }

        public frmFreteCad(FreteModel registro)
        {
            InitializeComponent();
            Item = registro;
            Title = "Editando Frete";
            this.editando = true;
            BindingContext = this;
            CarregaCombos();
            AddButons();
        }

        #endregion

        #region + Métodos

        private void CarregaCombos()
        {
            if (Item.VEICULO != null)
            {
                //_Veiculo = VeiculoHelper.ShowVeiculo(Item.COD_VEICULO);
                _Veiculo = Item.VEICULO;
                cboVeiculo.Text = _Veiculo.NOME;
            }

            if (Item.CIDADE_PARTIDA != null && Item.CIDADE_PARTIDA.Trim() != "")
            {
                txtCidadePartida.Text = Item.CIDADE_PARTIDA;
            }

            if (Item.ESTADO_PARTIDA != null)
            {
                cboEstado.Text = Item.ESTADO_PARTIDA.NOME;
            }

            if (Item.CIDADE_DESTINO != null && Item.CIDADE_DESTINO.Trim() != "")
            {
                txtCidadeDestino.Text = Item.CIDADE_DESTINO;
            }

            if (Item.ESTADO_DESTINO != null)
            {
                cboEstadoDestino.Text = Item.ESTADO_DESTINO.NOME;
            }
        }

        private bool ValidaDados()
        {
            if (Item.VEICULO == null)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Veículo!", "OK");
                return false;
            }

            if (Item.CIDADE_PARTIDA == null)
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Cidade de Partida!", "OK");
                return false;
            }

            if (Item.ESTADO_PARTIDA == null)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Estado de Partida!", "OK");
                return false;
            }

            return true;
        }

        private void RegistroVeiculoSelecionado(object sender, VeiculoModel _RegistroSelecionado)
        {
            try
            {
                if (_RegistroSelecionado != null)
                {
                    _Veiculo = _RegistroSelecionado;
                    cboVeiculo.Text = _Veiculo.NOME;
                    Item.VEICULO = _Veiculo;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void EstadoPartidaSelecionado(object sender, EstadoModel _RegistroSelecionado)
        {

            try
            {
                if (_RegistroSelecionado != null)
                {
                    cboEstado.Text = _RegistroSelecionado.NOME;
                    Item.ESTADO_PARTIDA = _RegistroSelecionado;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void EstadoDestinoSelecionado(object sender, EstadoModel _RegistroSelecionado)
        {
            try
            {
                if (_RegistroSelecionado != null)
                {
                    cboEstadoDestino.Text = _RegistroSelecionado.NOME;
                    Item.ESTADO_DESTINO = _RegistroSelecionado;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void AddButons()
        {
            ToolbarItem btnDeletar = new ToolbarItem();
            btnDeletar.Icon = "ic_trash.png";
            btnDeletar.Clicked += delegate { Deletar_Clicked(); };
            ToolbarItems.Add(btnDeletar);
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

                if (!editando)
                {
                    await FreteHelper.AddFrete(Item);
                    UserDialogs.Instance.Toast("Frete cadastrado com sucesso!", TimeSpan.FromSeconds(3));
                }
                else
                {
                    await FreteHelper.UpdateFrete(Item);
                    UserDialogs.Instance.Toast("Frete atualizado com sucesso!", TimeSpan.FromSeconds(3));
                }

                await Navigation.PopAsync();
                ToastProgress.Hide();
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
        }

        private void CboDisponibilidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboDisponibilidade.SelectedIndex)
            {
                case 1:
                    lblDataInicial.Hint = "Data";
                    lblDataInicial.IsVisible = true;
                    lblDataFinal.IsVisible = false;
                    break;
                case 2:
                    lblDataInicial.Hint = "Data Inicial";
                    lblDataInicial.IsVisible = true;
                    lblDataFinal.IsVisible = true;
                    break;
                default:
                    lblDataInicial.IsVisible = false;
                    lblDataFinal.IsVisible = false;
                    break;
            }
        }

        private void CboVeiculo_Clicked(object sender, EventArgs e)
        {
            try
            {
                frmVeiculoModal frm = new frmVeiculoModal(_Veiculo);
                frm._RegistroSelecionado += RegistroVeiculoSelecionado;
                Navigation.PushPopupAsync(frm);
            }
            catch (Exception ex)
            {
                DisplayAlert("Oooops", ex.Message, "OK");
                Debug.WriteLine(ex.Message);
            }
        }

        private void CboEstado_Clicked(object sender, EventArgs e)
        {
            try
            {
                frmEstadoModal frm = new frmEstadoModal(Item.ESTADO_PARTIDA);
                frm._RegistroSelecionado += EstadoPartidaSelecionado;
                Navigation.PushPopupAsync(frm);
            }
            catch (Exception ex)
            {
                DisplayAlert("Oooops", ex.Message, "OK");
                Debug.WriteLine(ex.Message);
            }
        }

        private void CboEstadoDestino_Clicked(object sender, EventArgs e)
        {
            try
            {
                frmEstadoModal frm = new frmEstadoModal(Item.ESTADO_DESTINO);
                frm._RegistroSelecionado += EstadoDestinoSelecionado;
                Navigation.PushPopupAsync(frm);
            }
            catch (Exception ex)
            {
                DisplayAlert("Oooops", ex.Message, "OK");
                Debug.WriteLine(ex.Message);
            }
        }

        private async void Deletar_Clicked()
        {
            try
            {
                var answer = await DisplayAlert("Deletar? ", "Deseja realmente deletar este item?", "Sim", "Não");
                ToastProgress.Show();

                if (answer)
                {
                    await FreteHelper.DeleteFrete(Item.ID);
                    UserDialogs.Instance.Toast("Frete deletado com sucesso!", TimeSpan.FromSeconds(3));
                    await Navigation.PopAsync();
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

    }
}