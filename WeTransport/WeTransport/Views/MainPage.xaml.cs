using WeTransport.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Frete, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Check:
                        MenuPages.Add(id, new NavigationPage(new Check.frmCheckLista()));
                        break;
                    case (int)MenuItemType.Frete:
                        MenuPages.Add(id, new NavigationPage(new Frete.frmFreteLista()));
                        break;
                    case (int)MenuItemType.Notificacao:
                        MenuPages.Add(id, new NavigationPage(new Notificacao.frmNotificacaoLista()));
                        break;
                    case (int)MenuItemType.Pedido:
                        MenuPages.Add(id, new NavigationPage(new Pedido.frmPedidoLista()));
                        break;
                    case (int)MenuItemType.Perfil:
                        MenuPages.Add(id, new NavigationPage(new Perfil.frmPerfil()));
                        break;
                    case (int)MenuItemType.Sobre:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.Veiculo:
                        MenuPages.Add(id, new NavigationPage(new Veiculo.frmVeiculoLista()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}