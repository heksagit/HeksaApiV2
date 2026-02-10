using HeksaApiV2.DataAccess.MasterData.Entities;
using HeksaApiV2.DataAccess.MasterData.Helpers;
using HeksaApiV2.DataAccess.MasterData.Objects;
using HeksaApiV2.DataAccess.MasterData.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Repositories
{
	public class ListStoredProcedureRepository : BaseRepository<ListStoredProcedureEntity, CustomSQLParam>, IListStoredProcedureRepository
	{
		private readonly IDbContext dbContext;

		public ListStoredProcedureRepository(IDbContext _dbContext) : base(_dbContext)
		{
			this.dbContext = _dbContext;
			this.BaseTableName = "ListStoredProcedure";
		}

		public IEnumerable<ListStoredProcedureEntity> GetAll()
		{
			CustomSQLParam param = new CustomSQLParam();
			param.QueryOrder = "ID ASC";
			var dataResult = SelectMany(param);
			return dataResult.Result;
		}

		public ListStoredProcedureEntity GetById(decimal id)
		{
			CustomSQLParam param = new CustomSQLParam();
			param.QueryWhere = "ID = " + id;
			var dataResult = SelectSingle(param);
			return dataResult.Result;
		}

		public SP_GetDataMasterAgenCore_Result GetDataAgenFromCoreSystem(string agenCode)
		{
			CustomSQLParam param = new CustomSQLParam();
			param.StoredProcedureName = "SP_GetDataMasterAgenCore";
			param.ListParamSQL = new List<SqlParameter>() {
				new SqlParameter() { ParameterName = "agenCode", Value = SqlInjectionHelper.ValidateSqlValue(agenCode) }
			};
			var dataResult = ExecuteGetData<SP_GetDataMasterAgenCore_Result>(param).Result.FirstOrDefault();
			return dataResult;
		}

		public AgenMasterDataObj GetDataAgenFromDBMasterData(string agenCode, string categoryAgen)
        {
			CustomSQLParam param = new CustomSQLParam();
			param.FullDynamicQuery = $"SELECT A.[AgenCode] AS [KodeAgen], A.[ReferralCode] AS [ReferralCode], A.[Name] AS [NamaAgen], CA.[Code] AS [CategoryAgenCode], CA.[Name] AS [CategoryAgen] " +
									 $" FROM [HEKSAMasterData].[dbo].[Agen] A " + 
									 $" LEFT JOIN [HEKSAMasterData].[dbo].[CategoryAgen] CA ON CA.[ID] = A.[CategoryAgenID] " + 
									 $" WHERE A.[IsDeleted] = 0 AND A.[AgenCode] = '{SqlInjectionHelper.ValidateSqlValue(agenCode)}'";
			var dataResult = ExecuteGetData<AgenMasterDataObj>(param).Result.FirstOrDefault();
			return dataResult;
        }
	}

}
