using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Empower.Applications
{
    public class CreateAppRequest
    {
        public  string EditorVersion { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string PreviewPubFile { get; set; }
        public List<string> RoleName { get; set; }
    }
}
