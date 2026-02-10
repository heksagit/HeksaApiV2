using HeksaApiV2.DataAccess.MasterData.Objects;
using System;
using System.Data;

namespace HeksaApiV2.DataAccess.MasterData.Providers
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDbTransaction transaction)
        {
            State = UnitOfWorkState.Open;
            Transaction = transaction;
        }

        public UnitOfWorkState State { get; private set; }

        public IDbTransaction Transaction { get; private set; }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
                State = UnitOfWorkState.Comitted;
            }
            catch (Exception)
            {
                Transaction.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            Transaction.Rollback();
            State = UnitOfWorkState.RolledBack;
        }
    }
}