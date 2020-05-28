using System.Threading.Tasks;
using Firebase.Auth;
using WeTransport.Services;

namespace WeTransport.iOS.Service
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public Task<string> CreateUserWithEmailAndPassword(string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            //var user = await Auth.DefaultInstance.SignInAsync(email, password);
            //return await user.GetIdTokenAsync();
            throw new System.NotImplementedException();

        }
    }
}