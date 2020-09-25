using System;
using System.IO;
using System.Net.Mime;
using Empower;
using Empower.Document;
using Empower.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace EmpowerApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IWebHostEnvironment host;
        public DocumentsController(IWebHostEnvironment hostingEnvironment)
        {
            host = hostingEnvironment;
        }

        [HttpPost]
        [Route("/mpw/resource/documents/import")]
        public IActionResult Import([FromForm] ImportRequest importRequest)
        {
            if (SecurityTokenMissing())
                return Unauthorized();

            var response = new ImportResponse
            {
                Header = new Header
                {
                    Status = new Status { Code = Status.SUCCESS }
                },
                Body = new DocumentBody
                {
                    Document = new Document
                    {
                        AppId = importRequest.AppId,
                        ApplicationName = "EditAreaTextParagraph",
                        BusDocId = importRequest.BusDocId,
                        CreationDate = DateTime.Now,
                        Deleted = false,
                        DocId = Guid.NewGuid().ToString(),
                        DocTags = importRequest.DocTag,
                        DocumentVersion = "400101",
                        EditorVersion = "1.1.0.7948",
                        EngineVersion = "900101",
                        FileName = "EditAreaTextParagraph.mpw",
                        ImportDate = DateTime.Now,
                        LastSaveDate = DateTime.Now,
                        PackageFileName = "ExstreamPackage.pub",
                        PackageVersion = "900122",
                        PreviewPubFile = "PdfPreview.pub",
                        UserId = importRequest.OwnerId
                    }
                }
            };

            return new OkObjectResult(response);
        }

        private bool SecurityTokenMissing()
        {
            return string.IsNullOrEmpty(Request.Headers[SecurityToken.X_CSRF_TOKEN]);
        }

        [HttpPost]
        [Route("/mpw/resource/documents/{id}/export")]
        public IActionResult Export(string id)
        {
            if (SecurityTokenMissing())
                return Unauthorized();

            string filePath = Path.Combine(host.ContentRootPath, "App_Data", "test.zip");
            var bytes = System.IO.File.ReadAllBytes(filePath);

            return new FileContentResult(bytes, MediaTypeNames.Application.Octet)
            {
                FileDownloadName = $"test.{id}.zip",
            };
        }
    }
}
