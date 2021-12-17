using System;
using System.IO;

namespace Pug.Application.Data.Csv
{
	public class CsvFile : IDisposable
	{
		private CsvReader _reader;

		protected CsvFile(string file)
		{
			_reader = CsvReader.Read(File.OpenRead(file));
		}

        [Obsolete]
        public CsvLine ReadNext()
        {
            return _reader.ReadLine();
        }

		public CsvLine ReadLine()
		{
			return _reader.ReadLine();
		}

		public void Dispose()
		{
			_reader.Dispose();
		}

		public static CsvFile Open(string file)
		{
			return new CsvFile(file);
		}
	}
}