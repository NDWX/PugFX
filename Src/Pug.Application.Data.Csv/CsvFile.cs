using System;
using System.IO;

namespace Pug.Application.Data.Csv
{
	public class CsvFile : IDisposable
	{
		CsvReader reader;

		protected CsvFile(string file)
		{
			reader = CsvReader.Read(File.OpenRead(file));
		}

        [Obsolete]
        public CsvLine ReadNext()
        {
            return reader.ReadLine();
        }

		public CsvLine ReadLine()
		{
			return reader.ReadLine();
		}

		public void Dispose()
		{
			reader.Dispose();
		}

		public static CsvFile Open(string file)
		{
			return new CsvFile(file);
		}
	}
}