using HeksaApiV2.DataAccess.MasterData.Entities;
using HeksaApiV2.DataAccess.MasterData.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Repositories
{
	public interface IAgenRepository : IRepository<AgenEntity, CustomSQLParam>
	{
		IEnumerable<AgenEntity> GetAll();
		AgenEntity GetById(decimal id);
	}
}
