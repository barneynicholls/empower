using System;
using Empower;
using Empower.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpowerApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpGet]
        [Route("/mpw/resource/GetToken")]
        public SecurityToken GetToken()
        {
            return new SecurityToken
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
                    CsrfHeader = SecurityToken.X_CSRF_TOKEN,
                    CsrfParameter = "_csrf",
                    CsrfToken = Guid.NewGuid().ToString()
                }
            };
        }
    }
}
