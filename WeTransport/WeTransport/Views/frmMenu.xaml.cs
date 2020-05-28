using WeTransport.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeTransport.Components;
using WeTransport.Helpers;

namespace WeTransport.Views.Start
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmMenu : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<MenuItemModel> menuItems;
        public frmMenu()
        {
            InitializeComponent();

            switch (Settings.UserType)
            {
                case "user":
                    menuItems = new List<MenuItemModel>
                    {
                        new MenuItemModel {Id = MenuItemType.Frete, Title="Fretes" },
                        new MenuItemModel {Id = MenuItemType.Pedido, Title="Pedidos" },
                        new MenuItemModel {Id = MenuItemType.Notificacao, Title="Mensagens" },
                        new MenuItemModel {Id = MenuItemType.Perfil, Title="Perfil" }
                        //new MenuItemModel {Id = MenuItemType.Sobre, Title="Sobre" }
                    };
                    Settings.isUser = true;
                    Settings.isService = false;
                    break;
                case "service-provider":
                    menuItems = new List<MenuItemModel>
                    {
                        new MenuItemModel {Id = MenuItemType.Frete, Title="Fretes" },
                        new MenuItemModel {Id = MenuItemType.Veiculo, Title="Veículos" },
                        new MenuItemModel {Id = MenuItemType.Notificacao, Title="Mensagens" },
                        new MenuItemModel {Id = MenuItemType.Pedido, Title="Solicitações" },
                        new MenuItemModel {Id = MenuItemType.Check, Title="Check-in" },
                        new MenuItemModel {Id = MenuItemType.Perfil, Title="Perfil" }
                        //new MenuItemModel {Id = MenuItemType.Sobre, Title="Sobre" }
                    };
                    Settings.isUser = false;
                    Settings.isService = true;
                    break;
                default:
                    menuItems = new List<MenuItemModel> {
                        new MenuItemModel {Id = MenuItemType.Sobre, Title="Sobre" }
                    };
                    break;
            }

            
            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((MenuItemModel)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);

            };

            if (Settings.UserName.Length > 0)
                lblUsuario.Text = Settings.UserName.ToUpper();
        }

        private async void BtnLogout_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool result = await DisplayAlert("SAIR", "Deseja realmente SAIR da aplicação?", "SIM", "NÃO");
                if (result)
                {
                    ToastProgress.Show();
                    Settings.UserToken = "";
                    Settings.UserType = "";
                    MessagingCenter.Send(EventArgs.Empty, "GoToLogin");
                    ToastProgress.Hide();
                }
            }
            catch (Exception ex)
            {
                ToastProgress.Hide();
                await DisplayAlert("Error", ex.Message + "\n\nTente novamente!", "OK");
            }
        }
    }
}