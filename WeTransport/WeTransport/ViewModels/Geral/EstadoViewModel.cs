
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;

namespace WeTransport.ViewModels.Geral
{
    public class EstadoViewModel : BaseViewModel
    {
        public ObservableCollection<EstadoModel> Items { get; set; }
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

        public EstadoViewModel()
        {
            Title = "Estados";
            Items = new ObservableCollection<EstadoModel>();
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

                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "AC", NOME = "Acre (AC)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "AL", NOME = "Alagoas (AL)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "AP", NOME = "Amapá (AP)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "AM", NOME = "Amazonas (AM)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "BA", NOME = "Bahia (BA)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "CE", NOME = "Ceará (CE)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "DF", NOME = "Distrito Federal (DF)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "ES", NOME = "Espírito Santo (ES)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "GO", NOME = "Goiás (GO)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "MA", NOME = "Maranhão (MA)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "MT", NOME = "Mato Grosso (MT)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "MS", NOME = "Mato Grosso do Sul (MS)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "MG", NOME = "Minas Gerais (MG)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "PA", NOME = "Pará (PA) " });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "PB", NOME = "Paraíba (PB)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "PR", NOME = "Paraná (PR)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "PE", NOME = "Pernambuco (PE)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "PI", NOME = "Piauí (PI)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "RJ", NOME = "Rio de Janeiro (RJ)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "RN", NOME = "Rio Grande do Norte (RN)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "RS", NOME = "Rio Grande do Sul (RS)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "RO", NOME = "Rondônia (RO)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "RR", NOME = "Roraima (RR)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "SC", NOME = "Santa Catarina (SC)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "SP", NOME = "São Paulo (SP)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "SE", NOME = "Sergipe (SE)" });
                Items.Add(new EstadoModel { ID = Guid.NewGuid(), UF = "TO", NOME = "Tocantins (TO)" });

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

        internal void SetQtdTotalItens(int total)
        {
            QtdTotalItens = Settings.SetQtdTotalItens(total);
        }
    }
}