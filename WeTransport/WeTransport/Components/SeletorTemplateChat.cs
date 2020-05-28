using System;
using System.Collections.Generic;
using System.Text;
using WeTransport.Models.Sistema;
using Xamarin.Forms;

namespace WeTransport.Components
{
    public class SeletorTemplateChat : DataTemplateSelector
    {
        private readonly DataTemplate incomingDataTemplate;
        private readonly DataTemplate outgoingDataTemplate;

        public SeletorTemplateChat()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(Views.Templates.Chat.MyChat));
            this.outgoingDataTemplate = new DataTemplate(typeof(Views.Templates.Chat.YourChat));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;

            if (messageVm == null)
                return null;

            return messageVm.IsIncoming ? this.incomingDataTemplate : this.outgoingDataTemplate;
        }
    }
}
