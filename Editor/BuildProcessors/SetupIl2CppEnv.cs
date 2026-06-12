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
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LeanCLR.BuildProcessors
{

    internal class SetupIl2CppEnv : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!Settings.EnableForCurrentBuildTarget)
            {
                Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", "");
                Environment.SetEnvironmentVariable("LEANAOT_EXTRA_ARGS", "");
                return;
            }
            var installerController = new LocalInstaller();
            if (!installerController.HasInstalledToLocal())
            {
                throw new Exception($"Please install LeanCLR first.");
            }
            string runtimeDir = Settings.LocalIl2CppPath;
            Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", runtimeDir);
            Debug.Log($"[SetupIl2CppEnv] set UNITY_IL2CPP_PATH='{runtimeDir}'");

            string leanAotExtraArgs = BuildLeanAotExtraArgs(report.summary.platform);
            Environment.SetEnvironmentVariable("LEANAOT_EXTRA_ARGS", leanAotExtraArgs);
            Debug.Log($"[SetupIl2CppEnv] set LEANAOT_EXTRA_ARGS='{leanAotExtraArgs}'");
        }

        private static string BuildLeanAotExtraArgs(BuildTarget target)
        {
            LeanAOTSettings aot = Settings.Instance.leanAOTSettings;
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

            var sb = new StringBuilder();
            sb.Append(" --leanaot-unity-version=");
            sb.Append(Application.unityVersion);
            sb.Append(" --leanaot-may-throw-exception-in-icall");
            if (aot.layoutValidation)
            {
                sb.Append(" --leanaot-enable-layout-validation");
            }
            sb.Append($" --leanaot-managed-stripped-duplicate-path=\"{Settings.GetManagedStrippedDuplicatePath(target)}\"");

            AppendExistingFileArguments(sb, aot.ruleFiles, projectRoot, "--leanaot-aot-rule-file", "AOT rule file");
            AppendExistingFileArguments(sb, aot.pgoRuleFiles, projectRoot, "--leanaot-pgo-rule-file", "PGO rule file");

            if (aot.lazyLoadedAssemblyNames != null)
            {
                foreach (string raw in aot.lazyLoadedAssemblyNames)
                {
                    if (string.IsNullOrWhiteSpace(raw))
                    {
                        continue;
                    }

                    AppendArgument(sb, "--leanaot-exclude-assembly-from-global-metadata=" + raw.Trim());
                }
            }

            return sb.ToString();
        }

        private static void AppendExistingFileArguments(
            StringBuilder sb,
            string[] paths,
            string projectRoot,
            string argument,
            string configLabel)
        {
            if (paths == null)
            {
                return;
            }

            foreach (string raw in paths)
            {
                if (string.IsNullOrWhiteSpace(raw))
                {
                    continue;
                }

                string fullPath = ResolveProjectFilePath(raw, projectRoot);
                if (!File.Exists(fullPath))
                {
                    throw new Exception(
                        $"LeanCLR: {configLabel} not found. Configured path: '{raw}', resolved to: '{fullPath}'.");
                }

                AppendQuotedFileArgument(sb, argument, fullPath);
            }
        }

        private static string ResolveProjectFilePath(string raw, string projectRoot)
        {
            string trimmed = raw.Trim();
            return Path.IsPathRooted(trimmed)
                ? Path.GetFullPath(trimmed)
                : Path.GetFullPath(Path.Combine(projectRoot, trimmed));
        }

        private static void AppendArgument(StringBuilder sb, string argument)
        {
            if (sb.Length > 0)
            {
                sb.Append(' ');
            }

            sb.Append(argument);
        }

        private static void AppendQuotedFileArgument(StringBuilder sb, string argument, string absolutePath)
        {
            AppendArgument(sb, argument);
            sb.Append(' ');
            sb.Append('"');
            sb.Append(absolutePath.Replace("\"", "\\\""));
            sb.Append('"');
        }
    }
}
