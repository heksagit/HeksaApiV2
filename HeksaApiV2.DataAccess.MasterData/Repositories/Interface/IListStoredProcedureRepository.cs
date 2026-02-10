using HeksaApiV2.DataAccess.MasterData.Entities;
using HeksaApiV2.DataAccess.MasterData.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Repositories
{
	public interface IListStoredProcedureRepository : IRepository<ListStoredProcedureEntity, CustomSQLParam>
	{
		IEnumerable<ListStoredProcedureEntity> GetAll();
		ListStoredProcedureEntity GetById(decimal id);
		SP_GetDataMasterAgenCore_Result GetDataAgenFromCoreSystem(string agenCode);
		AgenMasterDataObj GetDataAgenFromDBMasterData(string agenCode, string categoryAgen);
	}
}
