// Copyright 2026 Code Philosophy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#if UNITY_6000_3_OR_NEWER && UNITY_EDITOR_OSX
#define NEW_IL2CPP_PATH
#endif
#if UNITY_6000_3_OR_NEWER && UNITY_EDITOR_OSX
#define NEW_IL2CPP_PATH
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Debug = UnityEngine.Debug;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace LeanCLR
{

    public class LocalInstaller
    {
        public int MajorVersion => _curVersion.major;

        private readonly UnityVersion _curVersion;

        public UnityVersion CurVersion => _curVersion;

        public LocalInstaller()
        {
            _curVersion = new UnityVersion(Application.unityVersion);
        }

        public string ApplicationIl2cppPath
        {
            get
            {
                Debug.Log($"application path:{EditorApplication.applicationPath} {EditorApplication.applicationContentsPath}");
#if NEW_IL2CPP_PATH
#if UNITY_IOS
                string platformDirName = "iOSSupport";
#elif UNITY_TVOS
                string platformDirName = "AppleTVSupport";
#elif UNITY_VISIONOS
                string platformDirName = "VisionOSPlayer";
#else
                string platformDirName = "iOSSupport";
#endif
                return $"{EditorApplication.applicationContentsPath}/../../PlaybackEngines/{platformDirName}/il2cpp";
#else
                return $"{EditorApplication.applicationContentsPath}/il2cpp";
#endif
            }
        }

        public void InstallLocal()
        {
            RunInitLocalIl2CppData(ApplicationIl2cppPath, Settings.LeanCLRRuntimeCppPathInPackage, _curVersion);
        }

        public bool HasInstalledToLocal()
        {
            return Directory.Exists(Settings.LocalLibil2cppPath);
        }

        public bool NeedReinstallAfterUpdatePackage()
        {
            if (!HasInstalledToLocal())
            {
                return false;
            }
            long currentTicks = ComputePackageLatestFileWriteTimeUtcTicks();
            if (currentTicks == 0L)
            {
                return true;
            }
            string fingerprintPath = InstalledPackageFingerprintPath;
            if (!File.Exists(fingerprintPath))
            {
                return true;
            }
            string savedText = File.ReadAllText(fingerprintPath, Encoding.UTF8).Trim();
            if (!long.TryParse(savedText, out long savedTicks))
            {
                return true;
            }
            return currentTicks != savedTicks;
        }

        private static string InstalledPackageFingerprintPath =>
            Path.Combine(Settings.InstallRootDir, "installed_package_max_mtime_utc_ticks.txt");

        private static string GetLeanClrPackageRootPath()
        {
            var packageInfo = PackageInfo.FindForAssembly(typeof(LocalInstaller).Assembly);
            if (!string.IsNullOrEmpty(packageInfo?.resolvedPath))
            {
                return packageInfo.resolvedPath;
            }
            return Path.GetFullPath(Path.Combine("Packages", Settings.PackageName));
        }

        private static long ComputePackageLatestFileWriteTimeUtcTicks()
        {
            string root = GetLeanClrPackageRootPath();
            if (!Directory.Exists(root))
            {
                return 0L;
            }
            long maxTicks = 0L;
            foreach (string file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                long ticks = File.GetLastWriteTimeUtc(file).Ticks;
                if (ticks > maxTicks)
                {
                    maxTicks = ticks;
                }
            }
            return maxTicks;
        }

        private static void WriteInstalledPackageFingerprint()
        {
            long ticks = ComputePackageLatestFileWriteTimeUtcTicks();
            if (ticks == 0L)
            {
                return;
            }
            string path = InstalledPackageFingerprintPath;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, ticks.ToString(), Encoding.UTF8);
        }

        private static string AddExeSuffix(string path)
        {
#if UNITY_EDITOR_WIN
            return path + ".exe";
#else
            return path;
#endif
        }

        private void RunInitLocalIl2CppData(string editorIl2cppPath, string libil2cppWithHybridclrSourceDir, UnityVersion version)
        {
            string workDir = Settings.InstallRootDir;
            Directory.CreateDirectory(workDir);

            // create LocalIl2Cpp
            string localIl2CppDataDir = Settings.LocalIl2CppDataPath;
            BashUtil.RecreateDir(localIl2CppDataDir);
#if !NEW_IL2CPP_PATH
            // copy MonoBleedingEdge
            BashUtil.CopyDir($"{Directory.GetParent(editorIl2cppPath)}/MonoBleedingEdge", $"{localIl2CppDataDir}/MonoBleedingEdge", true);
#endif
            // copy il2cpp
            BashUtil.CopyDir(editorIl2cppPath, Settings.LocalIl2CppPath, true);
#if NEW_IL2CPP_PATH
            string buildDir = $"{Settings.LocalIl2CppPath}/build";
            if (RuntimeInformation.ProcessArchitecture == Architecture.Arm || RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
            {
                BashUtil.CopyDir($"{buildDir}/deploy_arm64", $"{buildDir}/deploy", false);
            }
            else
            {
                BashUtil.CopyDir($"{buildDir}/deploy_x86_64", $"{buildDir}/deploy", false);
            }
#endif

            // replace libil2cpp
            string dstLibil2cppDir = Settings.LocalLibil2cppPath;
            BashUtil.CopyDir($"{libil2cppWithHybridclrSourceDir}", dstLibil2cppDir, true);

            string orginalIl2CppExecutable = AddExeSuffix($"{Settings.LocalIl2CppToolPath}/il2cpp");
            string il2cppproxyExecutable = AddExeSuffix($"{Settings.Il2CppProxyDir}/il2cpp");
            string backupIl2CppExecutable = AddExeSuffix($"{Settings.LocalIl2CppToolPath}/il2cpp-origin");
            // il2cpp -> il2cpp-origin
            // il2cppproxy -> il2cpp
            File.Copy(orginalIl2CppExecutable, backupIl2CppExecutable, true);
            File.Copy(il2cppproxyExecutable, orginalIl2CppExecutable, true);

            // copy leanaot
            string srcLeanAotDir = Settings.LeanAOTPathInPackage;
            string dstLeanAotDir = Settings.LocalLeanAotDir;
            BashUtil.CopyDir(srcLeanAotDir, dstLeanAotDir, true);

            // clean Il2cppBuildCache
            BashUtil.RemoveDir($"Library/Il2cppBuildCache", true);
            BashUtil.RemoveDir($"Library/Bee", true);
            if (HasInstalledToLocal())
            {
                WriteInstalledPackageFingerprint();
                Debug.Log("Install Sucessfully");
            }
            else
            {
                Debug.LogError("Installation failed!");
            }
        }
    }
}
