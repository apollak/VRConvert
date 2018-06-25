using System.IO;

namespace VRConvert
{
    public class ConversionOptions
    {
        public enum OutputFormatOption
        {
            P360,
            SBS180
        }

        public bool ForceRecreate { get; set; } = false;

        public OutputFormatOption OutputFormat { get; set; } = OutputFormatOption.P360;

        public DirectoryInfo Source { get; set; }

        public DirectoryInfo Destination { get; set; }
    }
}
