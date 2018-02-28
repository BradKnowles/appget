﻿using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AppGet.Manifests
{
    public class PackageManifest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Licence { get; set; }
        public string Version { get; set; }

        [YamlIgnore]
        public string VersionTag { get; set; }

        public string ProductUrl { get; set; }

        public string[] Exe { get; set; }

        public InstallMethodTypes InstallMethod { get; set; }

        public List<Installer> Installers { get; set; }

        public InstallArgs Args { get; set; }
    }

    public class InstallArgs
    {
        public string Passive { get; set; }
        public string Silent { get; set; }
        public string Interactive { get; set; }
    }

    public class Installer
    {
        public string Location { get; set; }
        public string ZipSubDir { get; set; }

        public string Sha1 { get; set; }
        public string Sha256 { get; set; }
        public string Md5 { get; set; }

        [YamlIgnore] 
        public string FilePath { get; set; }

        public ArchitectureTypes Architecture { get; set; }

        public Version MinWindowsVersion { get; set; }

        public List<string> ProductIds { get; set; }

        public FileHash GetFileHash()
        {
            if (!string.IsNullOrEmpty(Sha256))
            {
                return new FileHash { HashType = HashTypes.Sha256, Value = Sha256 };
            }

            if (!string.IsNullOrEmpty(Sha1))
            {
                return new FileHash { HashType = HashTypes.Sha1, Value = Sha1 };
            }

            if (!string.IsNullOrEmpty(Md5))
            {
                return new FileHash { HashType = HashTypes.Md5, Value = Md5 };
            }

            return null;
        }
    }
}