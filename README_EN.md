# LeanCLR for Unity

Languages: [中文](./README.md) | [English](./README_EN.md)

[![GitHub](https://img.shields.io/badge/GitHub-leanclr4unity-181717?logo=github)](https://github.com/focus-creative-games/leanclr4unity) [![Gitee](https://img.shields.io/badge/Gitee-leanclr4unity-C71D23?logo=gitee&logoColor=white)](https://gitee.com/focus-creative-games/leanclr4unity) [![LeanCLR](https://img.shields.io/badge/LeanCLR-Runtime-181717?logo=github)](https://github.com/focus-creative-games/leanclr) [![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE) [![Discord](https://img.shields.io/badge/Discord-Join-7289DA?logo=discord&logoColor=white)](https://discord.gg/esAYcM6RDQ)

**leanclr4unity** is the **Unity integration package** for [LeanCLR](https://github.com/focus-creative-games/leanclr/blob/main/README.md).

This repository (package name `com.code-philosophy.leanclr`) is responsible for Unity Editor-side integration, build pipeline wiring, and release workflows. The **LeanCLR runtime** itself is maintained in a separate repository: [LeanCLR](https://github.com/focus-creative-games/leanclr).

## Why LeanCLR for Unity

LeanCLR is a lightweight, cross-platform implementation of CLR (Common Language Runtime). Its core goal is to provide a runtime that remains highly compliant with ECMA-335 while being more compact, easier to embed, and lower in memory footprint, making it suitable for resource-constrained targets such as mobile, H5, and mini-game platforms. When publishing Unity projects to WebGL or mini-game platforms, IL2CPP often introduces large wasm outputs and high memory overhead. Replacing IL2CPP with LeanCLR can significantly reduce post-build wasm size and lower both metadata and managed-memory costs.

For a complete background, comparisons with CoreCLR / Mono / IL2CPP, roadmap, and module progress, see the **[LeanCLR README](https://github.com/focus-creative-games/leanclr/blob/main/README.md)**.

## Documentation

### Supported Unity versions and platforms

- Supports Unity Editor on Windows and macOS.
- Supports all Unity versions from Unity 2021 to Unity 6000 (including both LTS and non-LTS releases).
- Supports all versions of Tuanjie Engine.
- Supports WebGL and MiniGame platforms.
- Supports all mini-game platforms supported by Unity, including WeChat Mini Game and Douyin Mini Game.
- Other platforms (for example, Win64) are partially supported, but only in single-thread mode. Manual edits to generated platform build project files **may** be required. For example, when publishing Win64 on Unity 6000, remove IL2CPP command-line argument `--static-lib-il2-cpp` from `Il2CppOutputProject.vcxproj`; otherwise startup may fail with `il2cpp init failed`.

### Limitations

- **GC is under development and is expected to be officially supported in June 2026.**
- Single-thread execution only.

### Sample project

- [leanclr4unity_demo](https://github.com/focus-creative-games/leanclr4unity_demo)

### Installation

In Unity Package Manager, click `Add package from git URL...`, then use either of the following:

- `https://github.com/focus-creative-games/leanclr4unity.git`
- `https://gitee.com/focus-creative-games/leanclr4unity.git`

### Settings

Open the project settings page via **`LeanCLR/Settings...`** (or **Edit > Project Settings > LeanCLR**). Settings are stored in **`ProjectSettings/LeanCLR.asset`** and are saved automatically when you leave the settings UI.

#### Main toggle

- **`enable`**: Turns LeanCLR integration on or off. When off, builds do not use this package’s IL2CPP / LeanCLR pipeline.

#### Lean AOT (`leanAOTSettings`)

Options for **Lean AOT (leanaot)** during packaging. If you leave the nested object unset, built-in defaults apply (layout validation off, no rule files, no lazy-load assembly names).

- **`layoutValidation`**: Enables layout-related checks to catch inconsistencies with native layouts earlier; turn on mainly for diagnostics or stricter validation workflows.
- **`ruleFiles`**: Configures the list of **AOT rule file** paths (multiple entries allowed). For rule file format, elements, and semantics, see **[`Docs~/aot-rule-file.md`](./Docs~/aot-rule-file.md)** in this package. Each path may be relative to the **Unity project root** (the folder that contains `Assets`) or an absolute path on disk. Missing files fail the build at preprocess time. Leave the list empty or unset to skip external rule files.
- **`lazyLoadAssemblyNames`**: Assemblies in this list are **not** written into **global-metadata.dat** at build time; load them yourself at runtime (for example with **`Assembly.Load`**). They **still take part in AOT compilation**.

### Build

No manual action is required. During release builds, this plugin automatically uses LeanCLR to replace IL2CPP.

### Lazy Loaded Assembly Limitations

:::tip
The following limitations are temporary; we expect to resolve them soon.

:::

- **Do not** attach scripts from lazy-load assemblies to scenes, prefabs, asset bundles, or other assets; otherwise you will get Missing Script errors. For the underlying reason, see [HybridCLR — MonoBehaviour support](https://www.hybridclr.cn/docs/basic/monobehaviour).
- If a lazy-loaded assembly also has some code compiled into AOT (i.e., AOT is not fully disabled for that assembly in aot.xml), loading the lazy-loaded assembly at runtime requires it to match the **trimmed AOT DLL produced during the build** exactly. You cannot use assemblies from Compile Dll directly; you must use the trimmed AOT DLL from the build output. If you have HybridCLR experience, this is analogous to using DLLs from the `AssemblyPostStripped` directory produced during packaging. leanclr4unity does not currently copy these AOT DLLs for you automatically; you need to find and copy them manually from `Library/Bee/artifacts`.

## Related repositories

| Repository | Description |
|------|------|
| [leanclr](https://github.com/focus-creative-games/leanclr) | LeanCLR runtime and toolchain (C++11, zero external dependencies) |
| [hybridclr](https://github.com/focus-creative-games/hybridclr)| HybridCLR is a feature-complete, zero-overhead, high-performance, low-memory native C# hot-update solution for Unity across all platforms |

## Support and contact

- Email: `leanclr#code-philosophy.com`
- Discord: <https://discord.gg/esAYcM6RDQ>
- QQ Group: 1047250380

## License

Distributed under the [MIT License](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE).
