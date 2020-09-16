using System;
using System.Collections.Generic;

namespace Pug.Application
{
    public delegate void SessionEventHandler(IUserSession session);

    public interface IUserSessionProvider
    {
        event SessionEventHandler SessionStarted;

        event SessionEventHandler SessionEnded;

        IUserSession CurrentSession
        {
            get;
        }
    }
}


