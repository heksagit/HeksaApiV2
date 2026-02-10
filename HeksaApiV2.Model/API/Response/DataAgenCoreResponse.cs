using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Model.API.Response
{
    public class DataAgenCoreResponse
    {
        public string KodeAgen { get; set; }
        public string NamaAgen { get; set; }
        public string KodeASM { get; set; }
        public string NamaASM { get; set; }
        public string KodeCabang { get; set; }
        public string NamaCabang { get; set; }
        public string NoLisensi { get; set; }
        public string RegionalName { get; set; }
        public string TanggalKadaluarsaAAJI { get; set; }
        public bool IsPasswordDefault { get; set; }
        public int Status { get; set; }
        public string IDPemasaran { get; set; }
    }
}
