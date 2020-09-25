using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Empower.Document
{
    public class ImportRequest
    {
        public string AppId { get; set; }
        public string BusDocId { get; set; }
        public List<string> DocTag { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public string OwnerId { get; set; }
    }
}
