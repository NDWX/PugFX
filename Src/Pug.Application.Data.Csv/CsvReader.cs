using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pug.Application.Data.Csv
{
	public class CsvReader : IDisposable
	{
		private Stream _fileStream;

		private byte[] _readData = new byte[256];

		// ReSharper disable RedundantDefaultMemberInitializer
		private int _lastReadBytes = 0;
		private int _readBytes = 0;
		// ReSharper restore RedundantDefaultMemberInitializer

		private int _charIdx;

		protected CsvReader(string file)
		{
			_fileStream = File.OpenRead(file);
		}

		protected CsvReader(Stream stream)
		{
			_fileStream = stream;
		}

		public CsvLine ReadLine()
		{
			CsvLine line = null;
			List<string> lineValues = new List<string>();
			StringBuilder valueBuilder = new StringBuilder();

			char character;

			bool waitingForClosingQuote = false, previousCharacterIsQuote = false, isStartOfNewValue = true;

			Action<string> saveContentAsValue = value =>
			{
				lineValues.Add(value);
				valueBuilder = new StringBuilder();
				isStartOfNewValue = true;
			};

			Action saveBuilderContentAsValue = () => saveContentAsValue(valueBuilder.ToString());

			Action saveValuesAsLine = () => line = new CsvLine(lineValues.ToArray());

			Func<bool> isNewLine = () => valueBuilder.Length == 0 && lineValues.Count == 0;

			while (line == null)
			{
				while (line == null && _charIdx < _lastReadBytes)
				{
					character = (char)_readData[_charIdx];

					switch (character)
					{
						case '"':

							if (isStartOfNewValue)
							{
								waitingForClosingQuote = true;
								isStartOfNewValue = false;
							}
							else
							{
								if (previousCharacterIsQuote)
								{
									valueBuilder.Append(character);
									previousCharacterIsQuote = false;
								}
								else
								{
									previousCharacterIsQuote = true;
								}
							}

							break;

						case ',':

							if (isStartOfNewValue)
								isStartOfNewValue = false;

							if (!waitingForClosingQuote)
							{
								saveBuilderContentAsValue();
							}
							else
							{
								if (previousCharacterIsQuote)
								{
									waitingForClosingQuote = false;

									saveBuilderContentAsValue();
								}
								else
								{
									valueBuilder.Append(character);
								}
							}

							previousCharacterIsQuote = false;

							break;

						case '\r':
							if (isNewLine())
								break;

							if (isStartOfNewValue)
								isStartOfNewValue = false;

							if (waitingForClosingQuote)
							{
								if (previousCharacterIsQuote)
								{
									waitingForClosingQuote = false;

									saveBuilderContentAsValue();
									saveValuesAsLine();
								}
								else
								{
									valueBuilder.Append(character);
								}
							}
							else
							{
								saveBuilderContentAsValue();
								saveValuesAsLine();
							}

							previousCharacterIsQuote = false;
							break;

						case '\n':
							if (isNewLine())
								break;

							if (isStartOfNewValue)
								isStartOfNewValue = false;

							if (waitingForClosingQuote)
							{
								if (previousCharacterIsQuote)
								{
									waitingForClosingQuote = false;

									saveBuilderContentAsValue();
									saveValuesAsLine();
								}
								else
								{
									valueBuilder.Append(character);
								}
							}
							else
							{
								saveBuilderContentAsValue();
								saveValuesAsLine();
							}

							previousCharacterIsQuote = false;
							break;

						default:

							if (isStartOfNewValue)
								isStartOfNewValue = false;

							valueBuilder.Append(character);

							previousCharacterIsQuote = false;
							break;
					}

					_charIdx++;
				}

				if (line == null)
				{
					if (_readBytes < _fileStream.Length)
					{
						_lastReadBytes = _fileStream.Read(_readData, 0, 256);
						_readBytes += _lastReadBytes;

						_charIdx = 0;
					}
					else
					{
						if (lineValues.Count > 0)
						{
							lineValues.Add(valueBuilder.ToString());
							saveValuesAsLine();
						}
						else
						{
							return null;
						}
					}
				}
			}

			return line;
		}

		public static CsvReader Read(Stream stream)
		{
			return new CsvReader(stream);
		}

		public void Close()
		{
			if (_fileStream != null)
			{
#if NETFX
                fileStream.Close();
#endif
                _fileStream.Dispose();
			}

			_fileStream = null;
		}

		#region IDisposable Members

		public void Dispose()
		{
			Close();
		}

		#endregion
	}
}
