using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.DataAccess.MasterData.Entities
{
    [Table("Agen")]
    public class AgenEntity : IEntity
    {
        public Decimal ID { get; set; }
        public Guid UniqueID { get; set; }
        public String AgenCode { get; set; }
        public String ReferralCode { get; set; }
        public String MerchantName { get; set; }
        public String Name { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public Guid PrivateSecret { get; set; }
        public String IDCardNo { get; set; }
        public String Email { get; set; }
        public String Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public String Phone { get; set; }
        public String WANo { get; set; }
        public String Npwp { get; set; }
        public String BankName { get; set; }
        public String BankBranch { get; set; }
        public String BankAccountNo { get; set; }
        public String BankAccountHolder { get; set; }
        public Int32? CategoryAgenID { get; set; }
        public DateTime CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public Boolean IsDeleted { get; set; }
        [Write(true)]
        [Computed]
        public object AdditionalInfo { get; set; }
    }
}
