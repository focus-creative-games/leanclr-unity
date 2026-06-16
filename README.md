# LeanCLR for Unity

语言: [中文](./README.md) | [English](./README_EN.md)

[![GitHub](https://img.shields.io/badge/GitHub-leanclr--unity-181717?logo=github)](https://github.com/focus-creative-games/leanclr-unity) [![Gitee](https://img.shields.io/badge/Gitee-leanclr--unity-C71D23?logo=gitee&logoColor=white)](https://gitee.com/focus-creative-games/leanclr-unity) [![LeanCLR](https://img.shields.io/badge/LeanCLR-Runtime-181717?logo=github)](https://github.com/focus-creative-games/leanclr) [![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/focus-creative-games/leanclr-unity/blob/main/LICENSE) [![Discord](https://img.shields.io/badge/Discord-Join-7289DA?logo=discord&logoColor=white)](https://discord.gg/esAYcM6RDQ)

**leanclr-unity**（包名 `com.code-philosophy.leanclr`）是 [LeanCLR](https://github.com/focus-creative-games/leanclr) 的 Unity 集成包：在发布 WebGL / 小游戏等平台时用 LeanCLR **自动替换 IL2CPP**。

## 相对 IL2CPP 的优势

在配合 `aot.xml` / PGO **选择性 AOT** 的前提下，相对 IL2CPP 全量 AOT 典型表现为：

- **AOT 代码体积**：托管代码编译产生的 AOT 原生代码可减少约 **70%–90%**（仅对热点/必要方法 AOT 时，整体性能影响可控）
- **内存**：虚拟机内存约减少 **20%–35%**，托管内存亦有少量下降
- **大型项目**：支持 [延迟加载](https://doc.leanclr.com/docs/ecosystem/unity/lazy-load) 与 [热更新](https://doc.leanclr.com/docs/ecosystem/unity/hot-update)，在符合小游戏包体要求下可支撑**数十 MB 级**托管代码
- **GC**：准确式 **Mark-Sweep GC**，回收效率更高，GC 更快

详见 [Unity 集成文档](https://doc.leanclr.com/docs/ecosystem/unity/)。

## 文档

完整文档站点：**[https://doc.leanclr.com](https://doc.leanclr.com/)**

| 主题 | 链接 |
|------|------|
| Unity 集成总览 | [doc.leanclr.com/docs/ecosystem/unity/](https://doc.leanclr.com/docs/ecosystem/unity/) |
| 安装 | [安装](https://doc.leanclr.com/docs/ecosystem/unity/install) |
| 项目设置 | [项目设置](https://doc.leanclr.com/docs/ecosystem/unity/settings) |
| 概念辨析（AOT / 延迟加载 / 热更新） | [概念辨析](https://doc.leanclr.com/docs/ecosystem/unity/concepts) |
| 构建与 CompileDll | [构建](https://doc.leanclr.com/docs/ecosystem/unity/build) |
| 代码热更新 | [热更新](https://doc.leanclr.com/docs/ecosystem/unity/hot-update) |
| 运行时与 AOT | [LeanCLR 文档首页](https://doc.leanclr.com/docs/intro/) |

## 快速开始

在 Unity **Package Manager → Add package from git URL** 中填入：

- `https://github.com/focus-creative-games/leanclr-unity.git`
- `https://gitee.com/focus-creative-games/leanclr-unity.git`

安装后打开 **LeanCLR → Settings...** 启用集成，按文档发布 Player 即可。示例工程：[leanclr-unity-demo](https://github.com/focus-creative-games/leanclr-unity-demo)。

## 相关仓库

| 仓库 | 说明 |
|------|------|
| [leanclr](https://github.com/focus-creative-games/leanclr) | LeanCLR 运行时与工具链 |
| [leanclr-doc](https://github.com/focus-creative-games/leanclr-doc) | 文档站源码 |
| [hybridclr](https://github.com/focus-creative-games/hybridclr) | Unity 全平台原生 C# 热更新（定位互补） |

## 支持与联系

- 邮箱：`leanclr#code-philosophy.com`
- Discord：<https://discord.gg/esAYcM6RDQ>
- QQ 群：1047250380

## 许可证

[MIT License](https://github.com/focus-creative-games/leanclr-unity/blob/main/LICENSE)
