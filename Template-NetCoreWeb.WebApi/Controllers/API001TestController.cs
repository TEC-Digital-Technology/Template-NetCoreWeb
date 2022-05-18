﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template_NetCoreWeb.Utils.Enums;
using Template_NetCoreWeb.WebApi.Exceptions;
using Template_NetCoreWeb.WebApi.Models.Request.API001Test;
using Template_NetCoreWeb.WebApi.Models.Response.API001Test;

namespace Template_NetCoreWeb.WebApi.Controllers
{
    /// <summary>
    /// API-001 測試控制器
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class API001TestController : ControllerBase
    {
        /// <summary>
        /// 取得公司名稱
        /// </summary>
        /// <param name="getCompanyNameRequest">取得公司名稱的請求</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetCompanyNameResponse> GetCompanyName([FromBody] GetCompanyNameRequest getCompanyNameRequest)
        {
            //throw new OperationFailedException(ResultCodeSettingEnum.UnrecognizedLanguage);
            return await Task.FromResult(new GetCompanyNameResponse()
            {
                ComapnyName = "TEC Digital Technology Inc."
            });
        }
        /// <summary>
        /// 輸入指定的資訊，回傳處理後的個人資訊
        /// </summary>
        /// <param name="getMyInformationRequest">輸入資訊的請求</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<GetMyInformationResponse> GetMyInformation([FromBody] GetMyInformationRequest getMyInformationRequest)
        {
            return await Task.FromResult(new GetMyInformationResponse()
            {
                YourName = getMyInformationRequest.Name,
                Banks = String.Join(",", getMyInformationRequest.BankNames!)
            });
        }

    }
}
