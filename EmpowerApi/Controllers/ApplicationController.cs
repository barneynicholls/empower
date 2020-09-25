using System;
using System.Collections.Generic;
using Empower;
using Empower.Applications;
using Empower.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpowerApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpPost]
        [Route("/mpw/resource/applications")]
        public IActionResult Create([FromForm] CreateAppRequest createRequest)
        {
            if (string.IsNullOrEmpty(Request.Headers[SecurityToken.X_CSRF_TOKEN]))
                return Unauthorized();

            CreateAppResponse response = new CreateAppResponse()
            {
                Header = new Header { Status = new Status { Code = Status.SUCCESS } },
                Body = new CreateAppResponseBody
                {
                    App = new App
                    {
                        AppId = Guid.NewGuid().ToString(),
                        ApplicationName = "EditAreaTextParagraph",
                        ApplicationOI = "3",
                        ApplicationPath = string.Empty,
                        DatabaseId = Guid.NewGuid().ToString(),
                        EditorVersion = createRequest.EditorVersion ?? "1.1.0.7948",
                        EngineVersion = "900101",
                        PackageCreationDate = DateTime.Now,
                        PackageFile = "ExstreamPackage.pub",
                        PackageVersion = "900122",
                        PreviewPubFile = createRequest.PreviewPubFile,
                        RoleNames = createRequest.RoleName ?? new List<string>()
                    }
                }
            };

            return new OkObjectResult(response);
        }
    }
}
