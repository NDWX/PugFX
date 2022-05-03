using System.IO;

namespace Pug.IO.Extensions
{
	public static class StreamExtensions
	{
#if !NET4
		public static void CopyTo(this Stream source, Stream destination)
		{
			int dataRead = 0;
			long totalDataRead = 0;
			byte[] buffer = new byte[4096];

			while (totalDataRead < source.Length)
			{
				dataRead = source.Read(buffer, 0, source.Length < buffer.Length ? (int)source.Length : buffer.Length);

				totalDataRead += dataRead;

				destination.Write(buffer, 0, dataRead);
			}
		}
#endif
	}
}