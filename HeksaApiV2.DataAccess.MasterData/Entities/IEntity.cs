using Dapper.Contrib.Extensions;

namespace HeksaApiV2.DataAccess.MasterData.Entities
{
    public interface IEntity
    {
        [Write(true)]
        [Computed]
        object AdditionalInfo { get; set; }
    }
}