using System;
using System.Runtime.Serialization;

namespace Pug.Bizcotti.Mathematics
{
	[DataContract]
	public class Dimensions : IEquatable<Dimensions>, IComparable<Dimensions>
	{
		decimal length, width, height;

		public Dimensions(decimal length, decimal width, decimal height)
		{
			this.length = length;
			this.width = width;
			this.height = height;
		}

		[DataMember]
		public decimal Height
		{
			get { return height; }
			protected set { height = value; }
		}

		[DataMember]
		public decimal Width
		{
			get { return width; }
			protected set { width = value; }
		}

		[DataMember]
		public decimal Length
		{
			get { return length; }
			protected set { length = value; }
		}

		public decimal Volume
		{
			get
			{
				return length * width * height;
			}
		}

		public bool IsEmpty()
		{
			return this.length == 0 && this.width == 0 && this.height == 0;
		}

		#region IEquatable<Dimensions> Members

		public bool Equals(Dimensions other)
		{
			return this.length == other.length && this.width == other.width && this.height == other.height;
		}

		#endregion

		#region IComparable<Dimensions> Members

		public int CompareTo(Dimensions other)
		{
			if (this.Volume < other.Volume)
				return -1;

			if (this.Volume == other.Volume)
				return 0;

			return 1;
		}

		#endregion

		static Dimensions empty = new Dimensions(0, 0, 0);

		public static Dimensions Empty
		{
			get
			{
				return empty;
			}
		}
	}
}
