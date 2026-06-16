using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;

namespace LeanCLR.BuildProcessors
{

	public static class BuildProcessorUtil
	{
        public static string NormalizeAssemblyName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            string name = raw.Trim();
            if (name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - 4);
            }

            return string.IsNullOrEmpty(name) ? null : name;
        }

        public static List<string> GetNormalizedAssemblyNames(string[] assemblyNames)
        {
            var result = new List<string>();
            if (assemblyNames == null)
            {
                return result;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (string raw in assemblyNames)
            {
                string name = NormalizeAssemblyName(raw);
                if (name == null)
                {
                    continue;
                }

                if (!seen.Add(name))
                {
                    throw new BuildFailedException($"[LeanCLR] assembly '{name}' is duplicated");
                }

                result.Add(name);
            }

            return result;
        }

        public static string GetXcodeProjectFile(string pathToBuiltProject)
        {
            foreach (string dir in Directory.GetDirectories(pathToBuiltProject, "*.xcodeproj", SearchOption.TopDirectoryOnly))
            {
                string pbxprojFile = $"{dir}/project.pbxproj";
                if (File.Exists(pbxprojFile))
                {
                    return pbxprojFile;
                }
            }
            throw new BuildFailedException($"can't find xxxx.xcodeproj/project.pbxproj in {pathToBuiltProject}");
        }
    }
}

