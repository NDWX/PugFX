using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pug.Application.Data.Csv
{
	public class CsvLine
	{
		private readonly ICollection<string> _values;
		private readonly char _delimiter;

		public CsvLine(ICollection<string> values, char delimiter)
		{
			_values = values;
			_delimiter = delimiter;
		}

		public CsvLine(string[] values)
			: this(values, ',')
		{
			_values = values;
		}

		public int Length
		{
			get
			{
				return _values.Count;
			}
		}

		public string this[int index]
		{
			get
			{
				if (index > _values.Count)
					throw new IndexOutOfRangeException();

				return _values.ElementAt(index);
			}
		}

		private void Write(string value, TextWriter writer, char delimiter)
		{
			bool enclosingQuotesRequired = false;

			if (value.Contains('"'))
			{
				value = value.Replace("\"", "\"\"");

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
			foreach (string value in _values)
			{
				Write(value, writer, delimiter);
			}

			writer.WriteLine();
		}

		public void Write(TextWriter writer)
		{
			Write(writer, _delimiter);
		}

		public void Write(Stream stream, char delimiter)
		{
			TextWriter writer = new StreamWriter(stream);

			Write(writer, delimiter);
		}

		public void Write(Stream stream)
		{
			Write(stream, _delimiter);
		}
	}
}
