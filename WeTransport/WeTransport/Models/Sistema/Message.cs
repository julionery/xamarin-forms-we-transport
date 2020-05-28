using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WeTransport.Models.Sistema
{
    public class Message : INotifyPropertyChanged
    {
        private string text;
        private string messageDateTime;
        private bool isIncoming;
        private string contatoName;

        public string Text
        {
            get { return text; }
            set { text = value; RaisePropertyChanged(); }
        }

        public string MessagDateTime
        {
            get { return messageDateTime; }
            set { messageDateTime = value; RaisePropertyChanged(); }
        }

        public bool IsIncoming
        {
            get { return isIncoming; }
            set { isIncoming = value; RaisePropertyChanged(); }
        }

        public string ContatoName
        {
            get { return contatoName; }
            set { contatoName = value; RaisePropertyChanged(); }
        }

        private void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
