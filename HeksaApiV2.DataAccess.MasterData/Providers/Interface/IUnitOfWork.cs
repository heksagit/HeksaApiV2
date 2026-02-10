using HeksaApiV2.DataAccess.MasterData.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Providers
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Represents the current state of the unit of work
        /// </summary>
        UnitOfWorkState State { get; }

        /// <summary>
        /// Represents the current transaction
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Commit Transaction
        /// Close Transaction.Connection
        /// Set State to IUnitOfWorkState.Comitted
        /// Dispose Transaction.Connect & Transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback Transaction
        /// Close Transaction.Connection
        /// Set State to IUnitOfWorkState.RolledBack
        /// Dispose Transaction.Connect & Transaction
        /// </summary>
        void Rollback();
    }
}
