using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Pug.Serialization
{
	public class JsonFormatter : System.Runtime.Serialization.IFormatter
	{
		SerializationBinder binder;
		StreamingContext context;

		public JsonFormatter(SerializationBinder binder, StreamingContext context)
		{
			this.binder = binder;
			this.context = context;
		}

		public override System.Runtime.Serialization.SerializationBinder Binder
		{
			get
			{
				return binder;
			}
			set
			{
				binder = value;
			}
		}

		public override System.Runtime.Serialization.StreamingContext Context
		{
			get
			{
				return context;
			}
			set
			{
				context = value;
			}
		}


		#region IFormatter Members


		public object Deserialize(System.IO.Stream serializationStream)
		{
			throw new NotImplementedException();
		}

		public void Serialize(System.IO.Stream serializationStream, object graph)
		{
			ISerializable serializable = (ISerializable)graph;

			SerializationInfo info = new SerializationInfo(graph.GetType(), )
			serializable.GetObjectData()
		}

		string GetString(object graph)
		{
			Type objectType = graph.GetType();

			if( objectType.IsPrimitive )
		}

		string GetVariableString(string name, string value)
		{
			return string.Format("\"{0}\" : \"{1}\"", name, value);
		}

		string GetVariableString(string name, Int16 value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, Int32 value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, Int64 value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		} 

		string GetVariableString(string name, UInt16 value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, UInt32 value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, UInt64 value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, decimal value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, float value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		string GetVariableString(string name, long value)
		{
			return string.Format("\"{0}\" : {1}", name, value.ToString());
		}

		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
