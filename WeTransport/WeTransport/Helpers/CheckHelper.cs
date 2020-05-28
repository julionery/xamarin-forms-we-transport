using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeTransport.Models;

namespace WeTransport.Helpers
{
    public class CheckHelper
    {
        static FirebaseClient firebase = new FirebaseClient(Config.URL_DATABASE);

        public static async Task AddCheckIn(CheckModal check)
        {
            check.ID = Guid.NewGuid();
            check.DAT_INCLUSAO = DateTime.Now;

            await firebase
              .Child(TabelasFirebase.CHECK)
              .PostAsync(check);
        }

        public static CheckModal ShowCheckIn(Guid id)
        {
            var theCheckIn = ( firebase
              .Child(TabelasFirebase.CHECK)
              .OnceAsync<CheckModal>().GetAwaiter().GetResult())
              .Where(a => a.Object.ID == id)
              .FirstOrDefault().Object;

            return theCheckIn;
        }

        public static CheckModal ShowCheckInByPedido(Guid cod_pedido)
        {
            var theCheckIn = (firebase
              .Child(TabelasFirebase.CHECK)
              .OnceAsync<CheckModal>().GetAwaiter().GetResult())
              .Where(a => a.Object.COD_PEDIDO == cod_pedido)
              .OrderByDescending(x => x.Object.DAT_INCLUSAO)
              .FirstOrDefault();

            if (theCheckIn == null)
                return null;
            else
                return theCheckIn.Object;
        }

    }
}
