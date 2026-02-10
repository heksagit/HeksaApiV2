using Dapper.Contrib.Extensions;
using System;

namespace HeksaApiV2.DataAccess.MasterData.Entities
{
    [Table("ListStoredProcedure")]
    public class ListStoredProcedureEntity : IEntity
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }

        [Write(true)]
        [Computed]
        public object AdditionalInfo { get; set; }
    }
}