using HeksaApiV2.DataAccess.MasterData.Objects;
using System;
using System.Data;
using System.Transactions; // Required for TransactionScope

namespace HeksaApiV2.DataAccess.MasterData.Providers
{
    public class DbContext : IDbContext, IDisposable
    {
        private readonly IDbConnectionFactory connectionFactory;
        private IDbConnection connection;
        private IDbTransaction transaction;
        private IUnitOfWork unitOfWork;
        private bool disposed = false;

        public DbContext(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public DbContextState State { get; private set; } = DbContextState.Closed;
        public bool IsUseTransactionScope { get; set; } = false; // Now properly handled

        // Lazy connection management
        public IDbConnection Connection =>
          connection ?? (connection = OpenConnection());

        public IDbTransaction Transaction
        {
            get
            {
                if (transaction == null || transaction.Connection == null)
                {
                    // Ensure the connection is open before starting a transaction
                    if (Connection.State != ConnectionState.Open)
                    {
                        Connection.Open();
                    }

                    transaction = Connection.BeginTransaction();
                }

                return transaction;
            }
        }

        // UnitOfWork implementation
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (unitOfWork == null || unitOfWork.Transaction?.Connection == null)
                {
                    // Recreate UnitOfWork if the previous transaction is disposed
                    unitOfWork = new UnitOfWork(Transaction);
                }
                return unitOfWork;
            }
            set { unitOfWork = value; }
        }

        // Begins a transaction, respecting the TransactionScope if set
        private IDbTransaction BeginTransaction()
        {
            if (IsUseTransactionScope)
            {
                // When using TransactionScope, don't create a new transaction.
                return null;
            }
            return Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            try
            {
                if (!IsUseTransactionScope)
                {
                    UnitOfWork.Commit();
                }
                State = DbContextState.Committed;
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                Reset();
            }
        }

        public void Rollback()
        {
            try
            {
                if (!IsUseTransactionScope)
                {
                    UnitOfWork.Rollback();
                }
                State = DbContextState.RolledBack;
            }
            finally
            {
                Reset();
            }
        }

        private IDbConnection OpenConnection()
        {
            if (connection == null)
            {
                connection = connectionFactory.CreateOpenConnection();
                State = DbContextState.Open;
            }
            return connection;
        }

        private void Reset()
        {
            // Close and dispose connection/transaction only if not using TransactionScope
            if (!IsUseTransactionScope)
            {
                transaction?.Dispose();
                connection?.Close();
                connection?.Dispose();
            }

            // Reset fields to null
            connection = null;
            transaction = null;
            unitOfWork = null;
        }

        // Dispose pattern for resource cleanup
        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Reset();
            }
            disposed = true;
        }

        ~DbContext()
        {
            Dispose(false);
        }
    }
}
