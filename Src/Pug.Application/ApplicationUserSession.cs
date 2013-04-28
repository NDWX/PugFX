using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace Pug.Application
{
	public class ApplicationUserSession
	{
		string identifier;
		Dictionary<string, Dictionary<Type, object>> sessionVariables;

		public ApplicationUserSession(string identifier)
		{
			this.identifier = identifier;
			sessionVariables = new Dictionary<string, Dictionary<Type, object>>();
		}

		public string Identifier
		{
			get
			{
				return identifier;
			}
		}

		public T Get<T>(string identifier = "")
		{
			if (!sessionVariables.ContainsKey(identifier))
				return default(T);

			Dictionary<Type, object> typeVariables = sessionVariables[identifier];

			if (typeVariables.ContainsKey(typeof(T)) )
				return (T)typeVariables[typeof(T)];

			return default(T);
		}

		public void Set<T>(string identifier, T value)
		{
			Dictionary<Type, object> typeVariables;

			if (!sessionVariables.ContainsKey(identifier))
			{
				typeVariables = new Dictionary<Type, object>();
				sessionVariables.Add(identifier, typeVariables);
			}
			else
			{
				typeVariables = sessionVariables[identifier];
			}

			typeVariables[typeof(T)] = value;
		}

		public void Remove<T>(string identifier)
		{
			Dictionary<Type, object> typeVariables;

			if (sessionVariables.ContainsKey(identifier))
			{
				typeVariables = sessionVariables[identifier];

				typeVariables.Remove(typeof(T));

				if (typeVariables.Count == 0)
					sessionVariables.Remove(identifier);
			}
		}

	}
}