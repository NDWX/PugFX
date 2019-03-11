using System;

#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
	public static class IApplicationDataExtensions
	{
		//public struct ActionDefinition<A>
		//{
		//	Action<A> action;
		//}

		//public struct ActionDefinition<T, A>
		//{
		//	public Action<T, A> Action;
		//	public A Arguments;
		//}

		//public struct FunctionDefinition<T, A, R>
		//{
		//	public Func<T, A, R> Function;
		//	public A Arguments;
		//}

		/// <summary>
		/// This is a wrapper function that allows developer to perform data tasks without having to worry about 
		/// </summary>
		/// <param name="action">Action to perform when a new instance of T data session has been successfully created.</param>
		/// <param name="transactionScopeOption">Specifies requirement and the scope of transaction.</param>
		/// <param name="onError">Action to perform when an error occured prior to completion of <paramref name="action"/>, this includes when error occured during creation of T data session instance.</param>
		/// <param name="errorHandler">Specifies if and how an error is to be handled. Corresponding error would be thrown by the function if this parameter is null.</param>
		/// <param name="onSuccess">Action to perform upon successful completion of <paramref name="action"/>. Created instance of T data session would have already been disposed at this stage.</param>
		/// <param name="onFinished">Action to perform upon completion of <paramref name="action"/> regardless of whether it was successful.</param>
		public static void Perform<T, C>(
			this IApplicationData<T> applicationData, 
			Action<T, C> action, 
			C context,
#if NETFX
			TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
#else
			bool transactionRequired = true, 
#endif
			Action<Exception, C> onError = null,
			Action<Exception, C> errorHandler = null, 
			Action<C> onSuccess = null, 
			Action<C> onFinished = null
		)		
			where T : class, IApplicationDataSession
		{
			T dataSession = null;
			bool successful = true;

			try
			{
				dataSession = applicationData.GetSession();

#if NETFX
				using (TransactionScope tx = new TransactionScope(transactionScopeOption))
				{
					action(dataSession, context);

					tx.Complete();
				}
#else
				if (transactionRequired)
				{
					dataSession.BeginTransaction();

					action(dataSession, context);
				
					dataSession.CommitTransaction();
				}
				else
				{
					action(dataSession, context);
				}
#endif
			}
			catch (Exception exception)
			{
#if !NETFX
				if (transactionRequired)
					dataSession.RollbackTransaction();
#endif

				successful = false;

				if (onError != null)
					onError(exception, context);

				if (errorHandler == null)
					throw;

				errorHandler(exception, context);
			}
			finally
			{
				if (dataSession != null)
					try
					{
						dataSession.Dispose();
					}
					catch (Exception finalException)
					{
						// todo: log error?
					}

				if (successful && onSuccess != null)
					onSuccess(context);

				if (onFinished != null)
					onFinished(context);
			}
		}

		public static R Execute<T, C, R>(
			this IApplicationData<T> applicationData, 
			Func<T, C, R> function, 
			C context,
#if NETFX
			TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
#else
			bool transactionRequired = true, 
#endif
			Action<Exception, C> onError = null, 
			Action<Exception, C> errorHandler = null, 
			Action<C> onSuccess = null, 
			Action<C> onFinished = null
		)
			where T : class, IApplicationDataSession
		{
			T dataSession = null;
			bool successful = true;
			R result = default(R);

			try
			{
				dataSession = applicationData.GetSession();				
#if NETFX
				using (TransactionScope tx = new TransactionScope(transactionScopeOption))
				{
					result = function(dataSession, context);

					tx.Complete();
				}
#else
				if (transactionRequired)
				{
					dataSession.BeginTransaction();

					result = function(dataSession, context);
				
					dataSession.CommitTransaction();
				}
				else
				{
					result = function(dataSession, context);
				}
#endif
			}
			catch (Exception exception)
			{
#if !NETFX
				if (transactionRequired)
					dataSession.RollbackTransaction();
#endif

				successful = false;

				if (onError != null)
					onError(exception, context);

				if (errorHandler == null)
					throw;

				errorHandler(exception, context);
			}
			finally
			{
				if (dataSession != null)
					try
					{
						dataSession.Dispose();
					}
					catch (Exception finalException)
					{
						// todo: log error?
					}

				if (successful && onSuccess != null)
					onSuccess(context);

				if (onFinished != null)
					onFinished(context);
			}

			return result;
		}

		[Obsolete]
		public static R Call<T, C, R>(
			this IApplicationData<T> applicationData,
			Func<T, C, R> function,
			C context,
#if NETFX
			TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
#else
			bool transactionRequired = true,
#endif
			Action<Exception, C> onError = null,
			Action<Exception, C> errorHandler = null,
			Action<C> onSuccess = null,
			Action<C> onFinished = null
		)
			where T : class, IApplicationDataSession
		{
#if NETFX
			return Execute( applicationData, function, context, transactionScopeOption, onError, errorHandler, onSuccess, onFinished );
#else
			return Execute( applicationData, function, context, transactionRequired, onError, errorHandler, onSuccess, onFinished );
#endif
		}
	}

	}
