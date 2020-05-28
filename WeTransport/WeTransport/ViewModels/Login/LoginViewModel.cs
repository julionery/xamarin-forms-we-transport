using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using WeTransport.Services;
using WeTransport.Validations;
using WeTransport.Helpers;

namespace WeTransport.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region + Variáveis Globais

        UsuarioHelper firebaseHelper = new UsuarioHelper();
        readonly IFirebaseAuthenticator firebaseAuthenticator;
        readonly NavigationService navigationService;

        #endregion

        public LoginViewModel()
        {
            this.firebaseAuthenticator = DependencyService.Get<IFirebaseAuthenticator>();
            this.navigationService = new NavigationService();
        }

        public async Task Login(string email, string senha)
        {
            Settings.UserToken = await firebaseAuthenticator.LoginWithEmailPassword(email, senha);
            Settings.UserKey = await firebaseHelper.GetUserKey(email);

            var pessoa = await firebaseHelper.GetPerson(email);
            Settings.UserName = pessoa.NOME;
            Settings.UserEmail = pessoa.EMAIL;
            Settings.UserType = pessoa.TIPO;

            MessagingCenter.Send(EventArgs.Empty, "GoToLogin");
        }

    }
}
