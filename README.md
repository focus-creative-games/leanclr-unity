# LeanCLR for Unity

语言: [中文](./README.md) | [English](./README_EN.md)

[![GitHub](https://img.shields.io/badge/GitHub-leanclr4unity-181717?logo=github)](https://github.com/focus-creative-games/leanclr4unity) [![LeanCLR](https://img.shields.io/badge/LeanCLR-Runtime-181717?logo=github)](https://github.com/focus-creative-games/leanclr) [![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE) [![Discord](https://img.shields.io/badge/Discord-Join-7289DA?logo=discord&logoColor=white)](https://discord.gg/esAYcM6RDQ)

**leanclr4unity** 是 [LeanCLR](https://github.com/focus-creative-games/leanclr/blob/main/README.md) 的 **Unity 集成包**。

本仓库（包名 `com.code-philosophy.leanclr`）负责 Unity Editor 侧集成、构建管线衔接与发布工作流；**LeanCLR 运行时** 为独立仓库 [LeanCLR](https://github.com/focus-creative-games/leanclr)。

## 为什么使用 LeanCLR for Unity

LeanCLR 是一个面向全平台的精练的 CLR（Common Language Runtime）实现。LeanCLR 的设计目标是在高度符合 ECMA-335 规范的前提下，提供更紧凑、易嵌入、低内存占用的运行时，实现对移动端、H5 与小游戏等资源受限平台的友好支持。Unity发布到WebGL和小游戏平台时
存在wasm文件过大，内存占用过高的问题。使用LeanCLR替换il2cpp，可以显著减少构建后的wasm文件大小，同时降低元数据内存和托管内存的开销。

更完整的背景、与 CoreCLR / Mono / IL2CPP 的对比、路线图与模块进度，请参阅 **[LeanCLR README](https://github.com/focus-creative-games/leanclr/blob/main/README.md)**。

## 文档

### 支持的Unity版本和平台

- 暂时支持Windows平台Unity Editor，后续版本会支持MacOS和linux
- 支持Unity 2019- Unity 6000 所有版本（含LTS和非LTS版本）
- 支持团结引擎所有版本
- 支持WebGL和MiniGame平台
- 部分支持其他平台（如Win64），但仅支持单线程，并且**可能**需要手动修改相应平台的构建工程文件（如Unity 6000 发布Win64平台时需要在Il2CppOutputProject.vcxproj文件中移除il2cpp的命令行参数`--static-lib-il2-cpp`，否则启动时会有 `il2cpp init failed`错误。）

### 限制

由于目前还处于早期版本，某些方面实现还不完整。

- **不支持GC**，但后续版本很快会跟进
- 只支持单线程
- 仅支持mscorlib和Unity引擎的dll中引入的pinvoke函数，不支持用户程序集中定义pinvoke函数。

### 示例项目

- [leanclr4unity_demo](https://github.com/focus-creative-games/leanclr4unity_demo)

### 安装

在Unity Package Manager中点击`Add package from git URL...`，填入 `https://github.com/focus-creative-games/leanclr4unity.git` 即可完成安装。

### 设置

- `LeanCLR/Settings`菜单打开设置页面，当前仅可以设置开启或者禁用本插件。

### 构建

无需任何操作，本插件会自动在发布时使用leanclr替换il2cpp。

## 相关仓库

| 仓库 | 说明 |
|------|------|
| [leanclr](https://github.com/focus-creative-games/leanclr) | LeanCLR 运行时与工具链（C++17，零外部依赖） |
| [hybridclr](https://github.com/focus-creative-games/hybridclr)| HybridCLR是一个特性完整、零成本、高性能、低内存的Unity全平台原生c#热更新解决方案 |

## 支持与联系

- 邮箱：`leanclr#code-philosophy.com`
- Discord：<https://discord.gg/esAYcM6RDQ>
- QQ 群：1047250380

## 许可证

使用 [MIT License](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE)。
