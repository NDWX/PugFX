namespace Pug.Application
{
    public class AppDomainUserSessionProvider : IUserSessionProvider
    {
        private static IUserSession _userSession;

        public IUserSession CurrentSession
        {
            get
            {
                if (_userSession == null)
                    _userSession = new UserSession();

                return _userSession;
            }
        }

        public event SessionEventHandler SessionStarted;
        public event SessionEventHandler SessionEnded;
    }
}


