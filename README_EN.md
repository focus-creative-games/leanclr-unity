# LeanCLR for Unity

Languages: [中文](./README.md) | [English](./README_EN.md)

[![GitHub](https://img.shields.io/badge/GitHub-leanclr--unity-181717?logo=github)](https://github.com/focus-creative-games/leanclr-unity) [![Gitee](https://img.shields.io/badge/Gitee-leanclr--unity-C71D23?logo=gitee&logoColor=white)](https://gitee.com/focus-creative-games/leanclr-unity) [![LeanCLR](https://img.shields.io/badge/LeanCLR-Runtime-181717?logo=github)](https://github.com/focus-creative-games/leanclr) [![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/focus-creative-games/leanclr-unity/blob/main/LICENSE) [![Discord](https://img.shields.io/badge/Discord-Join-7289DA?logo=discord&logoColor=white)](https://discord.gg/esAYcM6RDQ)

**leanclr-unity** (package `com.code-philosophy.leanclr`) is the Unity integration package for [LeanCLR](https://github.com/focus-creative-games/leanclr). It **replaces IL2CPP with LeanCLR** when you ship to WebGL, mini-game, and similar targets.

## Advantages over IL2CPP

With selective AOT via `aot.xml` / PGO, compared to IL2CPP full AOT you typically get:

- **AOT code size**: about **70%–90%** less native code from managed compilation (hot/necessary methods AOT’d; rest interpreted with controllable performance impact)
- **Memory**: about **20%–35%** lower VM memory, plus a modest drop in managed heap use
- **Large projects**: [lazy load](https://doc.leanclr.com/docs/ecosystem/unity/lazy-load) and [hot update](https://doc.leanclr.com/docs/ecosystem/unity/hot-update) support **tens of MB** of managed logic while meeting mini-game package limits
- **GC**: precise **mark-sweep** GC with higher reclamation efficiency and faster collections

See the [Unity integration docs](https://doc.leanclr.com/docs/ecosystem/unity/).

## Documentation

Full documentation: **[https://doc.leanclr.com](https://doc.leanclr.com/)**

| Topic | Link |
|-------|------|
| Unity integration overview | [doc.leanclr.com/docs/ecosystem/unity/](https://doc.leanclr.com/docs/ecosystem/unity/) |
| Installation | [Install](https://doc.leanclr.com/docs/ecosystem/unity/install) |
| Project settings | [Settings](https://doc.leanclr.com/docs/ecosystem/unity/settings) |
| Concepts (AOT / lazy load / hot update) | [Concepts](https://doc.leanclr.com/docs/ecosystem/unity/concepts) |
| Build & CompileDll | [Build](https://doc.leanclr.com/docs/ecosystem/unity/build) |
| Code hot update | [Hot update](https://doc.leanclr.com/docs/ecosystem/unity/hot-update) |
| Runtime & AOT | [LeanCLR docs home](https://doc.leanclr.com/docs/intro/) |

## Quick start

In Unity **Package Manager → Add package from git URL**, use:

- `https://github.com/focus-creative-games/leanclr-unity.git`
- `https://gitee.com/focus-creative-games/leanclr-unity.git`

After install, open **LeanCLR → Settings...**, enable the integration, and follow the docs to publish a Player. Sample project: [leanclr-unity-demo](https://github.com/focus-creative-games/leanclr-unity-demo).

## Related repositories

| Repository | Description |
|------------|-------------|
| [leanclr](https://github.com/focus-creative-games/leanclr) | LeanCLR runtime and toolchain |
| [leanclr-doc](https://github.com/focus-creative-games/leanclr-doc) | Documentation site source |
| [hybridclr](https://github.com/focus-creative-games/hybridclr) | Unity native C# hot update (complementary) |

## Support and contact

- Email: `leanclr#code-philosophy.com`
- Discord: <https://discord.gg/esAYcM6RDQ>
- QQ Group: 1047250380

## License

[MIT License](https://github.com/focus-creative-games/leanclr-unity/blob/main/LICENSE)
