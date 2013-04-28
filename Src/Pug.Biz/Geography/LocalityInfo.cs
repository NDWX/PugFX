using System;
using System.Runtime.Serialization;
using System.Text;

namespace Pug.Bizcotti.Geography
{
	[DataContract]
	public class LocalityInfo : IEquatable<LocalityInfo>, IEquatable<TerritoryInfo>
	{
		string name, municipality, postalCode;
		TerritoryInfo territoryInfo;

		public LocalityInfo(string name, string municipality, string postalCode, TerritoryInfo territoryInfo)
		{
			this.name = name;
			this.municipality = municipality;
			this.postalCode = postalCode;
			this.territoryInfo = territoryInfo;

		}

		#region ILocalityInfo<TerritoryInfo> Members

		[DataMember]
		public string Name
		{
			get { return name; }
			protected set { name = value; }
		}

		[DataMember]
		public string Municipality
		{
			get { return municipality; }
			protected set { municipality = value; }
		}

		[DataMember]
		public TerritoryInfo Territory
		{
			get { return territoryInfo; }
			protected set { territoryInfo = value; }
		}

		[DataMember]
		public string PostalCode
		{
			get { return postalCode; }
			protected set { postalCode = value; }
		}

		public override string ToString()
		{
			return string.Format(@"{0}\n{1}\n{2} {3}", name, municipality, territoryInfo.ToString(), postalCode);
		}

		#endregion

		#region IEquatable<LocalityInfo> Members

		public bool Equals(LocalityInfo other)
		{
			return this.name == other.name && this.municipality == other.municipality && this.postalCode == other.postalCode && this.territoryInfo.Equals(other.territoryInfo);
		}

		#endregion

		#region IEquatable<TerritoryInfo> Members

		public bool Equals(TerritoryInfo other)
		{
			return this.territoryInfo.Equals(other);
		}

		#endregion
	}
}
