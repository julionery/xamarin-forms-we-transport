
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WeTransport.Models;
using WeTransport.Helpers;
using Xamarin.Forms;

namespace WeTransport.ViewModels.Notificacao
{
    public class NotificacaoViewModel : BaseViewModel
    {
        public ObservableCollection<vwChat> Items { get; set; }
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

        public NotificacaoViewModel()
        {
            Title = "Mensagens";
            Items = new ObservableCollection<vwChat>();
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
                var items = await ChatHelper.GetAllChat();
                foreach (var item in items)
                {
                    vwChat vwItem = new vwChat();
                    vwItem.Pessoa = UsuarioHelper.ShowPerson(Settings.isService ? item.COD_USUARIO : item.COD_PRESTADOR);
                    vwItem.ID = item.ID;
                    vwItem.COD_FRETE = item.COD_FRETE;
                    vwItem.COD_USUARIO = item.COD_USUARIO;
                    vwItem.COD_PRESTADOR = item.COD_PRESTADOR;
                    vwItem.DSC_COD_FRETE = string.Format("Frete: #{0}", item.COD_FRETE.ToString().Split('-')[0].ToUpper());

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

        internal void SetQtdTotalItens(int total)
        {
            QtdTotalItens = Settings.SetQtdTotalItens(total);
        }

    }
}