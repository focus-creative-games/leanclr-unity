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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace LeanCLR.BuildProcessors
{
    internal class FilterHotUpdateAssembly : IFilterBuildAssemblies
    {
        public int callbackOrder => 0;

        public string[] OnFilterAssemblies(BuildOptions buildOptions, string[] assemblies)
        {
            if (!Settings.EnableForCurrentBuildTarget)
            {
                return assemblies;
            }

            List<string> hotUpdateNames = BuildProcessorUtil.GetNormalizedAssemblyNames(
                Settings.Instance.hotUpdateSettings?.hotUpdateAssemblyNames);
            if (hotUpdateNames.Count == 0)
            {
                return assemblies;
            }

            var hotUpdateNameSet = new HashSet<string>(hotUpdateNames, StringComparer.Ordinal);
            var buildAssemblyNames = new HashSet<string>(
                assemblies.Select(Path.GetFileNameWithoutExtension),
                StringComparer.Ordinal);

            foreach (string name in hotUpdateNames)
            {
                if (!buildAssemblyNames.Contains(name))
                {
                    Debug.LogWarning(
                        $"[FilterHotUpdateAssembly] hot update assembly '{name}' was not found in build assemblies, possibly a typo");
                }
            }

            return assemblies.Where(ass =>
            {
                string assName = Path.GetFileNameWithoutExtension(ass);
                bool keep = !hotUpdateNameSet.Contains(assName);
                if (!keep)
                {
                    Debug.Log($"[FilterHotUpdateAssembly] filter assembly: {assName}");
                }

                return keep;
            }).ToArray();
        }
    }
}
