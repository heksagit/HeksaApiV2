using System;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public class SingleResult
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public string StringValue { get; set; }
        public Decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public bool BoolValue { get; set; }
    }
}