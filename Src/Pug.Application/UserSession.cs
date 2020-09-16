using System;
using System.Collections.Generic;

namespace Pug.Application
{
    public class UserSession : IUserSession
    {
        Dictionary<string, Dictionary<Type, object>> store;
        
        public UserSession()
        {
            this.store = new Dictionary<string, Dictionary<Type, object>>();
        }

        public event EventHandler Ending;

        public T Get<T>(string identifier = "")
        {
            if (!store.ContainsKey(identifier))
                return default(T);

            Dictionary<Type, object> typeVariables = (Dictionary<Type, object>)store[identifier];

            if (typeVariables.ContainsKey(typeof(T)))
                return (T)typeVariables[typeof(T)];

            return default(T);
        }

        public void Remove<T>(string identifier)
        {
            Dictionary<Type, object> typeVariables;

            if (store.ContainsKey(identifier))
            {
                typeVariables = (Dictionary<Type, object>)store[identifier];

                typeVariables.Remove(typeof(T));

                if (typeVariables.Count == 0)
                    store.Remove(identifier);
            }
        }

        public void Set<T>(string identifier, T value)
        {
            Dictionary<Type, object> typeVariables;

            if (!store.ContainsKey(identifier))
            {
                typeVariables = new Dictionary<Type, object>();
                store.Add(identifier, typeVariables);
            }
            else
            {
                typeVariables = (Dictionary<Type, object>)store[identifier];
            }

            typeVariables[typeof(T)] = value;
        }
    }
}


