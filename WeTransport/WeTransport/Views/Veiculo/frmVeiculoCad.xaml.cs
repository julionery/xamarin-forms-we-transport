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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Veiculo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmVeiculoCad : ContentPage
    {
        #region + Variáveis Globais

        public VeiculoModel Item { get; set; }
        private bool editando = false;

        #endregion

        #region + Construtores

        public frmVeiculoCad()
        {
            InitializeComponent();
            Item = new VeiculoModel();
            Title = "Novo Veículo";
            this.editando = false;
            BindingContext = this;
        }

        public frmVeiculoCad(VeiculoModel registro)
        {
            InitializeComponent();
            Item = registro;
            Title = "Editando Veículo";
            this.editando = true;
            BindingContext = this;
            lblCode.IsVisible = true;
            lblCode.Text = string.Format("Code: #{0}", Item.ID.ToString().Split('-')[0].ToUpper());
            AddButons();
        }

        #endregion

        #region + Métodos

        private bool ValidaDados()
        {
            if (Item.NOME == null || Item.NOME.Trim().Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Nome!", "OK");
                txtNome.Focus();
                return false;
            }

            if (Item.PLACA == null || Item.PLACA.Trim().Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Placa do Veículo!", "OK");
                txtPlaca.Focus();
                return false;
            }

            if (Item.MODELO == null || Item.MODELO.Trim().Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar o Modelo do Veículo!", "OK");
                txtModelo.Focus();
                return false;
            }

            if (Item.COR == null || Item.COR.Trim().Length == 0)
            {
                DisplayAlert("Ooopss...", "Você precisa informar a Cor do Veículo!", "OK");
                txtCor.Focus();
                return false;
            }

            return true;
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

        async void BtnSalvar_Clicked(object sender, EventArgs e)
        {
            if (!ValidaDados())
                return;

            try
            {
                ToastProgress.Show();

                if (!editando)
                {
                    await VeiculoHelper.AddVeiculo(Item);
                    UserDialogs.Instance.Toast("Veículo cadastrado com sucesso!", TimeSpan.FromSeconds(3));
                }
                else
                {
                    await VeiculoHelper.UpdateVeiculo(Item);
                    UserDialogs.Instance.Toast("Veículo atualizado com sucesso!", TimeSpan.FromSeconds(3));
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

        private async void Deletar_Clicked()
        {
            try
            {
                var answer = await DisplayAlert("Deletar? ", "Deseja realmente deletar este item?", "Sim", "Não");
                ToastProgress.Show();

                if (answer)
                {
                    await VeiculoHelper.DeleteVeiculo(Item.ID);
                    UserDialogs.Instance.Toast("Veículo deletado com sucesso!", TimeSpan.FromSeconds(3));
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