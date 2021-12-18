using System;
using System.Collections.Generic;

namespace Pug.Application
{
    public class UserSession : IUserSession
    {
        private readonly Dictionary<string, Dictionary<Type, object>> _store;
        
        public UserSession()
        {
            _store = new Dictionary<string, Dictionary<Type, object>>();
        }

        public event EventHandler Ending;

        public T Get<T>(string identifier = "")
        {
            if (!_store.ContainsKey(identifier))
                return default(T);

            Dictionary<Type, object> typeVariables = _store[identifier];

            if (typeVariables.ContainsKey(typeof(T)))
                return (T)typeVariables[typeof(T)];

            return default(T);
        }

        public void Remove<T>(string identifier)
        {
            Dictionary<Type, object> typeVariables;

            if (_store.ContainsKey(identifier))
            {
                typeVariables = _store[identifier];

                typeVariables.Remove(typeof(T));

                if (typeVariables.Count == 0)
                    _store.Remove(identifier);
            }
        }

        public void Set<T>(string identifier, T value)
        {
            Dictionary<Type, object> typeVariables;

            if (!_store.ContainsKey(identifier))
            {
                typeVariables = new Dictionary<Type, object>();
                _store.Add(identifier, typeVariables);
            }
            else
            {
                typeVariables = _store[identifier];
            }

            typeVariables[typeof(T)] = value;
        }
    }
}


