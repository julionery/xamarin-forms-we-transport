
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;

namespace WeTransport.ViewModels.Frete
{
    public class FreteViewModel : BaseViewModel
    {
        public ObservableCollection<vwFrete> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        private string _qtdTotalItens;
        public string QtdTotalItens
        {
            get { return _qtdTotalItens; }
            set
            {
                _qtdTotalItens = value;
                OnPropertyChanged();
            }
        }

        public FreteViewModel()
        {
            Title = "WeTransport";
            Items = new ObservableCollection<vwFrete>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                List<vwFrete> items = new List<vwFrete>();
                
                if(Settings.isService)
                    items = await FreteHelper.GetAllFretesByUser();
                else
                    items = await FreteHelper.GetAllFretesAvailable();

                foreach (var item in items)
                {
                    vwFrete vwItem = parseVW(item);
                    Items.Add(vwItem);
                }
                SetQtdTotalItens(Items.Count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private vwFrete parseVW(vwFrete item)
        {

            item.DSC_COD_FRETE = string.Format("Frete: #{0}", item.ID.ToString().Split('-')[0].ToUpper());

            switch (item.DISPONIBILIDADE)
            {
                case 0:
                    item.DSC_DISPONIBILIDADE = "Imediata";
                    break;
                case 1:
                    item.DSC_DISPONIBILIDADE = string.Format("A partir de {0}.", item.DISPONIBILIDADE_INICIAL.ToString("dd/MM/yyyy"));
                    break;
                case 2:
                    item.DSC_DISPONIBILIDADE = string.Format("Entre {0} a {1}.", item.DISPONIBILIDADE_INICIAL.ToString("dd/MM/yyyy"), item.DISPONIBILIDADE_FINAL.ToString("dd/MM/yyyy"));
                    break;
            }

            if (Settings.isUser)
            {
                item.DSC_STATUS = "";
                item.DSC_COR = "Transparent";
            }
            else
            {
                switch (item.STATUS)
                {
                    case 0:
                        item.DSC_STATUS = "Em Aberto";
                        item.DSC_COR = "Green";
                        break;
                    case 1:
                        item.DSC_STATUS = "Reservado";
                        item.DSC_COR = "Gray";
                        break;
                    case 2:
                        item.DSC_STATUS = "Concluído";
                        item.DSC_COR = "Blue";
                        break;
                    case 3:
                        item.DSC_STATUS = "Cancelado";
                        item.DSC_COR = "Red";
                        break;
                    default:
                        item.DSC_STATUS = "";
                        item.DSC_COR = "Black";
                        break;
                }
            }

            switch (item.TIPO)
            {
                case 0:
                    item.DSC_TIPO = "CARGA GERAL";
                    break;
                case 1:
                    item.DSC_TIPO = "ENCOMENDAS";
                    break;
                case 2:
                    item.DSC_TIPO = "MUDANÇAS";
                    break;
                case 3:
                    item.DSC_TIPO = "CARGAS PERIGOSAS";
                    break;
                case 4:
                    item.DSC_TIPO = "CARGAS FRIGORÍFICAS";
                    break;
                case 5:
                    item.DSC_TIPO = "CARGAS DE PEQUENO PORTE";
                    break;
                case 6:
                    item.DSC_TIPO = "CARGAS DE GRANDE PORTE";
                    break;
                case 7:
                    item.DSC_TIPO = "PRODUTOS FARMACÊUTICOS";
                    break;
                case 8:
                    item.DSC_TIPO = "CARGA COMPLETA";
                    break;
                case 9:
                    item.DSC_TIPO = "CARGAS FRACIONADAS";
                    break;
                default:
                    item.DSC_TIPO = "";
                    break;
            }

            if (item.VALOR == 0)
                item.DSC_VALOR = "A combinar.";
            else
                item.DSC_VALOR = "R$"+ item.VALOR;

            if (item.OBS != null && item.OBS.Trim() != "")
                item.OBS = "Obs.:" + item.OBS;

            switch (item.VEICULO.TIPO)
            {
                case 0:
                    item.DSC_TIPO_VEICULO = "Moto/Motocicleta";
                    break;
                case 1:
                    item.DSC_TIPO_VEICULO = "Carro/Automóvel";
                    break;
                case 2:
                    item.DSC_TIPO_VEICULO = "Ônibus/Microônibus";
                    break;
                case 3:
                    item.DSC_TIPO_VEICULO = "Caminhão";
                    break;
            }

            return item;
        }

        internal void SetQtdTotalItens(int total)
        {
            QtdTotalItens = Settings.SetQtdTotalItens(total);
        }

    }
}