using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pug.Application.Data.Csv
{
	public class CsvLine
	{
		string[] values;

		internal CsvLine(string[] values)
		{
			this.values = values;
		}

		public int Length
		{
			get
			{
				return this.values.Length;
			}
		}

		public string this[int index]
		{
			get
			{
				if (index > values.Length)
					throw new IndexOutOfRangeException();

				return values[index];
			}
		}
	}
}
