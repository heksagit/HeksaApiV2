using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public interface ISQLParam
    {
        int TopBy { get; set; }
        string QueryWhere { get; set; }
        string QueryOrder { get; set; }
        string QueryGroupBy { get; set; }
        int FetchPage { get; set; }
        int FetchPageSize { get; set; }
        string FullDynamicQuery { get; set; }
        string StoredProcedureName { get; set; }
        IEnumerable<SqlParameter> ListParamSQL { get; set; }
        IEnumerable<string> ListSelectField { get; set; }
    }
}
