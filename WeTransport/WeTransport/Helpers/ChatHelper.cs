using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeTransport.Models;

namespace WeTransport.Helpers
{
    public class ChatHelper
    {
        static FirebaseClient firebase = new FirebaseClient(Config.URL_DATABASE);

        public static async Task<List<ChatModal>> GetAllChat()
        {
            return (await firebase
              .Child(TabelasFirebase.CHAT)
              .OnceAsync<ChatModal>())
              .Where(a => Settings.isService ? a.Object.COD_PRESTADOR == Settings.UserKey : a.Object.COD_USUARIO == Settings.UserKey)
              .Select(item => new ChatModal
              {
                  ID = item.Object.ID,
                  COD_FRETE = item.Object.COD_FRETE,
                  COD_USUARIO = item.Object.COD_USUARIO,
                  COD_PRESTADOR = item.Object.COD_PRESTADOR
              }).ToList();
        }

        public static async Task<List<ChatMessageModal>> GetAllChatMessage(Guid cod_chat)
        {
            return (await firebase
              .Child(TabelasFirebase.CHAT_MESSAGE)
              .OnceAsync<ChatMessageModal>())
              .Where(a => a.Object.COD_CHAT == cod_chat)
              .Select(item => new ChatMessageModal
                  {
                      ID = item.Object.ID,
                      COD_USUARIO = item.Object.COD_USUARIO,
                      MENSAGEM = item.Object.MENSAGEM,
                      DATA_ENVIO = item.Object.DATA_ENVIO
                  })
              .OrderBy(x => x.DATA_ENVIO)
              .ToList();
        }
        
        public static async Task AddChat(ChatModal chat)
        {
            chat.ID = Guid.NewGuid();
            chat.COD_USUARIO = Settings.UserKey;

            await firebase
              .Child(TabelasFirebase.CHAT)
              .PostAsync(chat);
        }

        public static async Task AddChatMessage(Guid cod_chat, string message)
        {
            ChatMessageModal chatMessage = new ChatMessageModal();
            chatMessage.ID = Guid.NewGuid();
            chatMessage.COD_CHAT = cod_chat;
            chatMessage.COD_USUARIO = Settings.UserKey;
            chatMessage.MENSAGEM = message;
            chatMessage.DATA_ENVIO = DateTime.Now;

            await firebase
              .Child(TabelasFirebase.CHAT_MESSAGE)
              .PostAsync(chatMessage);
        }

        public static ChatModal ShowChat(Guid id)
        {
            var theChat = ( firebase
              .Child(TabelasFirebase.CHAT)
              .OnceAsync<ChatModal>().GetAwaiter().GetResult())
              .Where(a => a.Object.ID == id).FirstOrDefault().Object;

            return theChat;
        }

        public static ChatModal ShowChatByFrete(Guid cod_frete)
        {
            var theChat = (firebase
              .Child(TabelasFirebase.CHAT)
              .OnceAsync<ChatModal>().GetAwaiter().GetResult())
              .Where(a => a.Object.COD_FRETE == cod_frete)
              .Where(x => x.Object.COD_USUARIO == Settings.UserKey)
              .FirstOrDefault();

            if (theChat == null)
                return null;
            else
                return theChat.Object;
        }

    }
}
