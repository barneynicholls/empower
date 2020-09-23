﻿using System;
using Empower;
using Empower.Security;
using Microsoft.AspNetCore.Mvc;

namespace EmpowerApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpGet]
        [Route("/mpw/resource/GetToken")]
        public SecurityTokenResponse GetToken()
        {
            return new SecurityTokenResponse
            {
                Header = new Header
                {
                    Status = new Status
                    {
                        Code = Status.SUCCESS
                    }
                },
                Body = new SecurityBody
                {
                    CsrfHeader = SecurityTokenResponse.X_CSRF_TOKEN,
                    CsrfParameter = "_csrf",
                    CsrfToken = Guid.NewGuid().ToString()
                }
            };
        }
    }
}