using EddiDataDefinitions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Utilities;



namespace EddiCargoMonitor
{
    public class CargoInfoReader
    {
        public int Count { get; set; }
        public List<CargoInfo> Inventory { get; set; }

        [JsonIgnore]
        private string dataPath;

        [JsonIgnore]
        static readonly object fileLock = new object();

        public CargoInfoReader()
        {
            Inventory = new List<CargoInfo>();
        }

        public static CargoInfoReader FromFile(string filename = null)
        {
            Regex Filter = new Regex(@"^Cargo\.json$");
            string directory = GetSavedGamesDir();
            if (directory == null || directory.Trim() == "")
            {
                return null;
            }


            if (filename == null)
            {
                filename = directory + @"\Cargo.json";
            }

            CargoInfoReader info = new CargoInfoReader();
            if (File.Exists(filename))
            {
                try
                {
                    string data = Files.Read(filename);
                    if (data != null)
                    {
                        info = JsonConvert.DeserializeObject<CargoInfoReader>(data);
                    }
                }
                catch (Exception ex)
                {
                    Logging.Debug("Failed to read cargo info", ex);
                }
            }

            if (info == null)
            {
                info = new CargoInfoReader();
            }

            info.dataPath = filename;
            return info;
        }

        private static string GetSavedGamesDir()
        {
            IntPtr path;
            int result = NativeMethods.SHGetKnownFolderPath(new Guid("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"), 0, new IntPtr(0), out path);
            if (result >= 0)
            {
                return Marshal.PtrToStringUni(path) + @"\Frontier Developments\Elite Dangerous";
            }
            else
            {
                throw new ExternalException("Failed to find the saved games directory.", result);
            }
        }

        internal class NativeMethods
        {
            [DllImport("Shell32.dll")]
            internal static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);
        }

        /// <summary>Find the latest file in a given directory matching a given expression, or null if no such file exists</summary>
        private static FileInfo FindCargoInfoFile(string path, Regex filter = null)
        {
            if (path == null)
            {
                // Configuration can be changed underneath us so we do have to check each time...
                return null;
            }

            var directory = new DirectoryInfo(path);
            if (directory != null)
            {
                try
                {
                    FileInfo info = directory.GetFiles().Where(f => filter == null || filter.IsMatch(f.Name)).FirstOrDefault();
                    if (info != null)
                    {
                        // This info can be cached so force a refresh
                        info.Refresh();
                    }
                    return info;
                }
                catch { }
            }
            return null;
        }
    }
}