﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Danliris.Service.Inventory.Lib.Facades;
using Com.Danliris.Service.Inventory.Lib.ViewModels.FPReturnInvToPurchasingViewModel;
using Com.Danliris.Service.Inventory.Lib.Services;
using System.ComponentModel.DataAnnotations;
using Com.Moonlay.NetCore.Lib.Service;
using Com.Danliris.Service.Inventory.WebApi.Helpers;
using Com.Danliris.Service.Inventory.Lib.Models.FPReturnInvToPurchasingModel;

namespace Com.Danliris.Service.Inventory.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/fp-return-inv-to-purchasings")]
    [Authorize]
    public class FPReturnInvToPurchasingController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly FPReturnInvToPurchasingFacade fpReturnInvToPurchasingFacade;
        private readonly IdentityService identityService;

        public FPReturnInvToPurchasingController(FPReturnInvToPurchasingFacade fpReturnInvToPurchasingFacade, IdentityService identityService)
        {
            this.fpReturnInvToPurchasingFacade = fpReturnInvToPurchasingFacade;
            this.identityService = identityService;
        }

        [HttpGet]
        public ActionResult Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            return new BaseGet<ListFPReturnInvToPurchasingViewModel, FPReturnInvToPurchasingFacade>(fpReturnInvToPurchasingFacade)
                .Get(page, size, order, keyword, filter);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            return await new BaseGetById<FPReturnInvToPurchasing, FPReturnInvToPurchasingViewModel, FPReturnInvToPurchasingFacade>(fpReturnInvToPurchasingFacade, ApiVersion)
                .GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FPReturnInvToPurchasingViewModel viewModel)
        {
            this.identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");
            this.identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

            ValidateService validateService = (ValidateService)fpReturnInvToPurchasingFacade.serviceProvider.GetService(typeof(ValidateService));
            return await new BasePost<FPReturnInvToPurchasing, FPReturnInvToPurchasingViewModel, FPReturnInvToPurchasingFacade>(fpReturnInvToPurchasingFacade, ApiVersion, validateService, Request.Path)
                .Post(viewModel);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
            this.identityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

            return await new BaseDelete<FPReturnInvToPurchasingFacade>(fpReturnInvToPurchasingFacade, ApiVersion)
                .Delete(id);
        }
    }
}