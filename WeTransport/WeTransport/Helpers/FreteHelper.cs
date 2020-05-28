using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeTransport.Models;

namespace WeTransport.Helpers
{
    public class FreteHelper
    {
        static FirebaseClient firebase = new FirebaseClient(Config.URL_DATABASE);

        public static async Task<List<vwFrete>> GetAllFretesByUser()
        {
            return (await firebase
              .Child(TabelasFirebase.FRETES)
              .OnceAsync<vwFrete>()).Where(a => a.Object.COD_USUARIO == Settings.UserKey).Select(item => new vwFrete
              {
                  ID = item.Object.ID,
                  TIPO = item.Object.TIPO,
                  COD_USUARIO = item.Object.COD_USUARIO,
                  VEICULO = item.Object.VEICULO,
                  CIDADE_PARTIDA = item.Object.CIDADE_PARTIDA,
                  CIDADE_DESTINO = item.Object.CIDADE_DESTINO,
                  ESTADO_PARTIDA = item.Object.ESTADO_PARTIDA,
                  ESTADO_DESTINO = item.Object.ESTADO_DESTINO,
                  VALOR = item.Object.VALOR,
                  DISPONIBILIDADE = item.Object.DISPONIBILIDADE,
                  DISPONIBILIDADE_INICIAL = item.Object.DISPONIBILIDADE_INICIAL,
                  DISPONIBILIDADE_FINAL = item.Object.DISPONIBILIDADE_FINAL,
                  OBS = item.Object.OBS,
                  STATUS = item.Object.STATUS
              })
              .OrderBy(x => x.ID)
              .ToList();
        }

        public static async Task<List<vwFrete>> GetAllFretesAvailable()
        {
            return (await firebase
              .Child(TabelasFirebase.FRETES)
              .OnceAsync<vwFrete>())
              .Where(a => a.Object.STATUS == 0)
              .Select(item => new vwFrete
              {
                  ID = item.Object.ID,
                  TIPO = item.Object.TIPO,
                  COD_USUARIO = item.Object.COD_USUARIO,
                  VEICULO = item.Object.VEICULO,
                  CIDADE_PARTIDA = item.Object.CIDADE_PARTIDA,
                  CIDADE_DESTINO = item.Object.CIDADE_DESTINO,
                  ESTADO_PARTIDA = item.Object.ESTADO_PARTIDA,
                  ESTADO_DESTINO = item.Object.ESTADO_DESTINO,
                  VALOR = item.Object.VALOR,
                  DISPONIBILIDADE = item.Object.DISPONIBILIDADE,
                  DISPONIBILIDADE_INICIAL = item.Object.DISPONIBILIDADE_INICIAL,
                  DISPONIBILIDADE_FINAL = item.Object.DISPONIBILIDADE_FINAL,
                  OBS = item.Object.OBS,
                  STATUS = item.Object.STATUS
              })
              .OrderBy(x => x.ID)
              .ToList();
        }

        public static async Task AddFrete(FreteModel frete)
        {
            frete.ID = Guid.NewGuid();
            frete.COD_USUARIO = Settings.UserKey;

            await firebase
              .Child(TabelasFirebase.FRETES)
              .PostAsync(frete);
        }

        public static async Task<FreteModel> GetFrete(Guid id)
        {
            //var allFretes = await GetAllFretes();
            //await firebase
            //  .Child(TabelasFirebase.FRETES)
            //  .OnceAsync<FreteModel>();
            //return allFretes.Where(a => a.ID == id).FirstOrDefault();

            var theFrete = (await firebase
              .Child(TabelasFirebase.FRETES)
              .OnceAsync<FreteModel>()).Where(a => a.Object.ID == id).FirstOrDefault();
            return theFrete.Object;
        }

        public static async Task UpdateFrete(FreteModel frete)
        {
            var toUpdateFrete = (await firebase
              .Child(TabelasFirebase.FRETES)
              .OnceAsync<FreteModel>()).Where(a => a.Object.ID == frete.ID).FirstOrDefault();

            await firebase
              .Child(TabelasFirebase.FRETES)
              .Child(toUpdateFrete.Key)
              .PutAsync(frete);
        }

        public static async Task DeleteFrete(Guid id)
        {
            var toDeleteFrete = (await firebase
              .Child(TabelasFirebase.FRETES)
              .OnceAsync<FreteModel>()).Where(a => a.Object.ID == id).FirstOrDefault();
            await firebase.Child(TabelasFirebase.FRETES).Child(toDeleteFrete.Key).DeleteAsync();

        }
    }
}
