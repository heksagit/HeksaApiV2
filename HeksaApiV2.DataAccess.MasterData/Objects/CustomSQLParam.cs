using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public class CustomSQLParam : ISQLParam
    {
        public CustomSQLParam()
        {
            ListSelectField = new List<string>();
            ListParamSQL = new List<SqlParameter>();
        }

        public IEnumerable<string> ListSelectField { get; set; }
        public int TopBy { get; set; }
        public string QueryWhere { get; set; }
        public string QueryOrder { get; set; }
        public string QueryGroupBy { get; set; }
        public int FetchPage { get; set; }
        public int FetchPageSize { get; set; }
        public string FullDynamicQuery { get; set; }
        public IEnumerable<SqlParameter> ListParamSQL { get; set; }
        public string StoredProcedureName { get; set; }
    }
}
