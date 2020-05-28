using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WeTransport.Models;
using WeTransport.Helpers;
using Xamarin.Forms;
using System.Collections.Generic;
using Acr.UserDialogs;

namespace WeTransport.ViewModels.Pedido
{
    public class PedidoViewModel : BaseViewModel
    {
        public ObservableCollection<vwPedido> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool isChekin = false;
        private bool isArquivados = false;

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

        public PedidoViewModel(bool _chekin = false, bool _arquivados = false)
        {
            Title = "Pedidos";
            Items = new ObservableCollection<vwPedido>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            isChekin = _chekin;
            isArquivados = _arquivados;
        }


        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            //UserDialogs.Instance.Loading("Carregando...");

            try
            {
                Items.Clear();
                List<PedidoModel> items;

                if(Settings.isService)
                    items = await PedidoHelper.GetAllPedidoByPrestador();
                else
                    items = await PedidoHelper.GetAllPedidoByUser();

                foreach (var item in items)
                {
                    if (!isArquivados && isChekin && (item.STATUS != 2 && item.STATUS != 3))
                        continue;

                    if (isArquivados)
                    {
                        if (Settings.isUser && !item.INATIVO_USER)
                            continue;

                        if (Settings.isService && !item.INATIVO_PRESTADOR)
                            continue;
                    }
                    else
                    {
                        if (Settings.isUser && item.INATIVO_USER)
                            continue;

                        if (Settings.isService && item.INATIVO_PRESTADOR)
                            continue;
                    }

                    vwPedido vwItem = new vwPedido();

                    if(Settings.isService)
                        vwItem.Pessoa = UsuarioHelper.ShowPerson(item.COD_USUARIO);
                    else
                        vwItem.Pessoa = UsuarioHelper.ShowPerson(item.COD_PRESTADOR);

                    vwItem.ID = item.ID;
                    vwItem.COD_FRETE = item.COD_FRETE;
                    vwItem.COD_USUARIO = item.COD_USUARIO;
                    vwItem.COD_PRESTADOR = item.COD_PRESTADOR;
                    vwItem.STATUS = item.STATUS;
                    vwItem.DSC_COD_FRETE = string.Format("Frete: #{0}", item.COD_FRETE.ToString().Split('-')[0].ToUpper());
                    vwItem.DESCRICAO = item.DESCRICAO;
                    vwItem.OBS = item.OBS;
                    vwItem.LOCAL_DESTINO = item.LOCAL_DESTINO;
                    vwItem.DATA_ENVIO = item.DATA_ENVIO;
                    vwItem.DATA_SOLICITACAO = item.DATA_SOLICITACAO;

                    switch (item.STATUS)
                    {
                        case 0:
                            vwItem.DSC_STATUS = "Em Aberto";
                            vwItem.DSC_COR = "Green";
                            break;
                        case 1:
                            vwItem.DSC_STATUS = "Reservado";
                            vwItem.DSC_COR = "Gray";
                            break;
                        case 2:
                            vwItem.DSC_STATUS = "A Caminho";
                            vwItem.DSC_COR = "Blue";
                            break;
                        case 3:
                            vwItem.DSC_STATUS = "Saio para Entrega";
                            vwItem.DSC_COR = "Blue";
                            break;
                        case 4:
                            vwItem.DSC_STATUS = "Concluído";
                            vwItem.DSC_COR = "Black";
                            break;
                        case 5:
                            vwItem.DSC_STATUS = "Cancelado";
                            vwItem.DSC_COR = "Red";
                            break;
                        default:
                            vwItem.DSC_STATUS = "";
                            vwItem.DSC_COR = "Black";
                            break;
                    }

                    Items.Add(vwItem);
                }

                SetQtdTotalItens(Items.Count);
            }
            catch (Exception ex)
            {
                //UserDialogs.Instance.HideLoading();
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                //UserDialogs.Instance.HideLoading();
            }
        }

        internal void SetQtdTotalItens(int total)
        {
            QtdTotalItens = Settings.SetQtdTotalItens(total);
        }

    }
}