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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace LeanCLR.BuildProcessors
{
    internal class UpdateUnityVersionMacrosInCpp : IPreprocessBuildWithReport
    {
        public int callbackOrder => 2;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!Settings.EnableForCurrentBuildTarget)
            {
                return;
            }
            UpdateUnityVersion();
        }

        public static void UpdateUnityVersion()
        {
            var installerController = new LocalInstaller();
            if (!installerController.HasInstalledToLocal())
            {
                throw new Exception($"Please install LeanCLR first.");
            }

            string unityVersionFilePath = $"{Settings.LocalLibil2cppPath}/il2cpp/unityversion.h";

            var ver = installerController.CurVersion;
            var codes = new List<string>();
            codes.Add("#pragma once");
            codes.Add("");
            codes.Add($"#define UNITY_VERSION {ver.major:D4}{ver.minor1:D2}{ver.minor2:D2}");
            if (ver.isTuanjieEngine)
            {
                codes.Add($"#define UNITY_TUANJIE_ENGINE 1");
            }
            codes.Add("");
            File.WriteAllLines(unityVersionFilePath, codes, new UTF8Encoding(false));
            Debug.Log($"[UpdateUnityVersion] update unity version to {unityVersionFilePath}");
        }
    }
}
