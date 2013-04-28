using System;
using System.Runtime.Serialization;
using System.Text;

namespace Pug.Bizcotti.Geography
{
	[DataContract]
	public class TerritoryInfo : IEquatable<TerritoryInfo>
	{
		string name, region, country;

		public TerritoryInfo(string name, string region, string country)
		{
			this.name = name;
			this.region = region;
			this.country = country;
		}

		#region ITerritoryInfo Members

		[DataMember]
		public string Name
		{
			get { return name; }
			protected set
			{
				this.name = value;
			}
		}

		[DataMember]
		public string Region
		{
			get { return region; }
			protected set { region = value; }
		}

		[DataMember]
		public string Country
		{
			get { return country; }
			protected set { country = value; }
		}

		public override string ToString()
		{
			return name;
		}

		#endregion

		#region IEquatable<TerritoryInfo> Members

		public bool Equals(TerritoryInfo other)
		{
			return this.name == other.name && this.region == other.region && this.country == other.country;
		}

		#endregion
	}
}
