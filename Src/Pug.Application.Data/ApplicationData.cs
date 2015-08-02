using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

using System.Transactions;

namespace Pug.Application.Data
{
	public abstract class ApplicationData<T>
		: IApplicationData<T>
		where T : class, IApplicationDataSession
	{
		string location;
		IDataAccessProvider dataProvider;

		protected ApplicationData(string location, IDataAccessProvider dataProvider)
		{
			this.location = location;
			this.dataProvider = dataProvider;
		}

		protected string Location
		{
			get
			{
				return this.location;
			}
		}

		protected IDataAccessProvider DataAccessProvider
		{
			get
			{
				return this.dataProvider;
			}
		}

		public abstract T GetSession();

		/// <summary>
		/// This is a wrapper function that allows developer to perform data tasks without having to worry about 
		/// </summary>
		/// <param name="action">Action to perform when a new instance of T data session has been successfully created.</param>
		/// <param name="transactionScopeOption">Specifies requirement and the scope of transaction.</param>
		/// <param name="onError">Action to perform when an error occured prior to completion of <paramref name="action"/>, this includes when error occured during creation of T data session instance.</param>
		/// <param name="errorHandler">Specifies if and how an error is to be handled. Corresponding error would be thrown by the function if this parameter is null.</param>
		/// <param name="onSuccess">Action to perform upon successful completion of <paramref name="action"/>. Created instance of T data session would have already been disposed at this stage.</param>
		/// <param name="onFinished">Action to perform upon completion of <paramref name="action"/> regardless of whether it was successful.</param>
		//public void Perform(Action<T> action, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required, Action<Exception> onError = null, Action<Exception> errorHandler = null, Action onSuccess = null, Action onFinished = null)
		//{
		//	T dataSession = null;
		//	bool successful = true;

		//	try
		//	{
		//		dataSession = this.GetSession();

		//		using (TransactionScope tx = new TransactionScope(transactionScopeOption))
		//		{
		//			action(dataSession);

		//			tx.Complete();
		//		}
		//	}
		//	catch (Exception exception)
		//	{
		//		successful = false;

		//		if (onError != null)
		//			onError(exception);

		//		if (errorHandler == null)
		//			throw;

		//		errorHandler(exception);
		//	}
		//	finally
		//	{
		//		if (dataSession != null)
		//			try
		//			{
		//				dataSession.Dispose();
		//			}
		//			catch (Exception finalException)
		//			{
		//				// todo: log error?
		//			}

		//		if (successful && onSuccess != null)
		//			onSuccess();

		//		if (onFinished != null)
		//			onFinished();
		//	}
		//}
	}
}
