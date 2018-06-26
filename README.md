# VRConvert
Windows Commandline App to convert Googles *.vr.jpg files to SBS180 png files.

VRConvert Version 0.2
This source includes a slightly modified version of the VRJPEG Library by Joan Charmant
The original library can be found here: https://github.com/JoanCharmant/vrjpeg.git
Usage:
VRConvert [-new] [-format:<P360, SBS180>]
Example:
VRConvert "C:\VR180" "C:\VR180\Converted" -new -format:SBS180
Attention:
Please enclose the path in double quotes not single quotes like the app writes out.
