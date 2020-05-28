using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeTransport.Helpers;
using WeTransport.Models;
using WeTransport.Models.Sistema;
using Xamarin.Forms;

namespace WeTransport.ViewModels.Notificacao
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        #region + Variáveis Globais

        Guid CHAT_ID;
        PessoaModel PESSOA;
        public Command LoadItemsCommand { get; set; }
        private ObservableCollection<Message> messagesList;
        internal Syncfusion.ListView.XForms.SfListView ListView;
        private bool indicatorIsVisible;
        private bool gridIsVisible;
        private string newText;
        private ImageSource sendIcon;
        private string outgoingText;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SendCommand { get; set; }

        #endregion + Variáveis Globais

        #region + Propriedades

        public ObservableCollection<Message> Messages
        {
            get { return messagesList; }
            set { messagesList = value; }
        }

        public bool IndicatorIsVisible
        {
            get { return indicatorIsVisible; }
            set
            {
                indicatorIsVisible = value;
                OnPropertyChanged("IndicatorIsVisible");
            }
        }

        public bool GridIsVisible
        {
            get { return gridIsVisible; }
            set
            {
                gridIsVisible = value;
                OnPropertyChanged("GridIsVisible");
            }
        }

        public string NewText
        {
            get { return newText; }
            set
            {
                newText = value;
                OnPropertyChanged("NewText");
            }
        }

        public ImageSource SendIcon
        {
            get
            { return sendIcon; }
            set
            { sendIcon = value; }
        }

        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string OutGoingText
        {
            get { return outgoingText; }
            set { outgoingText = value; }
        }

        #endregion + Propriedades

        #region + Construtores

        public ChatViewModel(PessoaModel _pessoa, Guid _chat_id)
        {
            InitializeSendCommand();
            Messages = new ObservableCollection<Message>();
            CHAT_ID = _chat_id;
            PESSOA = _pessoa;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        #endregion + Construtores

        #region + Métodos

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                Messages.Clear();
                var items = await ChatHelper.GetAllChatMessage(CHAT_ID);
                foreach (var item in items)
                {
                    var collection = new Message();
                    collection.ContatoName = PESSOA.NOME;
                    collection.Text = item.MENSAGEM;

                    if(item.COD_USUARIO == Settings.UserKey)
                        collection.IsIncoming = true;
                    else
                        collection.IsIncoming = false;

                    collection.MessagDateTime = item.DATA_ENVIO.ToString("dd/MM/yyyy HH:mm:ss");

                    Messages.Add(collection);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                GridIsVisible = true;
            }
        }

        #endregion


        private void InitializeSendCommand()
        {
            try
            {
                NewText = "";
                SendCommand = new Command(() =>
                {
                    if (!string.IsNullOrWhiteSpace(NewText))
                    {
                        Messages.Add(new Message
                        {
                            Text = NewText,
                            IsIncoming = true,
                            MessagDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                        });
                        (ListView.LayoutManager as LinearLayout).ScrollToRowIndex(Messages.Count - 1, true);

                        ChatHelper.AddChatMessage(CHAT_ID, newText);
                    }
                    NewText = null;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
