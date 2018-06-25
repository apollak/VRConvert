#region License
// 
// 2018 Andreas Pollak
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    Includes a modified version of Joan Charmants VRJPEG Library under it's
//    own license.
//
#endregion
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using VrJpeg;

namespace VRConvert
{
    class Program
    {
        static int Main(string[] args)
        {
            ConversionOptions options = new ConversionOptions();

            DisplayInfo(args);
           
            options = OptionsFromParameters(args);
            if (options == null) return 1;


            foreach (var file in options.Source.GetFiles("*.jpg"))
            {
                FileInfo destinationFile = new FileInfo(
                        options.Destination.FullName + @"\" + file.Name.Substring(0, file.Name.Length - (file.Extension.Length)) + ".png"
                    );

                if (File.Exists(destinationFile.FullName) && !(options.ForceRecreate) )
                {
                    Console.WriteLine($"Skipping: {file.FullName}");
                    continue;
                }

                Console.Write($"Converting: {file.Name}  ==> ");
                switch (options.OutputFormat)
                {
                    case ConversionOptions.OutputFormatOption.P360:
                        {
                            ConvertStereoImageEquirectangular(file.FullName, destinationFile.FullName);
                        }
                        break;

                    case ConversionOptions.OutputFormatOption.SBS180:
                        {
                            ConvertStereoImageSBS(file.FullName, destinationFile.FullName);
                        }
                        break;
                }
                Console.WriteLine(destinationFile.Name);
            }
            return 0;
        }

        private static ConversionOptions OptionsFromParameters(string[] args)
        {
            if (args.Length < 2) return null;
            ConversionOptions options = new ConversionOptions
            {
                Source = new DirectoryInfo(args[0]),
                Destination = new DirectoryInfo(args[1])
            };

            if (args.Where(arg => arg.ToLower() == "-new").Count() > 0) options.ForceRecreate = true;
            if (args.Where(arg => arg.ToLower() == "-format:p360").Count() > 0) options.OutputFormat = ConversionOptions.OutputFormatOption.P360;
            if (args.Where(arg => arg.ToLower() == "-format:sbs180").Count() > 0) options.OutputFormat = ConversionOptions.OutputFormatOption.SBS180;
            return options;
        }

        private static void DisplayInfo(string[] args)
        {
            Console.WriteLine("VRConvert Version 0.1");
            Console.WriteLine("This source includes a slightly modified version of the VRJPEG Library by Joan Charmant ");
            Console.WriteLine("The original library can be found here: https://github.com/JoanCharmant/vrjpeg.git");
            Console.WriteLine("");
            if (args.Length < 2)
            {
                Console.WriteLine("VRConvert <SourceDir> <DestinationDir> [-new] [-format:<P360, SBS180>]");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine(@"    VRConvert 'C:\VR180' 'C:\VR180\Converted' -new -format:P360");
            }
        }

        private static void ConvertStereoImageEquirectangular(string sourceFilename, string destinationName)
        {
            // Extract both eyes and create an equirectangular stereo image.
            int maxWidth = 8192;
            bool fillPoles = true;
            Bitmap composite = VrJpegHelper.CreateStereoEquirectangular(sourceFilename, EyeImageGeometry.LeftRight, fillPoles, maxWidth);

            // Save the result.
            string compositeFilename = destinationName;
            string compositeFile = Path.Combine(Path.GetDirectoryName(sourceFilename), compositeFilename);
            composite.Save(compositeFile);
        }

        private static void ConvertStereoImageSBS(string sourceFilename, string destinationName)
        {
            // Extract both eyes and create an equirectangular stereo image.
            bool fillPoles = true;
            Bitmap composite = VrJpegHelper.CreateStereo(sourceFilename, EyeImageGeometry.LeftRight, fillPoles);
            // Save the result.
            string compositeFilename = destinationName;
            string compositeFile = Path.Combine(Path.GetDirectoryName(sourceFilename), compositeFilename);
            composite.Save(compositeFile);
        }

    }
}
