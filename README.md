# LeanCLR for Unity

语言: [中文](./README.md) | [English](./README_EN.md)

[![GitHub](https://img.shields.io/badge/GitHub-leanclr4unity-181717?logo=github)](https://github.com/focus-creative-games/leanclr4unity) [![Gitee](https://img.shields.io/badge/Gitee-leanclr4unity-C71D23?logo=gitee&logoColor=white)](https://gitee.com/focus-creative-games/leanclr4unity) [![LeanCLR](https://img.shields.io/badge/LeanCLR-Runtime-181717?logo=github)](https://github.com/focus-creative-games/leanclr) [![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE) [![Discord](https://img.shields.io/badge/Discord-Join-7289DA?logo=discord&logoColor=white)](https://discord.gg/esAYcM6RDQ)

**leanclr4unity** 是 [LeanCLR](https://github.com/focus-creative-games/leanclr/blob/main/README.md) 的 **Unity 集成包**。

本仓库（包名 `com.code-philosophy.leanclr`）负责 Unity Editor 侧集成、构建管线衔接与发布工作流；**LeanCLR 运行时** 为独立仓库 [LeanCLR](https://github.com/focus-creative-games/leanclr)。

## 为什么使用 LeanCLR for Unity

LeanCLR 是一个面向全平台的精练的 CLR（Common Language Runtime）实现。LeanCLR 的设计目标是在高度符合 ECMA-335 规范的前提下，提供更紧凑、易嵌入、低内存占用的运行时，实现对移动端、H5 与小游戏等资源受限平台的友好支持。Unity发布到WebGL和小游戏平台时
存在wasm文件过大，内存占用过高的问题。使用LeanCLR替换il2cpp，可以显著减少构建后的wasm文件大小，同时降低元数据内存和托管内存的开销。

更完整的背景、与 CoreCLR / Mono / IL2CPP 的对比、路线图与模块进度，请参阅 **[LeanCLR README](https://github.com/focus-creative-games/leanclr/blob/main/README.md)**。

## 文档

### 支持的Unity版本和平台

- 支持Windows和MacOS平台Unity Editor
- 支持Unity 2021 - 6000 所有版本（含LTS和非LTS版本）
- 支持团结引擎所有版本
- 支持WebGL和MiniGame平台
- 支持微信小游戏、抖音小游戏等所有Unity支持的小游戏平台
- 部分支持其他平台（如Win64），但仅支持单线程，并且**可能**需要手动修改相应平台的构建工程文件（如Unity 6000 发布Win64平台时需要在Il2CppOutputProject.vcxproj文件中移除il2cpp的命令行参数`--static-lib-il2-cpp`，否则启动时会有 `il2cpp init failed`错误。）

### 限制

- **GC正在开发中，预计在2026年6月份正式支持**
- 只支持单线程

### 示例项目

- [leanclr4unity_demo](https://github.com/focus-creative-games/leanclr4unity_demo)

### 安装

在Unity Package Manager中点击`Add package from git URL...`，填入以下任一地址即可完成安装：

- `https://github.com/focus-creative-games/leanclr4unity.git`
- `https://gitee.com/focus-creative-games/leanclr4unity.git`

### 设置

通过菜单 **`LeanCLR/Settings...`**（或 **Edit > Project Settings > LeanCLR**）打开项目设置页。配置保存在 **`ProjectSettings/LeanCLR.asset`**，离开设置页时会自动保存。

#### 主开关

- **`enable`**：是否启用 LeanCLR。关闭后，打包时不会走本包集成的 IL2CPP / LeanCLR 流程。

#### Lean AOT（`leanAOTSettings`）

打包阶段 **Lean AOT（leanaot）** 相关选项。若未展开配置，则使用内置默认值（布局校验关闭，规则文件与延迟加载程序集列表为空）。

- **`layoutValidation`**：是否开启类型布局等相关校验，便于在开发期尽早发现与原生布局不一致的问题；一般仅在需要排查或严格校验时开启。
- **`ruleFiles`**：用于配置 **AOT 规则文件**路径列表（可多个）。规则文件的格式、节点与语义说明见包内文档 **[`Docs~/aot-rule-file.md`](./Docs~/aot-rule-file.md)**。每项路径可为相对 **Unity 工程根目录**（与 `Assets` 同级）的相对路径，或本机绝对路径；打包前会校验文件存在，不存在则构建失败。留空或不配置列表即不使用外部规则文件。
- **`lazyLoadAssemblyNames`**：列表中的程序集在构建时**不会**写入 **global-metadata.dat**，运行时需你自行 **`Assembly.Load`** 等方式加载。这些程序集**仍会参与 AOT 编译**。

### 构建

无需任何操作，本插件会自动在发布时使用leanclr替换il2cpp。

### Lazy Loaed Assembly 的限制

:::tip
以下限制都只是暂时的，我们将很快会解决这些问题。

:::

- **不要**在场景、Prefab、AB 等资源上挂载 lazy 程序集里的脚本，否则会出现 Missing Script。原理见 [HybridCLR — MonoBehaviour 支持](https://www.hybridclr.cn/docs/basic/monobehaviour)。
- 如果Lazy Loaded也有部分代码被编译到aot（即没有在aot.xml中对该程序集禁用aot），则要求加载Lazy Loaded程序集必须跟构建过程中的生成的裁剪后的aot dll完全一致。因此并不能直接使用 Compile Dll生成的程序集，必须使用打包时生成的裁剪后的aot dll。如果你有hybridclr使用经验，可以理解为lazy loaded程序集必须来自打包过程中生成的AssemblyPostStripped目录下的dll。目前leanclr4unity并没有主动复制构建过程中生成的aot dll，因此需要手动从 `Library/Bee/artifacts` 目录下查找并复制。

## 相关仓库

| 仓库 | 说明 |
|------|------|
| [leanclr](https://github.com/focus-creative-games/leanclr) | LeanCLR 运行时与工具链（C++11，零外部依赖） |
| [hybridclr](https://github.com/focus-creative-games/hybridclr)| HybridCLR是一个特性完整、零成本、高性能、低内存的Unity全平台原生c#热更新解决方案 |

## 支持与联系

- 邮箱：`leanclr#code-philosophy.com`
- Discord：<https://discord.gg/esAYcM6RDQ>
- QQ 群：1047250380

## 许可证

使用 [MIT License](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE)。
