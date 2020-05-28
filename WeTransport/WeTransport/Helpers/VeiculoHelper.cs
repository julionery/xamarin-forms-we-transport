using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeTransport.Models;

namespace WeTransport.Helpers
{
    public class VeiculoHelper
    {
        static FirebaseClient firebase = new FirebaseClient(Config.URL_DATABASE);

        public static async Task<List<vwVeiculo>> GetAllVeiculos()
        {
            return (await firebase
              .Child(TabelasFirebase.VEICULOS)
              .OnceAsync<vwVeiculo>()).Where(a => a.Object.COD_USUARIO == Settings.UserKey).Select(item => new vwVeiculo
              {
                  ID = item.Object.ID,
                  COD_USUARIO = item.Object.COD_USUARIO,
                  NOME = item.Object.NOME,
                  DESCRICAO = item.Object.DESCRICAO,
                  TIPO = item.Object.TIPO,
                  TIPO_CAMINHAO = item.Object.TIPO_CAMINHAO,
                  TIPO_CARROCERIA = item.Object.TIPO_CARROCERIA,
                  PLACA = item.Object.PLACA,
                  MODELO = item.Object.MODELO,
                  COR = item.Object.COR,
                  OBS = item.Object.OBS
              })
              .OrderBy(x => x.ID)
              .ToList();
        }

        public static async Task AddVeiculo(VeiculoModel veiculo)
        {
            veiculo.ID = Guid.NewGuid();
            veiculo.COD_USUARIO = Settings.UserKey;

            await firebase
              .Child(TabelasFirebase.VEICULOS)
              .PostAsync(veiculo);
        }

        public static async Task<VeiculoModel> GetVeiculo(Guid id)
        {
            var allVeiculos = await GetAllVeiculos();
            await firebase
              .Child(TabelasFirebase.VEICULOS)
              .OnceAsync<VeiculoModel>();
            return allVeiculos.Where(a => a.ID == id).FirstOrDefault();
        }

        public static VeiculoModel ShowVeiculo(Guid id)
        {
            var toVeiculo = ( firebase
              .Child(TabelasFirebase.VEICULOS)
              .OnceAsync<VeiculoModel>().GetAwaiter().GetResult())
              .Where(a => a.Object.ID == id).FirstOrDefault().Object;

            return toVeiculo;
        }

        public static async Task UpdateVeiculo(VeiculoModel veiculo)
        {
            var toUpdateVeiculo = (await firebase
              .Child(TabelasFirebase.VEICULOS)
              .OnceAsync<VeiculoModel>()).Where(a => a.Object.ID == veiculo.ID).FirstOrDefault();

            await firebase
              .Child(TabelasFirebase.VEICULOS)
              .Child(toUpdateVeiculo.Key)
              .PutAsync(veiculo);
        }

        public static async Task DeleteVeiculo(Guid id)
        {
            var toDeleteVeiculo = (await firebase
              .Child(TabelasFirebase.VEICULOS)
              .OnceAsync<VeiculoModel>()).Where(a => a.Object.ID == id).FirstOrDefault();
            await firebase.Child(TabelasFirebase.VEICULOS).Child(toDeleteVeiculo.Key).DeleteAsync();

        }
    }
}
