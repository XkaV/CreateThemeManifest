using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CreateThemeManifest
{
    class ConfigData
    {
        public string OutputFolder;
        public bool OldThemeChooser;
        public string PackageName;
        public string AppName;
        public string Author;
        public string VersionName;
        public string VersionCode;
        public bool UseDatestamp;
        public int MinSdkVersion;
        public int TargetSdkVersion;
        public string IconDrawable;
        public string PreviewDrawable;
        public string WallpaperDrawable;
        public bool SetWallpaperPermission;
        public bool ModifyAudioSettingsPermission;

        public void Initialize()
        {
            OutputFolder = "";
            OldThemeChooser = false;
            PackageName = "";
            AppName = "";
            Author = "";
            VersionName = "";
            VersionCode = "";
            UseDatestamp = false;
            MinSdkVersion = 19;
            TargetSdkVersion = 19;
            IconDrawable = "";
            PreviewDrawable = "";
            WallpaperDrawable = "";
            SetWallpaperPermission = false;
            ModifyAudioSettingsPermission = false;
        }

        public bool LoadConfigFile(string Filename)
        {
            try
            {
                XmlDocument Document = new XmlDocument();
                Document.Load(Filename);

                XmlNode Node = Document.GetElementsByTagName("Output")[0];
                SetString(Node, "Folder", ref OutputFolder, true);
                SetBool(Node, "OldThemeChooser", ref OldThemeChooser, false);

                Node = Document.GetElementsByTagName("Package")[0];
                SetString(Node, "Name", ref PackageName, true);
                SetString(Node, "AppName", ref AppName, true);
                SetString(Node, "Author", ref Author, true);

                Node = Document.GetElementsByTagName("Versions")[0];
                SetString(Node, "VersionName", ref VersionName, true);
                SetString(Node, "VersionCode", ref VersionCode, false);
                if ((VersionCode == null) || (VersionCode.Length == 0))
                    UseDatestamp = true;
                SetInt(Node, "SdkMinimum", ref MinSdkVersion, false);
                TargetSdkVersion = MinSdkVersion;
                SetInt(Node, "SdkTarget", ref TargetSdkVersion, false);

                Node = Document.GetElementsByTagName("Application")[0];
                SetString(Node, "Icon", ref IconDrawable, true);
                SetString(Node, "Preview", ref PreviewDrawable, false);
                SetString(Node, "Wallpaper", ref WallpaperDrawable, false);

                Node = Document.GetElementsByTagName("Permissions")[0];
                if (Node != null)
                {
                    SetBool(Node, "SetWallpaper", ref SetWallpaperPermission, false);
                    SetBool(Node, "ModifyAUdioSettings", ref ModifyAudioSettingsPermission, false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        static void SetInt(XmlNode n, string attribute, ref int val, bool required)
        {
            XmlAttribute a = n.Attributes[attribute];
            if (a == null)
            {
                if (!required)
                    return;
                throw new Exception("Missing attribute in " + n.Name + ": " + attribute);
            }
            val = int.Parse(a.Value);
        }

        static void SetString(XmlNode n, string attribute, ref string val, bool required)
        {
            XmlAttribute a = n.Attributes[attribute];
            if (a == null)
            {
                if (!required)
                    return;
                throw new Exception("Missing attribute in " + n.Name + ": " + attribute);
            }
            val = a.Value;
        }

        static void SetFloat(XmlNode n, string attribute, ref float val, bool required)
        {
            XmlAttribute a = n.Attributes[attribute];
            if (a == null)
            {
                if (!required)
                    return;
                throw new Exception("Missing attribute in " + n.Name + ": " + attribute);
            }
            val = float.Parse(a.Value);
        }

        static void SetBool(XmlNode n, string attribute, ref bool val, bool required)
        {
            XmlAttribute a = n.Attributes[attribute];
            if (a == null)
            {
                if (!required)
                    return;
                throw new Exception("Missing attribute in " + n.Name + ": " + attribute);
            }
            val = bool.Parse(a.Value);
        }
    }
}
