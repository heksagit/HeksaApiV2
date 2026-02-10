using HeksaApiV2.DataAccess.MasterData.Entities;
using HeksaApiV2.DataAccess.MasterData.Objects;
using HeksaApiV2.DataAccess.MasterData.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Repositories
{
	public class AgenRepository : BaseRepository<AgenEntity, CustomSQLParam>, IAgenRepository
	{
		private readonly IDbContext dbContext;

		public AgenRepository(IDbContext _dbContext) : base(_dbContext)
		{
			this.dbContext = _dbContext;
			this.BaseTableName = "Agen";
		}

		public IEnumerable<AgenEntity> GetAll()
		{
			CustomSQLParam param = new CustomSQLParam();
			param.QueryOrder = "ID ASC";
			var dataResult = SelectMany(param);
			return dataResult.Result;
		}

		public AgenEntity GetById(decimal id)
		{
			CustomSQLParam param = new CustomSQLParam();
			param.QueryWhere = "ID = " + id;
			var dataResult = SelectSingle(param);
			return dataResult.Result;
		}

		public AgenEntity GetByKodeAgen(string kodeAgen)
        {
			CustomSQLParam param = new CustomSQLParam();

			param.FullDynamicQuery = $"SELECT A.* FROM [HEKSAMasterData].[dbo].[Agen] A " + 
									 $"INNER JOIN [HEKSAMasterData].[dbo].[CategoryAgen] CA ON CA.ID = A.CategoryAgenID " +
									 $"WHERE A.AgenCode = '{kodeAgen}' AND A.IsDeleted = 0";

			var dataResult = SelectSingle(param);
			return dataResult.Result;
		}
	}
}
