using System;
using System.Collections.Generic;

namespace Empower.Document
{
    public class Document
    {
        public string AppId { get; set; }
        public string ApplicationName { get; set; }
        public string BusDocId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Deleted { get; set; }
        public string DocId { get; set; }
        public List<string> DocTags { get; set; }
        public string DocumentVersion { get; set; }
        public string EditorVersion { get; set; }
        public string EngineVersion { get; set; }
        public DateTime? ExportDate { get; set; }
        public string FileName { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime LastSaveDate { get; set; }
        public string OwnerIds { get; set; }
        public string PackageFileName { get; set; }
        public string PackageVersion { get; set; }
        public string PreviewPubFile { get; set; }
        public string RoleNames { get; set; }
        public string UserId { get; set; }
    }
}
