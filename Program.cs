﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CreateThemeManifest
{
    class Program
    {
        static String Version = "1.0.0";

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                ConfigData Config = new ConfigData();
                if (Config.LoadConfigFile(args[0]))
                    WriteOutputFile(Config);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("CreateThemeManifest " + Version);
                Console.WriteLine("Written by Martin J. Pollard");
                Console.WriteLine("");
                Console.WriteLine("Creates a theme AndroidManifest.xml file using a project configuration file.");
                Console.WriteLine("");
                Console.WriteLine("CreateThemeManifest [project-file.xml]");
            }
        }

        static void WriteOutputFile(ConfigData Config)
        {
            StreamWriter OutputFile;
            DateTime dt = DateTime.Now;
            String VersionCode;

            if (Config.UseDatestamp)
            {
                /*
                VersionCode = dt.Year.ToString().PadLeft(4, '0') +
                              dt.Month.ToString().PadLeft(2, '0') +
                              dt.Day.ToString().PadLeft(2, '0') +
                              dt.Hour.ToString().PadLeft(2, '0') +
                              dt.Minute.ToString().PadLeft(2, '0') +
                              dt.Second.ToString().PadLeft(2, '0');
                */
                int Year = (dt.Year % 100);
                VersionCode = Year.ToString().PadLeft(2, '0') +
                              dt.Month.ToString().PadLeft(2, '0') +
                              dt.Day.ToString().PadLeft(2, '0') +
                              dt.Hour.ToString().PadLeft(2, '0') +
                              dt.Minute.ToString().PadLeft(2, '0');
            }
            else
            {
                VersionCode = Config.VersionCode;
            }

            try
            {
                OutputFile = new StreamWriter(Config.OutputFolder + "\\AndroidManifest.xml");

                OutputFile.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                OutputFile.WriteLine("<!--");
                OutputFile.WriteLine(" * Theme: {0} v{1}", Config.AppName, Config.VersionName);
                OutputFile.WriteLine(" *    By: {0}", Config.Author);
                OutputFile.WriteLine(" *");
                OutputFile.WriteLine(" * Manifest file generated by CreateThemeManifest");
                OutputFile.WriteLine(" * Written by Martin J. Pollard");
                OutputFile.WriteLine("-->");
                if (Config.OldThemeChooser)
                {
                    OutputFile.WriteLine("<manifest xmlns:android=\"http://schemas.android.com/apk/res/android\"");
                    OutputFile.WriteLine("          xmlns:pluto=\"http://www.w3.org/2001/pluto.html\"");
                    OutputFile.WriteLine("          package=\"{0}\"", Config.PackageName);
                    OutputFile.WriteLine("          android:versionCode=\"{0}\"", VersionCode);
                    OutputFile.WriteLine("          android:versionName=\"{0}\"", Config.VersionName);
                    OutputFile.WriteLine("          android:hasCode=\"false\"");
                    OutputFile.WriteLine("          android:installLocation=\"internalOnly\">");
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <uses-sdk android:minSdkVersion=\"{0}\"", Config.MinSdkVersion.ToString());
                    OutputFile.WriteLine("              android:targetSdkVersion=\"{0}\" />", Config.TargetSdkVersion.ToString());
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <application android:icon=\"@drawable/{0}\"", Config.IconDrawable);
                    OutputFile.WriteLine("                 android:label=\"@string/theme_name\">");
                    OutputFile.WriteLine("        <activity android:label=\"@string/theme_name\"");
                    OutputFile.WriteLine("                  android:name=\"{0}\" />", Config.PackageName);
                    OutputFile.WriteLine("    </application>");
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <theme pluto:author=\"@string/author\"");
                    OutputFile.WriteLine("           pluto:copyright=\"@string/copyright\"");
                    OutputFile.WriteLine("           pluto:name=\"@string/theme_name\"");
                    OutputFile.WriteLine("           pluto:preview=\"@drawable/{0}\"", Config.PreviewDrawable);
                    OutputFile.WriteLine("           pluto:styleId=\"@style/{0}\"", Config.AppName);
                    OutputFile.WriteLine("           pluto:styleName=\"@string/style_appearance_name\"");
                    OutputFile.WriteLine("           pluto:themeId=\"{0}\"", Config.AppName);
                    OutputFile.WriteLine("           pluto:wallpaperImage=\"@drawable/{0}\" />", Config.WallpaperDrawable);
                }
                else
                {
                    OutputFile.WriteLine("<manifest xmlns:android=\"http://schemas.android.com/apk/res/android\"");
                    OutputFile.WriteLine("          package=\"{0}\"", Config.PackageName);
                    OutputFile.WriteLine("          android:versionCode=\"{0}\"", VersionCode);
                    OutputFile.WriteLine("          android:versionName=\"{0}\"", Config.VersionName);
                    OutputFile.WriteLine("          android:installLocation=\"internalOnly\">");
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <meta-data android:name=\"org.cyanogenmod.theme.name\"");
                    OutputFile.WriteLine("               android:value=\"{0}\" />", Config.AppName);
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <meta-data android:name=\"org.cyanogenmod.theme.author\"");
                    OutputFile.WriteLine("               android:value=\"{0}\" />", Config.Author);
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <application android:hasCode=\"false\"");
                    OutputFile.WriteLine("                 android:allowBackup=\"true\"");
                    OutputFile.WriteLine("                 android:icon=\"@drawable/{0}\"", Config.IconDrawable);
                    OutputFile.WriteLine("                 android:label=\"{0}\" />", Config.AppName);
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <uses-sdk android:minSdkVersion=\"{0}\"", Config.MinSdkVersion.ToString());
                    OutputFile.WriteLine("              android:targetSdkVersion=\"{0}\" />", Config.TargetSdkVersion.ToString());
                    OutputFile.WriteLine("");
                    OutputFile.WriteLine("    <uses-feature android:required=\"true\"");
                    OutputFile.WriteLine("                  android:name=\"org.cyanogenmod.theme\" />");
                    if (Config.SetWallpaperPermission || Config.ModifyAudioSettingsPermission)
                    {
                        OutputFile.WriteLine("");
                        if (Config.SetWallpaperPermission)
                            OutputFile.WriteLine("    <uses-permission android:name=\"android.permission.SET_WALLPAPER\" />");
                        if (Config.ModifyAudioSettingsPermission)
                            OutputFile.WriteLine("    <uses-permission android:name=\"android.permission.MODIFY_AUDIO_SETTINGS\" />");
                    }
                }
                OutputFile.WriteLine("");
                OutputFile.WriteLine("</manifest>");

                OutputFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("");
                Console.WriteLine(e.Message);
            }
        }
    }
}
