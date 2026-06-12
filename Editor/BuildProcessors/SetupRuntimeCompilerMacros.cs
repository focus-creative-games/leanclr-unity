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

using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LeanCLR.BuildProcessors
{
    internal class SetupUnityVersionCompilerMacros : IPreprocessBuildWithReport
    {
        public int callbackOrder => 2;

        public void OnPreprocessBuild(BuildReport report)
        {
            ApplyForBuildTarget(report.summary.platform);
        }

        /// <summary>
        /// Refreshes PlayerSettings additional IL2CPP args for the given build target
        /// (or the active editor build target when called from the menu).
        /// </summary>
        public static void UpdateUnityVersion(BuildTarget? targetPlatform = null)
        {
            ApplyForBuildTarget(targetPlatform ?? EditorUserBuildSettings.activeBuildTarget);
        }

        static void ApplyForBuildTarget(BuildTarget platform)
        {
            var installerController = new LocalInstaller();
            if (!installerController.HasInstalledToLocal())
            {
                throw new Exception("Please install LeanCLR first.");
            }

            bool enable = Settings.EnableForCurrentBuildTarget;
            string current = PlayerSettings.GetAdditionalIl2CppArgs();
            string stripped = StripUnityVersionCompilerDefines(current);
            stripped = CollapseDuplicateWhitespace(stripped);

            if (!enable)
            {
                PlayerSettings.SetAdditionalIl2CppArgs(stripped.Trim());
                return;
            }

            var ver = installerController.CurVersion;
            
            string[] lazyLoadedAssemblyNames = Settings.Instance.leanAOTSettings?.lazyLoadedAssemblyNames;
            string inner = BuildCompilerDefineFlags(ver, lazyLoadedAssemblyNames);
            string block = $"--compiler-flags=\"{inner}\"";
            string merged = string.IsNullOrWhiteSpace(stripped) ? block : $"{stripped} {block}";
            PlayerSettings.SetAdditionalIl2CppArgs(merged.Trim());
            Debug.Log($"[LeanCLR] IL2CPP compiler defines: {inner} (target={platform})");
        }

        /// <summary>
        /// Removes LeanCLR-owned UNITY_VERSION / UNITY_TUANJIE_ENGINE / GC / placeholder-assembly
        /// preprocessor flags (-D... and legacy /D...) from any quoted --compiler-flags="..." segments
        /// before re-applying.
        /// </summary>
        internal static string StripUnityVersionCompilerDefines(string additionalIl2CppArgs)
        {
            if (string.IsNullOrEmpty(additionalIl2CppArgs))
            {
                return string.Empty;
            }

            return Regex.Replace(
                additionalIl2CppArgs,
                @"--compiler-flags=""([^""]*)""",
                m =>
                {
                    string inner = m.Groups[1].Value;
                    string cleaned = StripUnityDefineTokens(inner);
                    if (string.IsNullOrWhiteSpace(cleaned))
                    {
                        return string.Empty;
                    }

                    return $"--compiler-flags=\"{cleaned.Trim()}\"";
                });
        }

        static string StripUnityDefineTokens(string inner)
        {
            if (string.IsNullOrWhiteSpace(inner))
            {
                return string.Empty;
            }

            string s = inner;
            const RegexOptions opt = RegexOptions.IgnoreCase;
            // Require start-of-string or whitespace before -D so we strip the first token
            // (e.g. inner is exactly "-DUNITY_VERSION=60000400", not only after a space).
            s = Regex.Replace(s, @"(?:^|[\s]+)-D\s*UNITY_VERSION\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)-D\s*UNITY_TUANJIE_ENGINE(\s*=\s*\d+)?", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)/D\s*UNITY_VERSION\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)/D\s*UNITY_TUANJIE_ENGINE(\s*=\s*\d+)?", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)-D\s*LEANCLR_GC_ZERO_GC\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)-D\s*LEANCLR_GC_MARK_SWEEP\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)/D\s*LEANCLR_GC_ZERO_GC\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)/D\s*LEANCLR_GC_MARK_SWEEP\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)-D\s*LEANCLR_GC_DEBUG\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)/D\s*LEANCLR_GC_DEBUG\s*=\s*\d+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)-D\s*LEANCLR_PLACEHOLDER_ASSEMBLY_NAMES\s*=\S+", " ", opt);
            s = Regex.Replace(s, @"(?:^|[\s]+)/D\s*LEANCLR_PLACEHOLDER_ASSEMBLY_NAMES\s*=\S+", " ", opt);
            return s.Trim();
        }

        static string CollapseDuplicateWhitespace(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return Regex.Replace(s.Trim(), @"\s{2,}", " ");
        }

        /// <summary>
        /// IL2CPP additional args use Clang-style -D defines for all targets (Unity IL2CPP toolchain).
        /// </summary>
        internal static string BuildCompilerDefineFlags(UnityVersion ver, string[] lazyLoadedAssemblyNames)
        {
            int packed = ver.major * 10000 + ver.minor1 * 100 + ver.minor2;
            var sb = new StringBuilder(64);
            sb.Append("-DUNITY_VERSION=").Append(packed);
            if (ver.isTuanjieEngine)
            {
                sb.Append(" -DUNITY_TUANJIE_ENGINE=1");
            }

            GCSettings gCSettings = Settings.Instance.gcSettings;
            switch (gCSettings.mode)
            {
                case GCMode.Zero:
                    sb.Append(" -DLEANCLR_GC_ZERO_GC=1");
                    break;
                case GCMode.MarkSweep:
                    sb.Append(" -DLEANCLR_GC_MARK_SWEEP=1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gCSettings.mode), gCSettings.mode, null);
            }

            if (gCSettings.enableGCDebug)
            {
                sb.Append(" -DLEANCLR_GC_DEBUG=1");
            }

            if (lazyLoadedAssemblyNames != null && lazyLoadedAssemblyNames.Length > 0)
            {
                sb.Append(" -DLEANCLR_PLACEHOLDER_ASSEMBLY_NAMES=").Append(string.Join(",", lazyLoadedAssemblyNames));
            }
            
            return sb.ToString();
        }
    }
}
