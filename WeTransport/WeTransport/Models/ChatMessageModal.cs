using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public class ChatMessageModal
    {
        private Guid id;
        private Guid cod_chat;
        private string cod_usuario;
        private string mensagem;
        private DateTime data_envio;

        public Guid ID { get => id; set => id = value; }
        public Guid COD_CHAT { get => cod_chat; set => cod_chat = value; }
        public string COD_USUARIO { get => cod_usuario; set => cod_usuario = value; }
        public string MENSAGEM { get => mensagem; set => mensagem = value; }
        public DateTime DATA_ENVIO { get => data_envio; set => data_envio = value; }
    }
}
