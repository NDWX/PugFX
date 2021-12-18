namespace Pug.Application
{
    public class AppDomainUserSessionProvider : IUserSessionProvider
    {
        public IUserSession CurrentSession =>
            UserSessionProviderAppDomainStore.UserSession ??
            (UserSessionProviderAppDomainStore.UserSession = new UserSession());

        public event SessionEventHandler SessionStarted;
        public event SessionEventHandler SessionEnded;
    }
}


