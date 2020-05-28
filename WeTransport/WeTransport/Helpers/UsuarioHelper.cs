using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Models;
using WeTransport.Services;
using Xamarin.Forms;

namespace WeTransport.Helpers
{
    public class UsuarioHelper
    {
        static FirebaseClient firebase = new FirebaseClient(Config.URL_DATABASE);

        public async Task VerificaEmail(string email)
        {
            var toPerson = (await firebase
             .Child(TabelasFirebase.PESSOAS)
             .OnceAsync<PessoaModel>()).Where(a => a.Object.EMAIL == email).FirstOrDefault();

            if (toPerson != null)
                throw new Exception("E-mail já cadastrado!");
        }

        public async Task AddUser(string email, string senha)
        {
            IFirebaseAuthenticator firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();
            string token = await firebaseAuthenticator.CreateUserWithEmailAndPassword(email, senha);
            (Application.Current as App).AuthToken = token;
        }

        public async Task AddPerson(string name, string email, string telefone, string cpf, string cnh = "", string tipo = "user")
        {
            PessoaModel thePerson = new PessoaModel {
                NOME = name,
                EMAIL = email,
                TELEFONE = telefone,
                CPF = cpf,
                TIPO = tipo,
                CNH = cnh
            };

            var newPerson = (await firebase
              .Child(TabelasFirebase.PESSOAS)
              .PostAsync(thePerson));

            thePerson.COD_USUARIO = newPerson.Key;

            await firebase
              .Child(TabelasFirebase.PESSOAS)
              .Child(newPerson.Key)
              .PutAsync(thePerson);
        }

        public async Task<PessoaModel> GetPerson(string email)
        {
            var toPerson = (await firebase
              .Child(TabelasFirebase.PESSOAS)
              .OnceAsync<PessoaModel>())
              .Where(a => a.Object.EMAIL == email)
              .FirstOrDefault();

            PessoaModel pessoa = new PessoaModel {
                CPF = toPerson.Object.CPF,
                EMAIL = toPerson.Object.EMAIL,
                NOME = toPerson.Object.NOME,
                TELEFONE = toPerson.Object.TELEFONE,
                TIPO = toPerson.Object.TIPO,
                CNH = toPerson.Object.CNH
            };

            return pessoa;
        }

        public static PessoaModel ShowPerson(string cod_usuario)
        {
            var thePerson = (firebase
              .Child(TabelasFirebase.PESSOAS)
              .Child(cod_usuario)
              .OnceSingleAsync<PessoaModel>().GetAwaiter().GetResult());

            if (thePerson == null)
                return null;
            else
                return thePerson;
        }

        public async Task<string> GetUserKey(string email)
        {
            var toPerson = (await firebase
              .Child(TabelasFirebase.PESSOAS)
              .OnceAsync<PessoaModel>()).Where(a => a.Object.EMAIL == email).FirstOrDefault();

            if (toPerson == null)
                return null;
            else
                return toPerson.Key;
        }

        public async Task UpdatePerson(PessoaModel pessoa)
        {
            var toUpdatePerson = (await firebase
              .Child(TabelasFirebase.PESSOAS)
              .OnceAsync<PessoaModel>())
              .Where(a => a.Object.COD_USUARIO == Settings.UserKey)
              .FirstOrDefault();

            await firebase
              .Child(TabelasFirebase.PESSOAS)
              .Child(toUpdatePerson.Key)
              .PutAsync(pessoa);
        }
    }
}
