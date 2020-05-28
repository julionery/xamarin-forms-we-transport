using Autofac;
using WeTransport.iOS.Service;
using WeTransport.Services;

namespace WeTransport.iOS
{
    public class IOSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FirebaseAuthenticator>().As<IFirebaseAuthenticator>().SingleInstance();
        }
    }
}
