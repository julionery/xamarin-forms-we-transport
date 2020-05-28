using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.Models.Sistema;
using WeTransport.ViewModels.Notificacao;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Notificacao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmChat : ContentPage
    {
        #region + Variáveis Globais

        ChatViewModel ViewModel;

        #endregion

        #region + Construtores

        public frmChat(PessoaModel _pessoa, Guid cod_chat)
        {
            InitializeComponent();
            Title = _pessoa.NOME;
            BindingContext = ViewModel = new ChatViewModel(_pessoa, cod_chat);
            ViewModel.ListView = this.ListView;
            ListView.Loaded += ListView_Loaded;
        }

        #endregion

        #region + Eventos

        private void ListView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            (ListView.LayoutManager as LinearLayout).ScrollToRowIndex(ViewModel.Messages.Count - 1, true);
        }

        private async void btnAbrirMais_Clicked(object sender, EventArgs e)
        {
            ////To get the current first item which is visible in the View.
            //var firstItem = ListView.DataSource.DisplayItems[0];
            //ViewModel.GridIsVisible = false;
            //ViewModel.IndicatorIsVisible = true;
            //await Task.Delay(2000);
            //var r = new Random();

            ////To avoid layout calls for arranging each and every items to be added in the View. 
            //ListView.DataSource.BeginInit();
            //for (int i = 0; i < 5; i++)
            //{
            //    var collection = new Message();
            //    collection.Text = ViewModel.MessageText[r.Next(0, ViewModel.MessageText.Count() - 1)];
            //    collection.IsIncoming = i % 2 == 0 ? true : false;
            //    collection.MessagDateTime = DateTime.Now.ToString();
            //    ViewModel.Messages.Insert(0, collection);
            //}
            //ListView.DataSource.EndInit();
            //var firstItemIndex = ListView.DataSource.DisplayItems.IndexOf(firstItem);
            //var header = (ListView.HeaderTemplate != null && !ListView.IsStickyHeader) ? 1 : 0;
            //var totalItems = firstItemIndex + header;

            ////Need to scroll back to previous position else the ScrollViewer moves to top of the list.
            //ListView.LayoutManager.ScrollToRowIndex(totalItems, true);
            //ViewModel.GridIsVisible = true;
            //ViewModel.IndicatorIsVisible = false;
        }

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel.Messages.Count == 0)
                ViewModel.LoadItemsCommand.Execute(null);
        }

    }
}