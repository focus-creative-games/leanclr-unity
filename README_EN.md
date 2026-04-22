# LeanCLR for Unity

Languages: [中文](./README.md) | [English](./README_EN.md)

[![GitHub](https://img.shields.io/badge/GitHub-leanclr4unity-181717?logo=github)](https://github.com/focus-creative-games/leanclr4unity) [![LeanCLR](https://img.shields.io/badge/LeanCLR-Runtime-181717?logo=github)](https://github.com/focus-creative-games/leanclr) [![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE) [![Discord](https://img.shields.io/badge/Discord-Join-7289DA?logo=discord&logoColor=white)](https://discord.gg/esAYcM6RDQ)

**leanclr4unity** is the **Unity integration package** for [LeanCLR](https://github.com/focus-creative-games/leanclr/blob/main/README.md).

This repository (package name `com.code-philosophy.leanclr`) is responsible for Unity Editor-side integration, build pipeline wiring, and release workflows. The **LeanCLR runtime** itself is maintained in a separate repository: [LeanCLR](https://github.com/focus-creative-games/leanclr).

## Why LeanCLR for Unity

LeanCLR is a lightweight, cross-platform implementation of CLR (Common Language Runtime). Its core goal is to provide a runtime that remains highly compliant with ECMA-335 while being more compact, easier to embed, and lower in memory footprint, making it suitable for resource-constrained targets such as mobile, H5, and mini-game platforms. When publishing Unity projects to WebGL or mini-game platforms, IL2CPP often introduces large wasm outputs and high memory overhead. Replacing IL2CPP with LeanCLR can significantly reduce post-build wasm size and lower both metadata and managed-memory costs.

For a complete background, comparisons with CoreCLR / Mono / IL2CPP, roadmap, and module progress, see the **[LeanCLR README](https://github.com/focus-creative-games/leanclr/blob/main/README.md)**.

## Documentation

### Supported Unity versions and platforms

- Unity Editor on Windows is currently supported. macOS and Linux support will be added in future releases.
- Supports all Unity versions from Unity 2019 to Unity 6000 (including both LTS and non-LTS releases).
- Supports all versions of Tuanjie Engine.
- Supports WebGL and MiniGame platforms.
- Other platforms (for example, Win64) are partially supported, but only in single-thread mode. Manual edits to generated platform build project files **may** be required. For example, when publishing Win64 on Unity 6000, remove IL2CPP command-line argument `--static-lib-il2-cpp` from `Il2CppOutputProject.vcxproj`; otherwise startup may fail with `il2cpp init failed`.

### Limitations

Because this is still an early version, some capabilities are not fully implemented yet.

- **GC is not supported yet**, but planned for an upcoming release.
- Single-thread execution only.
- Only P/Invoke functions introduced by `mscorlib` and Unity engine DLLs are supported. P/Invoke functions defined in user assemblies are not supported.

### Sample project

- [leanclr4unity_demo](https://github.com/focus-creative-games/leanclr4unity_demo)

### Installation

In Unity Package Manager, click `Add package from git URL...`, then enter `https://github.com/focus-creative-games/leanclr4unity.git`.

### Settings

- Open the settings page from the `LeanCLR/Settings` menu. Currently, the plugin can only be enabled or disabled.

### Build

No manual action is required. During release builds, this plugin automatically uses LeanCLR to replace IL2CPP.

## Related repositories

| Repository | Description |
|------|------|
| [leanclr](https://github.com/focus-creative-games/leanclr) | LeanCLR runtime and toolchain (C++17, zero external dependencies) |
| [hybridclr](https://github.com/focus-creative-games/hybridclr)| HybridCLR is a feature-complete, zero-overhead, high-performance, low-memory native C# hot-update solution for Unity across all platforms |

## Support and contact

- Email: `leanclr#code-philosophy.com`
- Discord: <https://discord.gg/esAYcM6RDQ>
- QQ Group: 1047250380

## License

Distributed under the [MIT License](https://github.com/focus-creative-games/leanclr4unity/blob/main/LICENSE).
