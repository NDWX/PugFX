namespace Pug.Application
{
    public class AppDomainUserSessionProvider : IUserSessionProvider
    {
        static IUserSession userSession = null;

        public IUserSession CurrentSession
        {
            get
            {
                if (userSession == null)
                    userSession = new UserSession();

                return userSession;
            }
        }

        public event SessionEventHandler SessionStarted;
        public event SessionEventHandler SessionEnded;
    }
}


