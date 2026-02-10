using HeksaApiV2.DataAccess.MasterData.Objects;
using System.Collections.Generic;
using System.Data;

namespace HeksaApiV2.DataAccess.MasterData.Repositories
{
    public interface IRepository<T, U>
        where T : class
        where U : class
    {
        string BaseTableName { get; set; }
        string BaseDatabaseName { get; set; }

        IDbResult<T> Insert(T model, bool isExplicitKey = false);

        IDbResult<T> Update(T model);

        IDbResult<T> Delete(T model);

        IDbResult<T> SelectSingle(U param);

        IDbResult<IEnumerable<T>> SelectMany(U param);

        IDbResult<T> Execute(U param);

        IDbTransaction GetTransaction();

        void CommitTransaction(IDbTransaction transScope);

        void DisposeTransaction(IDbTransaction transScope);
    }
}