using System;
using System.IO;
using System.Text;
using UnityEngine;
using World.Generation;

namespace SaveLoad
{
    /// <summary>
    /// A directory builder for saving/loading files
    /// </summary>
    public class FileLocation
    {
        private readonly string _location;
        private readonly string _fileName;
        private readonly Func<WorldSettings, string> _argumentPath;

        private string _builtLocation;
        
        public FileLocation(string location, string fileName, Func<WorldSettings, string> argumentPath = null)
        {
            _location = location;
            _fileName = fileName;
            _argumentPath = argumentPath;
        }

        /// <summary>
        /// Gets the Path of this SaveLocation
        /// </summary>
        /// <param name="worldSettings">Current world setting</param>
        /// <returns>The directory path to the location, does not include the file name</returns>
        public string GetPath(WorldSettings worldSettings)
        {
            if (_builtLocation != null) return _builtLocation;
            
            var p = new StringBuilder(Application.persistentDataPath + "/Data/" + _location);

            if (_argumentPath != null)
            {
                p.Append(_argumentPath(worldSettings) + "/");
            }

            if (!p.ToString().EndsWith("/"))
            {
                p.Append("/");
            }

            return _builtLocation = p.ToString();
        }

        /// <summary>
        /// Gets the full path of this SaveLocation
        /// </summary>
        /// <param name="worldSettings">Current world setting</param>
        /// <returns>The directory path to the location, including the file name</returns>
        public string GetFullPath(WorldSettings worldSettings)
        {
            return GetPath(worldSettings) + _fileName;
        }

        /// <summary>
        /// If the file this location points to exist
        /// </summary>
        /// <param name="worldSettings">Current world setting</param>
        /// <returns>If this file exist</returns>
        public bool Exists(WorldSettings worldSettings)
        {
            return _fileName == string.Empty ? Directory.Exists(GetPath(worldSettings)) : File.Exists(GetFullPath(worldSettings));
        }

        public string[] Directories(WorldSettings worldSettings)
        {
            return Directory.GetDirectories(GetPath(worldSettings));
        }

        public static FileLocation Global(string fileName, bool isBinary = true)
        {
            return new FileLocation(string.Empty, isBinary ? fileName + ".tri" : fileName);
        }

        public static FileLocation CurrentWorldSave(string fileName, bool isBinary = true)
        {
            return new FileLocation("Saves/", isBinary ? fileName + ".tri" : fileName, settings => settings.WorldName);
        }

        public static FileLocation Folder(string path)
        {
            return new FileLocation(path, string.Empty);
        }

        public static FileLocation WorldSavesFolder(string worldName = "")
        {
            return new FileLocation($"Saves/{worldName}", string.Empty);
        }

        public void CreateDirectoryIfAbsent(WorldSettings worldSettings)
        {
            Directory.CreateDirectory(GetPath(worldSettings));
        }
        
        public FileLocation AddPath(string path)
        {
            return new FileLocation(_location + path, _fileName, _argumentPath);
        }

        public FileLocation FileAtLocation(string fileName, bool isBinary = true)
        {
            if (isBinary) fileName += ".tri";
            return new FileLocation(_location, fileName, _argumentPath);
        }
    }
}