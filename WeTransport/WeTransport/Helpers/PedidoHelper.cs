using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeTransport.Models;

namespace WeTransport.Helpers
{
    public class PedidoHelper
    {
        static FirebaseClient firebase = new FirebaseClient(Config.URL_DATABASE);

        public static async Task<List<PedidoModel>> GetAllPedidoByUser()
        {
            return (await firebase
              .Child(TabelasFirebase.PEDIDOS)
              .OnceAsync<PedidoModel>()).Where(a => a.Object.COD_USUARIO == Settings.UserKey).Select(item => new PedidoModel
              {
                  ID = item.Object.ID,
                  COD_FRETE = item.Object.COD_FRETE,
                  COD_USUARIO = item.Object.COD_USUARIO,
                  COD_PRESTADOR = item.Object.COD_PRESTADOR,
                  STATUS = item.Object.STATUS,
                  DESCRICAO = item.Object.DESCRICAO,
                  OBS = item.Object.OBS,
                  LOCAL_DESTINO = item.Object.LOCAL_DESTINO,
                  DATA_ENVIO = item.Object.DATA_ENVIO,
                  DATA_SOLICITACAO = item.Object.DATA_SOLICITACAO,
                  INATIVO_USER = item.Object.INATIVO_USER
              })
              .OrderByDescending(x => x.DATA_SOLICITACAO)
              .ToList();
        }

        public static async Task<List<PedidoModel>> GetAllPedidoByPrestador()
        {
            return (await firebase
              .Child(TabelasFirebase.PEDIDOS)
              .OnceAsync<PedidoModel>())
              .Where(a => a.Object.COD_PRESTADOR == Settings.UserKey)
              .Select(item => new PedidoModel
              {
                  ID = item.Object.ID,
                  COD_FRETE = item.Object.COD_FRETE,
                  COD_USUARIO = item.Object.COD_USUARIO,
                  COD_PRESTADOR = item.Object.COD_PRESTADOR,
                  STATUS = item.Object.STATUS,
                  DESCRICAO = item.Object.DESCRICAO,
                  OBS = item.Object.OBS,
                  LOCAL_DESTINO = item.Object.LOCAL_DESTINO,
                  DATA_ENVIO = item.Object.DATA_ENVIO,
                  DATA_SOLICITACAO = item.Object.DATA_SOLICITACAO,
                  INATIVO_PRESTADOR = item.Object.INATIVO_PRESTADOR
              })
              .OrderByDescending(x => x.DATA_SOLICITACAO)
              .ToList();
        }

        public static async Task AddPedido(PedidoModel pedido)
        {
            pedido.ID = Guid.NewGuid();
            pedido.COD_USUARIO = Settings.UserKey;

            await firebase
              .Child(TabelasFirebase.PEDIDOS)
              .PostAsync(pedido);
        }

        public static async Task UpdatePedido(PedidoModel pedido)
        {
            var toUpdate = (await firebase
              .Child(TabelasFirebase.PEDIDOS)
              .OnceAsync<PedidoModel>())
              .Where(a => a.Object.ID == pedido.ID)
              .FirstOrDefault();

            await firebase
              .Child(TabelasFirebase.PEDIDOS)
              .Child(toUpdate.Key)
              .PutAsync(pedido);
        }

        public static PedidoModel ShowPedido(Guid id)
        {
            var thePedido = ( firebase
              .Child(TabelasFirebase.PEDIDOS)
              .OnceAsync<PedidoModel>().GetAwaiter().GetResult())
              .Where(a => a.Object.ID == id).FirstOrDefault().Object;

            return thePedido;
        }

        public static PedidoModel ShowPedidoByFrete(Guid cod_frete)
        {
            var thePedido = (firebase
              .Child(TabelasFirebase.PEDIDOS)
              .OnceAsync<PedidoModel>().GetAwaiter().GetResult())
              .Where(a => a.Object.COD_FRETE == cod_frete)
              .Where(x => x.Object.COD_USUARIO == Settings.UserKey)
              .FirstOrDefault();

            if (thePedido == null)
                return null;
            else
                return thePedido.Object;
        }

    }
}
