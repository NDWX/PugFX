using System;
using System.Runtime.Serialization;
using System.Text;

namespace Pug.Bizcotti.Geography
{
	public class Address : IEquatable<Address>
	{
		string unitType, unitNumber, place, streetNumber, streetName, streetType /*, neighbourhood, municipality, postCode, territory, country*/;
		LocalityInfo locality;

		public Address(string unitType, string unitNumber, string place, string streetNumber, string streetName, string streetType, LocalityInfo locality)
		{
			this.unitType = unitType;
			this.unitNumber = unitNumber;
			this.place = place;
			this.streetNumber = streetNumber;
			this.streetName = streetName;
			this.streetType = streetType;

			this.locality = locality;
		}

		public Address(string unitType, string unitNumber, string place, string streetNumber, string streetName, string streetType, string neighbourhood, string municipality, string postalCode, string territory, string region, string country) : this(unitType, unitNumber, place, streetNumber, streetName, streetType, new LocalityInfo(neighbourhood, municipality, postalCode, new TerritoryInfo(territory, region, country)))
		{
			//this.neighbourhood = neighbourhood;
			//this.municipality = municipality;
			//this.postCode = postCode;
			//this.territory = territory;
			//this.country = country;
		}

		[DataMember]
		public string UnitType
		{
			get
			{
				return unitType;
			}
			protected set
			{
				this.unitType = value;
			}
		}

		[DataMember]
		public string UnitNumber
		{
			get
			{
				return unitNumber;
			}
			protected set
			{
				this.unitNumber = value;
			}
		}

		[DataMember]
		public string Place
		{
			get
			{
				return place;
			}
			protected set
			{
				this.place = value;
			}
		}

		[DataMember]
		public string StreetNumber
		{
			get
			{
				return streetNumber;
			}
			protected set
			{
				this.streetNumber = value;
			}
		}

		[DataMember]
		public string StreetName
		{
			get
			{
				return streetName;
			}
			protected set
			{
				this.streetName = value;
			}
		}

		[DataMember]
		public string StreetType
		{
			get
			{
				return streetType;
			}
			protected set
			{
				this.streetType = value;
			}
		}

		[DataMember]
		public LocalityInfo Locality
		{
			get
			{
				return locality;
			}
			protected set
			{
				this.locality = value;
			}
		}

		//[DataMember]
		//public string Neighbourhood
		//{
		//    get
		//    {
		//        return neighbourhood;
		//    }
		//    protected set
		//    {
		//        this.neighbourhood = value;
		//    }
		//}

		//[DataMember]
		//public string Municipality
		//{
		//    get
		//    {
		//        return municipality;
		//    }
		//    protected set
		//    {
		//        this.municipality = value;
		//    }
		//}

		//[DataMember]
		//public string PostCode
		//{
		//    get
		//    {
		//        return postCode;
		//    }
		//    protected set
		//    {
		//        this.postCode = value;
		//    }
		//}

		//[DataMember]
		//public string Territory
		//{
		//    get
		//    {
		//        return territory;
		//    }
		//    protected set
		//    {
		//        this.territory = value;
		//    }
		//}

		//[DataMember]
		//public string Country
		//{
		//    get
		//    {
		//        return country;
		//    }
		//    protected set
		//    {
		//        this.country = country;
		//    }
		//}

		public override string ToString()
		{
			System.Text.StringBuilder addressBuilder = new StringBuilder();

			if (!string.IsNullOrEmpty(place))
			{
				addressBuilder.AppendFormat("{0} {1}, {2}", unitType, unitNumber, place);
				addressBuilder.AppendLine(string.Format("{2} {3} {4}", streetNumber, streetName, streetType));
			}
			else
			{
				addressBuilder.AppendFormat("{0} {1}, {2} {3} {4}", unitType, unitNumber, streetNumber, streetNumber, streetType);
			}

			if (!string.IsNullOrEmpty(locality.Name))
				addressBuilder.AppendLine(locality.Name);

			addressBuilder.AppendLine(string.Format("{7} {8} {9}\n{10}", locality.Municipality, locality.Territory.Name, locality.PostalCode, locality.Territory.Country));

			return addressBuilder.ToString();
		}

		#region IEquatable<Address> Members

		public bool Equals(Address other)
		{
			return this.unitType == other.unitType && this.unitNumber == other.unitNumber && this.place == other.place && this.streetNumber == other.streetNumber && this.streetName == other.streetName && this.streetType == other.streetType && this.locality.Equals(other.locality);
		}

		#endregion
	}
}