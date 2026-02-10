using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Objects
{
    public class SP_GetDataMasterAgenCore_Result
    {
        public string AgenId { get; set; }
        public string AgenCode { get; set; }
        public string AgenName { get; set; }
        public string AgenLevel { get; set; }
        public DateTime? AgenJoinDate { get; set; }
        public string KodeCabang { get; set; }
        public string AgenCabang { get; set; }
        public string AgenRegional { get; set; }
        public string AgenKodeJenjang { get; set; }
        public string AgenJenjang { get; set; }
        public string BirthPlaceAgen { get; set; }
        public DateTime? BirthDateAgen { get; set; }
        public string GenderAgen { get; set; }
        public string PhoneAgen { get; set; }
        public string EmailAgen { get; set; }
        public string AddressAgen { get; set; }
        public string StatusAgen { get; set; }
        public string IDPemasaran { get; set; }
        public string AgenPemasaran { get; set; }
        public bool IsPasswordDefault { get; set; }
        public string NoLisensi { get; set; }
        public DateTime? TanggalKadaluarsaAAJI { get; set; }
        public string Up1Name { get; set; }
        public string Up1AgenCode { get; set; }
        public string Up1Level { get; set; }
        public string Up2Name { get; set; }
        public string Up2AgenCode { get; set; }
        public string Up2Level { get; set; }
        public string Up3Name { get; set; }
        public string Up3AgenCode { get; set; }
        public string Up3Level { get; set; }
        public string Up4Name { get; set; }
        public string Up4AgenCode { get; set; }
        public string Up4Level { get; set; }
    }
}
