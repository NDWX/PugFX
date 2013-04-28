using System;
using System.Runtime.Serialization;

namespace Pug.Bizcotti
{
	[DataContract]
	public class CommunicationMethod : IEquatable<CommunicationMethod>
	{
		string name, address;

		public CommunicationMethod(string name, string address)
		{
			this.name = name;
			this.address = address;
		}

		[DataMember]
		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				this.name = name;
			}
		}

		[DataMember]
		public string Address
		{
			get
			{
				return address;
			}
			protected set
			{
				this.address = value;
			}
		}

		#region IEquatable<CommunicationMethod> Members

		public bool Equals(CommunicationMethod other)
		{
			return this.name == other.name && this.address == other.address;
		}

		#endregion
	}
}
