using HeksaApiV2.Common.Object;

using DBMasterRepo = HeksaApiV2.DataAccess.MasterData.Repositories;
using DBMasterHelp = HeksaApiV2.DataAccess.MasterData.Helpers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using HeksaApiV2.Model.Common;
using HeksaApiV2.Model.API.Response;
using HeksaApiV2.Common.Extensions;
using HeksaApiV2.Model.Enum;

namespace HeksaApiV2.Logic
{
    public interface IAgenCoreLogic
    {
        IResult<ReferralHeksaStoreModel> GetDataReferensiKodeAgen(string kodeAgen, string categoryAgen = "");
    }


    public class AgenCoreLogic : IAgenCoreLogic
    {
        private GlobalSettings _configs;

        private DBMasterRepo.IListStoredProcedureRepository _listStoredProcedureRepo;

        public AgenCoreLogic(IConfiguration _iconfig,
            IWebHostEnvironment _ienv,
            DBMasterRepo.IListStoredProcedureRepository _ilistStoredProcedureRepo)
        {
            _configs = new GlobalSettings(_iconfig);

            _listStoredProcedureRepo = _ilistStoredProcedureRepo;
        }

        public IResult<ReferralHeksaStoreModel> GetDataReferensiKodeAgen(string kodeAgen, string categoryAgen = "")
        {
            IResult<ReferralHeksaStoreModel> result = new ResultModel<ReferralHeksaStoreModel>();

            if (!DBMasterHelp.SqlInjectionHelper.IsValidateSqlValue(kodeAgen))
                return result.ReturnFailed("Invalid kode parameter", ResponseCode.BadRequest);

            if (!DBMasterHelp.SqlInjectionHelper.IsValidateSqlValue(categoryAgen))
                return result.ReturnFailed("Invalid category parameter", ResponseCode.BadRequest);

            if(string.IsNullOrWhiteSpace(categoryAgen))
            {
                var dataRes = GetDataAgenCoreSys(kodeAgen);
                if (dataRes.Success)
                    return dataRes;
                else
                    result.SetFailed(dataRes.Message, dataRes.StatusCode);
            }
            else
            {
                var dataRes = GetDataAgenMaster(kodeAgen, categoryAgen);
                if (dataRes.Success)
                    return dataRes;
                else
                    result.SetFailed(dataRes.Message, dataRes.StatusCode);
            }

            return result;
        }

        private IResult<ReferralHeksaStoreModel> GetDataAgenCoreSys(string kodeAgen)
        {
            IResult<ReferralHeksaStoreModel> result = new ResultModel<ReferralHeksaStoreModel>();
            ReferralHeksaStoreModel model = new ReferralHeksaStoreModel();

            if (!DBMasterHelp.SqlInjectionHelper.IsValidateSqlValue(kodeAgen))
                return result.ReturnFailed("Invalid parameter", ResponseCode.ErrorParameter);

            var dtAgen = _listStoredProcedureRepo.GetDataAgenFromCoreSystem(kodeAgen);

            if (dtAgen == null || string.IsNullOrEmpty(dtAgen.AgenId))
                return result.ReturnFailed("Data referensi tidak ditemukan pada sistem", ResponseCode.ErrorParameter);

            if (dtAgen.StatusAgen.TrimNull().ToLower() == "terminated")
                return result.ReturnFailed("Data agen sudah tidak aktif", ResponseCode.ErrorButSuccess);

            if (!dtAgen.TanggalKadaluarsaAAJI.HasValue)
                return result.ReturnFailed("Lisensi data agen tidak valid", ResponseCode.ErrorButSuccess);

            if (dtAgen.TanggalKadaluarsaAAJI.Value.Date < DateTime.Now.Date)
                return result.ReturnFailed("Lisensi data agen telah kadaluarsa", ResponseCode.ErrorButSuccess);

            if (dtAgen.IDPemasaran != "01" && dtAgen.IDPemasaran != "02")
                return result.ReturnFailed("Jalur distribusi agen tidak valid", ResponseCode.ErrorButSuccess);

            model.RefCode = dtAgen.AgenCode;
            model.RefName = dtAgen.AgenName;
            model.RefType = dtAgen.AgenPemasaran;

            result.SetSuccess("success", model);

            return result;
        }

        private IResult<ReferralHeksaStoreModel> GetDataAgenMaster(string kodeAgen, string categoryAgen = "")
        {
            IResult<ReferralHeksaStoreModel> result = new ResultModel<ReferralHeksaStoreModel>();
            ReferralHeksaStoreModel model = new ReferralHeksaStoreModel();

            if (!DBMasterHelp.SqlInjectionHelper.IsValidateSqlValue(kodeAgen))
                return result.ReturnFailed("Invalid parameter", ResponseCode.ErrorParameter);

            var dtAgen = _listStoredProcedureRepo.GetDataAgenFromDBMasterData(kodeAgen, categoryAgen);
            if(dtAgen == null || string.IsNullOrWhiteSpace(dtAgen.KodeAgen))
                return result.ReturnFailed("Data referensi tidak ditemukan pada sistem", ResponseCode.ErrorParameter);

            model.RefCode = dtAgen.KodeAgen;
            model.RefName = dtAgen.NamaAgen;
            model.RefType = dtAgen.CategoryAgen;

            result.SetSuccess("success", model);

            return result;
        }

        private IResult<ReferralHeksaStoreModel> GetDataReferensiPoint(string kodeAgen)
        {
            IResult<ReferralHeksaStoreModel> result = new ResultModel<ReferralHeksaStoreModel>();

            if (!DBMasterHelp.SqlInjectionHelper.IsValidateSqlValue(kodeAgen))
                return result.ReturnFailed("Invalid parameter", ResponseCode.ErrorParameter);



            return result;
        }
    }
}
