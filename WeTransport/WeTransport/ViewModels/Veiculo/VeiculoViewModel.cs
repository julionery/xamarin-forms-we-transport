
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;

namespace WeTransport.ViewModels.Veiculo
{
    public class VeiculoViewModel : BaseViewModel
    {
        public ObservableCollection<vwVeiculo> Items { get; set; }
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

        public VeiculoViewModel()
        {
            Title = "Veículos";
            Items = new ObservableCollection<vwVeiculo>();
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
                var items = await VeiculoHelper.GetAllVeiculos();
                foreach (var item in items)
                {
                    vwVeiculo vwItem = parseVW(item);
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

        private vwVeiculo parseVW(vwVeiculo item)
        {
            switch (item.TIPO)
            {
                case 0:
                    item.DSC_TIPO = "Moto/Motocicleta";
                    break;
                case 1:
                    item.DSC_TIPO = "Carro/Automóvel";
                    break;
                case 2:
                    item.DSC_TIPO = "Ônibus/Microônibus";
                    break;
                case 3:
                    item.DSC_TIPO = "Caminhão";
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