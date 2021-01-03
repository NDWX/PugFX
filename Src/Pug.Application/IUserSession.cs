using System;

namespace Pug.Application
{
    public interface IUserSession
    {
        T Get<T>(string identifier = "");

        void Set<T>(string identifier, T value);

        void Remove<T>(string identifier);

        event EventHandler Ending;
    }
}


