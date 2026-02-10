using Dapper;
using Dapper.Contrib.Extensions;
using HeksaApiV2.DataAccess.MasterData.Objects;
using HeksaApiV2.DataAccess.MasterData.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace HeksaApiV2.DataAccess.MasterData.Repositories
{
    public abstract class BaseRepository<T, U> : IRepository<T, U>
        where T : class
        where U : CustomSQLParam
    {
        public string BaseTableName { get; set; }
        public string BaseDatabaseName { get; set; }

        private readonly IDbContext dbContext;

        public BaseRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private IDbTransaction Transaction =>
            dbContext.UnitOfWork.Transaction;

        /// <summary>
        /// Insert record into table in database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isExplicitKey">Explicit Keys use when table does not have primary key IDENTITY(1,1) </param>
        /// <returns></returns>
        public virtual IDbResult<T> Insert(T model, bool isExplicitKey = false)
        {
            var result = new DbResult<T>();
            try
            {
                if (model == null)
                {
                    result.SetFailed("Database error, model data cannot be null", DbResponseState.ErrorParameter);
                    return result;
                }

                long resID = dbContext.Connection.Insert<T>(entityToInsert: model, transaction: Transaction);

                if (resID > 0 && !isExplicitKey)
                {
                    result.SetSuccess("Success insert data into " + BaseTableName, model);
                    if (!dbContext.IsUseTransactionScope)
                        Transaction.Commit();
                }
                else if (resID == 0 && isExplicitKey)
                {
                    result.SetSuccess("Success insert data into " + BaseTableName, model);
                    if (!dbContext.IsUseTransactionScope)
                        Transaction.Commit();
                }
                else
                    result.SetFailed("Failed insert data into table " + BaseTableName, DbResponseState.ErrorButSuccess);
            }
            catch (SqlException ex)
            {
                result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Rollback();
            }
            catch (Exception ex)
            {
                result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Rollback();
            }
            finally
            {
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Update record into table in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IDbResult<T> Update(T model)
        {
            var result = new DbResult<T>();
            try
            {
                if (model == null)
                {
                    result.SetFailed("Database error, model data cannot be null", DbResponseState.ErrorParameter);
                    return result;
                }

                bool isUpdated = dbContext.Connection.Update<T>(entityToUpdate: model, transaction: Transaction);

                if (isUpdated)
                {
                    result.SetSuccess("Success update data from table " + BaseTableName, model);
                    if (!dbContext.IsUseTransactionScope)
                        Transaction.Commit();
                }
                else
                    result.SetFailed("Failed update data from table " + BaseTableName, DbResponseState.ErrorButSuccess);
            }
            catch (SqlException ex)
            {
                result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Rollback();
            }
            catch (Exception ex)
            {
                result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Rollback();
            }
            finally
            {
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Delete record table in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual IDbResult<T> Delete(T model)
        {
            var result = new DbResult<T>();
            try
            {
                if (model == null)
                {
                    result.SetFailed("Database error, model data cannot be null", DbResponseState.ErrorParameter);
                    return result;
                }

                bool isDeleted = dbContext.Connection.Delete<T>(entityToDelete: model, transaction: Transaction);

                if (isDeleted)
                {
                    result.SetSuccess("Success delete data from table " + BaseTableName);
                    if (!dbContext.IsUseTransactionScope)
                        Transaction.Commit();
                }
                else
                    result.SetFailed("Failed delete data from table " + BaseTableName, DbResponseState.ErrorButSuccess);
            }
            catch (SqlException ex)
            {
                result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Rollback();
            }
            catch (Exception ex)
            {
                result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                if (!dbContext.IsUseTransactionScope)
                    Transaction.Rollback();
            }
            return result;
        }

        /// <summary>
        /// Get Single record from database
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual IDbResult<T> SelectSingle(U param)
        {
            var result = new DbResult<T>();
            if (param == null)
            {
                result.SetFailed("Database error, param cannot be null", DbResponseState.ErrorParameter);
                return result;
            }

            var resultMany = SelectMany(param);
            if (resultMany.Success)
            {
                result.SetSuccess("Success get data");
                if (resultMany.Result != null && resultMany.Result.Count() > 0)
                    result.Result = resultMany.Result.FirstOrDefault();
                else
                    result.Result = null;
            }
            else
            {
                result.SetFailed(resultMany.Message, resultMany.StatusCode, resultMany.ErrorException);
                result.Result = null;
            }

            return result;
        }

        /// <summary>
        /// Get multiple record from database
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual IDbResult<IEnumerable<T>> SelectMany(U param)
        {
            var result = new DbResult<IEnumerable<T>>();
            string query = "";
            if (param != null)
            {
                try
                {
                    query = GenerateQuery(param);
                    result.Result = dbContext.Connection.Query<T>(sql: query, transaction: Transaction);

                    result.SetSuccess("Success get data");
                }
                catch (SqlException ex)
                {
                    result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                }
                catch (Exception ex)
                {
                    result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                }
            }
            else
                result.SetFailed("Database error, param cannot be null", DbResponseState.ErrorParameter);

            return result;
        }

        /// <summary>
        /// Generate query used to operate in database
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GenerateQuery(U param)
        {
            string query = "";
            if (param != null)
            {
                if (string.IsNullOrWhiteSpace(param.FullDynamicQuery))
                {
                    string queryTopBy = "";
                    if (param.TopBy > 0)
                        queryTopBy = " TOP " + param.TopBy;

                    string queryField = "*";
                    if (param.ListSelectField.Count() > 0)
                    {
                        queryField = "";
                        foreach (var str in param.ListSelectField.OrderBy(x => x))
                        {
                            if (!string.IsNullOrWhiteSpace(str))
                            {
                                if (string.IsNullOrWhiteSpace(queryField))
                                    queryField = str;
                                else
                                    queryField = queryField + ", " + str;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(queryField))
                            queryField = "*";
                    }

                    string queryWhere = "";
                    if (!string.IsNullOrWhiteSpace(param.QueryWhere))
                        queryWhere = " WHERE " + param.QueryWhere;

                    string queryOrder = "";
                    if (!string.IsNullOrWhiteSpace(param.QueryOrder))
                        queryOrder = " ORDER BY " + param.QueryOrder;

                    string queryGroupBy = "";
                    if (!string.IsNullOrWhiteSpace(param.QueryGroupBy))
                        queryGroupBy = " GROUP BY " + param.QueryGroupBy;

                    string queryFetchPage = "";
                    if (param.FetchPage > 0 && param.FetchPageSize > 0)
                        queryFetchPage = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", (param.FetchPage - 1), param.FetchPageSize);

                    query = string.Format("SELECT {0} {1} FROM {2} {3} {4} {5} {6}",
                        queryTopBy, queryField, BaseTableName, queryWhere, queryOrder, queryGroupBy, queryFetchPage);
                }
                else
                    query = param.FullDynamicQuery;
            }
            return query;
        }

        public virtual IDbResult<T> Execute(U param)
        {
            return Execute(param, false);
        }

        public virtual IDbResult<T> Execute(U param, bool IsGetData)
        {
            var result = new DbResult<T>();
            if (param != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(param.StoredProcedureName))
                    {
                        if (IsGetData)
                        {
                            var resMultiple = ExecuteGetData<T>(param);
                            if (resMultiple.Success)
                            {
                                if (resMultiple.Result != null && resMultiple.Result.Count() > 0)
                                {
                                    result.Result = resMultiple.Result.FirstOrDefault();
                                }
                                result.SetSuccess(resMultiple.Message);
                            }
                            else
                                result.SetFailed(resMultiple.Message, resMultiple.StatusCode, resMultiple.ErrorException);
                        }
                        else
                        {
                            object paramSP = ListParamSQLToDynamic(param.ListParamSQL);
                            int spResult = dbContext.Connection.Execute(sql: param.StoredProcedureName, param: paramSP, transaction: Transaction, commandType: CommandType.StoredProcedure);

                            result.AdditionalInfo = spResult;
                            result.SetSuccess("Success Execute SP " + param.StoredProcedureName);
                            if (!dbContext.IsUseTransactionScope)
                                Transaction.Commit();
                        }
                    }
                    else
                        result.SetFailed("Database error, param StoredProcedure name cannot be empty", DbResponseState.ErrorParameter);
                }
                catch (SqlException ex)
                {
                    result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                    if (!dbContext.IsUseTransactionScope)
                        Transaction.Rollback();
                }
                catch (Exception ex)
                {
                    result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                    if (!dbContext.IsUseTransactionScope)
                        Transaction.Rollback();
                }
            }
            else
                result.SetFailed("Database error, param cannot be null", DbResponseState.ErrorParameter);

            return result;
        }

        public virtual IDbResult<IEnumerable<TResult>> ExecuteGetData<TResult>(U param)
            where TResult : class
        {
            var result = new DbResult<IEnumerable<TResult>>();
            if (param != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(param.StoredProcedureName))
                    {
                        object paramSP = ListParamSQLToDynamic(param.ListParamSQL);
                        result.Result = dbContext.Connection.Query<TResult>(sql: param.StoredProcedureName, param: paramSP, transaction: Transaction, commandType: CommandType.StoredProcedure);

                        result.SetSuccess("Success Execute SP " + param.StoredProcedureName);
                    }
                    else if (!string.IsNullOrWhiteSpace(param.FullDynamicQuery))
                    {
                        result.Result = dbContext.Connection.Query<TResult>(sql: param.FullDynamicQuery, transaction: Transaction, commandType: CommandType.Text);

                        result.SetSuccess("Success Execute Dynamic Query");
                    }
                    else
                        result.SetFailed("Database error, param StoredProcedure name cannot be empty", DbResponseState.ErrorParameter);
                }
                catch (SqlException ex)
                {
                    result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                }
                catch (Exception ex)
                {
                    result.SetFailed(ex.Message, DbResponseState.InternalServerError, ex);
                }
            }
            else
                result.SetFailed("Database error, param cannot be null", DbResponseState.ErrorParameter);
            return result;
        }

        private object ListParamSQLToDynamic(IEnumerable<SqlParameter> sqlParams)
        {
            object result = null;
            if (sqlParams != null && sqlParams.Count() > 0)
            {
                Dictionary<string, object> dictParamSQL = new Dictionary<string, object>();
                foreach (var item in sqlParams)
                {
                    if (!string.IsNullOrWhiteSpace(item.ParameterName))
                    {
                        dictParamSQL.Add(item.ParameterName, item.Value);
                    }
                }
                result = DictToExpando(dictParamSQL);
            }
            return result;
        }

        public virtual IDbConnection GetConnection()
        {
            return dbContext.Connection;
        }

        public virtual IDbTransaction GetTransaction()
        {
            dbContext.IsUseTransactionScope = true;
            return Transaction;
        }

        public virtual void CommitTransaction(IDbTransaction transScope)
        {
            transScope.Commit();
            dbContext.IsUseTransactionScope = false;
        }

        public virtual void DisposeTransaction(IDbTransaction transScope)
        {
            transScope.Dispose();
            dbContext.IsUseTransactionScope = false;
        }

        /// <summary>
        /// Extension method that turns a dictionary of string and object to an ExpandoObject
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns>Dynamic Object</returns>
        private ExpandoObject DictToExpando(IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = DictToExpando((IDictionary<string, object>)kvp.Value);
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = DictToExpando((IDictionary<string, object>)item);
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }
    }
}