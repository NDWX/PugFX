
using System;
using System.Collections.Generic;

using Pug.Application;
using Pug.Application.Data;

namespace Pug.Application.ServiceModel
{
    public interface IApplicationTransaction<DS> : IDisposable
        where DS : class, IApplicationDataSession
    {
        void Commit();

        void Rollback();
    }

    internal class ApplicationTransaction<DS> : IApplicationTransaction<DS>
         where DS : class, IApplicationDataSession
    {
        bool transactionEnded = false;

        public DS DataSession { get; }

        public ApplicationTransaction(DS dataSession)
        {
            this.DataSession = dataSession;
        }

        public void Commit()
        {
            try
            {
                DataSession.CommitTransaction();
            }
            catch(Exception e)
            {
                throw;
            }
            finally
            {
                transactionEnded = true;
            }
        }

        public void Dispose()
        {
            if (!transactionEnded)
                DataSession.RollbackTransaction();
        }

        public void Rollback()
        {
            try
            {
                DataSession.RollbackTransaction();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                transactionEnded = true;
            }
        }
    }

    public abstract class ApplicationService<DS> where DS : class, IApplicationDataSession
	{
		IApplicationData<DS> applicationDataProvider;
        IUserSessionProvider sessionProvider;

		protected ApplicationService( IApplicationData<DS> applicationDataProvider, IUserSessionProvider sessionProvider )
		{
            this.applicationDataProvider = applicationDataProvider;
            this.sessionProvider = sessionProvider;
		}

		protected IApplicationData<DS> DataProvider => this.applicationDataProvider;

        void setCurrentTransaction(IApplicationTransaction<DS> transaction)
        {
            IUserSession userSession = sessionProvider.CurrentSession;

            userSession.Set("CURRENT", transaction);
        }

        void Register(ApplicationTransaction<DS> transaction)
        {
            IUserSession userSession = sessionProvider.CurrentSession;

            userSession.Ending += UserSession_Ending;
            
            List<ApplicationTransaction<DS>> userTransactions = userSession.Get<List<ApplicationTransaction<DS>>>();

            if (userTransactions == null)
            {
                userTransactions = new List<ApplicationTransaction<DS>>();
                userSession.Set(string.Empty, userTransactions);
            }

            setCurrentTransaction(transaction);

            userTransactions.Add(transaction);
        }

        private void UserSession_Ending(object sender, EventArgs e)
        {
            IUserSession userSession = sessionProvider.CurrentSession;

            List<ApplicationTransaction<DS>> userTransactions = userSession.Get<List<ApplicationTransaction<DS>>>();

            foreach (ApplicationTransaction<DS> transaction in userTransactions)
                transaction.Dispose();
        }

        public IApplicationTransaction<DS> CurrentTransaction
        {
            get
            {
                IUserSession userSession = sessionProvider.CurrentSession;

                return userSession.Get<ApplicationTransaction<DS>>("CURRENT");
            }
            set
            {
                if( value is ApplicationTransaction<DS> )
                {
                    setCurrentTransaction(value);

                    return;
                }

                throw new InvalidOperationException("Specified value is not a valid IApplicationTransaction<DS> implementation.");
            }
        }

        protected DS GetDataSession()
        {
            if (CurrentTransaction != null)
                return ((ApplicationTransaction<DS>)CurrentTransaction).DataSession;

            return applicationDataProvider.GetSession();
        }

        public IApplicationTransaction<DS> BeginTransaction()
        {
            DS dataSession = applicationDataProvider.GetSession();

            dataSession.BeginTransaction();

            ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(dataSession);

            Register(transaction);

            return transaction;
        }
	}
}
