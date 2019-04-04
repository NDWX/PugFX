using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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