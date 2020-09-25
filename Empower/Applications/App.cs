using System;
using System.Collections.Generic;

namespace Empower.Applications
{
    public class App
    {
        public string AppId { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationOI { get; set; }

        public string ApplicationPath { get; set; }

        public string DatabaseId { get; set; }

        public string EditorVersion { get; set; }

        public string EngineVersion { get; set; }

        public DateTime PackageCreationDate { get; set; }

        public string PackageFile { get; set; }

        public string PackageVersion { get; set; }

        public string PreviewPubFile { get; set; }

        public List<string> RoleNames { get; set; }
    }
}
