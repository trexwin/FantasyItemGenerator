using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileReader.Parser
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class SupportedExtensionsAttribute : Attribute
    {
        public string[] FileExtensions { get; set; }

        public SupportedExtensionsAttribute(string fileExtension)
            => FileExtensions = [fileExtension];
        public SupportedExtensionsAttribute(string[] fileExtensions)
            => FileExtensions = fileExtensions;
    }
}
