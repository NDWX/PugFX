using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.ComponentModel;

using System.Runtime.InteropServices;

namespace Pug.Application.Security.Extensions
{
	public static class SecureStringExtensions
	{
		public static byte[] ToByteArray(this SecureString secret)
		{
			byte[] data = new byte[secret.Length * 2];

			IntPtr dataPointer = IntPtr.Zero;

			try
			{
				dataPointer = Marshal.SecureStringToGlobalAllocUnicode(secret);

				for (int dataIdx = 0; dataIdx < secret.Length * 2; dataIdx++)
					data[dataIdx] = Marshal.ReadByte(dataPointer, dataIdx);
			}
			catch
			{
				if (dataPointer != IntPtr.Zero)
					Marshal.ZeroFreeGlobalAllocUnicode(dataPointer);

				for (int dataIdx = 0; dataIdx < data.Length * 2; dataIdx++)
					data[dataIdx] = 0;
			}

			return data;
		}

		public static string ToText(this SecureString secret)
		{
			return Encoding.Unicode.GetString(secret.ToByteArray());
		}
	}
}
