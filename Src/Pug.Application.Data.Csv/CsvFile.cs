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
//public CsvLine ReadRecord()
//{
//    List<CsvLine> lines = new List<CsvLine>();
//    CsvLine line;
//    List<string> lineValues = new List<string>();

//    FileStream fileStream = File.OpenRead(path);

//    byte[] readData = new byte[256];

//    int lastReadBytes = 0;
//    int readBytes = 0;
//    int charIdx;

//    StringBuilder valueBuilder = new StringBuilder();
//    char character;

//    bool waitingForClosingQuote = false, previousCharacterIsQuote = false, isStartOfNewValue = true;

//    while (lastReadBytes < fileStream.Length)
//    {
//        lastReadBytes = fileStream.Read(readData, readBytes, 256);

//        readBytes += lastReadBytes;

//        charIdx = 0;

//        while (charIdx < lastReadBytes)
//        {
//            character = (char)readData[charIdx];

//            switch (character)
//            {
//                case '"':
//                    if (isStartOfNewValue)
//                    {
//                        waitingForClosingQuote = true;
//                        isStartOfNewValue = false;
//                    }
//                    else
//                    {
//                        if (previousCharacterIsQuote)
//                            valueBuilder.Append(character);
//                        else
//                            previousCharacterIsQuote = true;
//                    }
//                    break;

//                case ',':

//                    if (isStartOfNewValue)
//                        isStartOfNewValue = false;

//                    if (!waitingForClosingQuote)
//                    {
//                        lineValues.Add(valueBuilder.ToString());
//                        valueBuilder.Clear();
//                        isStartOfNewValue = true;
//                    }
//                    else
//                    {
//                        if (previousCharacterIsQuote)
//                        {
//                            waitingForClosingQuote = false;

//                            lineValues.Add(valueBuilder.ToString());
//                            valueBuilder.Clear();
//                            isStartOfNewValue = true;
//                        }
//                        else
//                        {
//                            valueBuilder.Append(character);
//                        }
//                    }

//                    previousCharacterIsQuote = false;

//                    break;

//                case '\n':
//                    if (isStartOfNewValue)
//                        isStartOfNewValue = false;

//                    if (waitingForClosingQuote)
//                    {
//                        valueBuilder.Append(character);
//                    }
//                    else
//                    {
//                        lineValues.Add(valueBuilder.ToString());
//                        valueBuilder.Clear();
//                        isStartOfNewValue = true;

//                        line = new CsvLine(lineValues.ToArray());
//                        lines.Add(line);
//                        lineValues = new List<string>();
//                    }

//                    previousCharacterIsQuote = false;
//                    break;

//                default:
//                    if (isStartOfNewValue)
//                        isStartOfNewValue = false;

//                    valueBuilder.Append(character);

//                    previousCharacterIsQuote = false;
//                    break;
//            }

//            charIdx++;
//        }
//    }

//    if (lineValues.Count > 0)
//    {
//        lineValues.Add(valueBuilder.ToString());
//        line = new CsvLine(lineValues.ToArray());
//        lines.Add(line);
//    }

//    return new CsvData(lines.ToArray());
//}

//public CsvLine this[int index]
//{
//    get
//    {
//        if (index > lines.Length)
//            throw new IndexOutOfRangeException();

//        return this.lines[index];
//    }
//}

//public static string Parse(string line, char separator)
//{
//    string[] prospectiveValues = line.Split(separator);

//    List<string> values = new List<string>();

//    int index = 0;
//    bool valueIsQuoted = false;
//    string currentValue = string.Empty;
//    char quoteCharacter;

//    while (index < prospectiveValues.Length)
//    {
//        currentValue = ReadValue(prospectiveValues, ref index, separator);
//        currentValue = prospectiveValues[index];



//        index++;
//    }
//}

//protected static string ReadValue(string[] values, ref int valueIndex, char separator, out bool lastValueFinished)
//{
//    StringBuilder valueBuilder = new StringBuilder(values[valueIndex]);
//    lastValueFinished = false;

//    while (valueIndex < values.Length - 1)
//    {
//        valueIndex++;

//        if (values[valueIndex].StartsWith('"'.ToString()))
//        {
//            lastValueFinished = true;
//            break;
//        }

//        valueBuilder.Append(separator);
//        valueBuilder.Append(values[valueIndex]);

//        if (values[valueIndex].EndsWith('"'.ToString()))
//        {
//            lastValueFinished = true;
//            break;
//        }
//    }

//    return valueBuilder.ToString();
//}