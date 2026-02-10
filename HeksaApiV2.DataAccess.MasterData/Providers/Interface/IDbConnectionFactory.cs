using System.Data;

namespace HeksaApiV2.DataAccess.MasterData.Providers
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateOpenConnection();
    }
}