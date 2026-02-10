using DBMasterHelp = HeksaApiV2.DataAccess.MasterData.Helpers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeksaApiV2.Model.Common;
using HeksaApiV2.Model.API.Response;
using HeksaApiV2.Common.Object;
using HeksaApiV2.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using HeksaApiV2.Logic;
using HeksaApiV2.Model.Enum;

namespace HeksaApiV2.Controllers
{
    [ApiController]
    public class AgenController : ControllerBase
    {
        private GlobalSettings _configs;
        private RestAPIHelper _apiHelper;
        private IAgenCoreLogic _agenCoreLogic;

        public AgenController(IConfiguration _iconfig,
            IServiceProvider _iserviceProvider,
            IHttpContextAccessor _ihttpAccess, 
            IAgenCoreLogic _iagenCoreLogic)
        {
            _configs = new GlobalSettings(_iconfig);
            _apiHelper = new RestAPIHelper(_iconfig);
            _agenCoreLogic = _iagenCoreLogic;
        }

        [HttpPost, Route("api/agen/getdateref")]
        public IActionResult GetDataReferralHeksaStore(string kodeAgen, string kategoriAgen)
        {
            IResult<ReferralHeksaStoreModel> result = new ResultModel<ReferralHeksaStoreModel>();

            result = _agenCoreLogic.GetDataReferensiKodeAgen(kodeAgen, kategoriAgen);

            if (result.StatusCode == ResponseCode.BadRequest)
                return BadRequest("Invalid Parameter");

            return Ok(result);
        }
    }
}
