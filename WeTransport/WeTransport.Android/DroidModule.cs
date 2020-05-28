using Autofac;
using WeTransport.Droid.Service;
using WeTransport.Services;

namespace WeTransport.Droid
{
    public class DroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FirebaseAuthenticator>().As<IFirebaseAuthenticator>().SingleInstance();
        }
    }
}
