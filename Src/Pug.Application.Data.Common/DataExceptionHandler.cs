using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace Pug.Application.Data
{
	public delegate void DataExceptionHandler(Exception exception);
}
