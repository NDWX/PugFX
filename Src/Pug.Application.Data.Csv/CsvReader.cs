using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pug.Application.Data.Csv
{
	public class CsvReader : IDisposable
	{
		Stream fileStream;

		byte[] readData = new byte[256];

		int lastReadBytes = 0;
		int readBytes = 0;

		int charIdx;

		protected CsvReader(string file)
		{
			fileStream = File.OpenRead(file);
		}

		protected CsvReader(Stream stream)
		{
			this.fileStream = stream;
		}

		public CsvLine ReadLine()
		{
			CsvLine line = null;
			List<string> lineValues = new List<string>();
			StringBuilder valueBuilder = new StringBuilder();

			char character;

			bool waitingForClosingQuote = false, previousCharacterIsQuote = false, isStartOfNewValue = true;

			Action<string> saveContentAsValue = new Action<string>(
				delegate(string value)
				{
					lineValues.Add(value);
					valueBuilder = new StringBuilder();
					isStartOfNewValue = true;
				}
			);

			Action saveBuilderContentAsValue = new Action(
				delegate()
				{
					saveContentAsValue(valueBuilder.ToString());
				}
			);

			Action saveValuesAsLine = new Action(
				delegate()
				{
					line = new CsvLine(lineValues.ToArray());
				}
			);

			Func<bool> isNewLine = new Func<bool>(
				delegate()
				{
					return valueBuilder.Length == 0 && lineValues.Count == 0;
				}
			);

			while (line == null)
			{
				while (line == null && charIdx < lastReadBytes)
				{
					character = (char)readData[charIdx];

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

					charIdx++;
				}

				if (line == null)
				{
					if (readBytes < fileStream.Length)
					{
						lastReadBytes = fileStream.Read(readData, 0, 256);
						readBytes += lastReadBytes;

						charIdx = 0;
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
			if (fileStream != null)
			{
#if NETFX
                fileStream.Close();
#endif
                fileStream.Dispose();
			}

			fileStream = null;
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
