using System;
using System.Collections.Generic;
using System.Text;

namespace DALGen
{
    public class GenerateSettings
    {
        public string BaseDirectory { get; set; }

        public string CoreSdkVer { get; set; }

        public string DataFileName { get; set; }

        public string SolutionName { get; set; }

        public string AuthorName { get; set; }

        public string ProjectName { get; set; }

        public string CompanyName { get; set; }

        public string CopyRight { get; set; }

        public string DalSpaceName { get; set; }

        public string ModelSpaceName { get; set; }

        public string DbContextName { get; set; }

        public string DevConnectionString { get; set; }

        public List<string> ModelNames { get; set; }

        public List<string> DalRefferences { get; set; }

        public List<string> DalPackages { get; set; }

        public bool WebApiCreation { get; set; }

        public string WebApiFolder { get; set; }

        public List<string> WebApiRefferences { get; set; }

        public List<string> WebApiPackages { get; set; }

        public bool SDKCreation { get; set; }

        public string SDKFolder { get; set; }

        public List<string> SDKRefferences { get; set; }

        public List<string> SDKPackages { get; set; }

        public bool WebCreation { get; set; }

        public string WebFolder { get; set; }

        public List<string> WebRefferences { get; set; }

        public List<string> WebPackages { get; set; }
    }
}
