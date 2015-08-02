using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Security.Cryptography
{
	public class TextIsBidirectional : Exception
	{
		public string Text
		{
			get;
			protected set;
		}

		public TextIsBidirectional(string text)
			: base(string.Format("The following text is bidirectional:{0}{0}\t{1}", Environment.NewLine, text))
		{
			this.Text = text;
		}
	}
}
