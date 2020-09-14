using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pug.Application.Data.Csv
{
	public class CsvLine
	{
		ICollection<string> values;
		char delimiter;

		public CsvLine(ICollection<string> values, char delimiter)
		{
			this.values = values;
			this.delimiter = delimiter;
		}

		public CsvLine(string[] values)
			: this(values, ',')
		{
			this.values = values;
		}

		public int Length
		{
			get
			{
				return this.values.Count;
			}
		}

		public string this[int index]
		{
			get
			{
				if (index > values.Count)
					throw new IndexOutOfRangeException();

				return values.ElementAt(index);
			}
		}

		void Write(string value, TextWriter writer, char delimiter)
		{
			bool enclosingQuotesRequired = false;

			if (value.Contains('"'))
			{
				value.Replace("\"", "\"\"");

				enclosingQuotesRequired = true;
			}

			if (value.Contains(' ') || value.Contains("\n") || value.Contains("\r") || value.Contains(delimiter))
			{
				enclosingQuotesRequired = true;
			}

			if (enclosingQuotesRequired)
				value = "\"" + value + "\"";

			writer.Write(value);
		}

		public void Write(TextWriter writer, char delimiter)
		{
			foreach (string value in values)
			{
				Write(value, writer, delimiter);
			}

			writer.WriteLine();
		}

		public void Write(TextWriter writer)
		{
			Write(writer, this.delimiter);
		}

		public void Write(Stream stream, char delimiter)
		{
			TextWriter writer = new StreamWriter(stream);

			Write(stream);
		}

		public void Write(Stream stream)
		{
			Write(stream, this.delimiter);
		}
	}
}
