> Locked baseline: EFT **1.2.5.0.48146**, SPT 5.0. Newer EFT work must use a separate branch and installation. The original `spt-5.0-48146` branch remains available as historical provenance.

# Skipper — local SPT 5.0 / EFT 48146 port

This branch reuses [huaxianmfs's v1 port](https://github.com/huaxianmfs/SPT-Skipper/tree/v1), commit `cd2d5bb0cef3ee850df72f56e889995f5c3e3a92` (port implementation `654e21c`), preserving its history on top of acidphantasm's `b133f2cb1e2dafea438181508df8846b3fa97330`. Our branch is [jpeckham/spt-5.0-eft-1.2.5.0.48146](https://github.com/jpeckham/SPT-Skipper/tree/spt-5.0-eft-1.2.5.0.48146), experimental version **1.2.0**.

We selected the community v1 branch because it retains original trader-objective skipping. The community master branch's later experimental main-story completion/status-forcing logic is not included. Its author reports story-progression risks; this port does not claim to support main-story skipping.

## Build and install this branch

Requires .NET 10 SDK and a working SPT 5.0 / EFT 1.2.5.0.48146 installation with generated IL2CPP references. From this repository root:

```powershell
dotnet build Terkoiz.Skipper.csproj -c Release `
  '-p:SPTPath=C:\sptarkov\SPTarkov 5.0.0-BE-1.2.5.0.48146'
```

The build stages `terkoiz-skipper.dll` and `LICENSE` under `Build/BepInEx/plugins/Skipper`. Close the client/server, back up your save and any previous Skipper plugin/config, then copy those two files into the matching folder under your installation. Remove duplicate older Skipper DLLs. Builds do not automatically install. Game/reference assemblies are not distributed. Uninstall by closing the game and removing the plugin DLL; uninstalling does not undo already saved quest progress.

Local changes: target .NET 10 and this installation's references, require SPT core 5.0, use typed native postfix arguments, retire a row's previous skip button before cloning a replacement, reduce repeated diagnostic messages to Debug, restore build-output ignore rules, and stage locally instead of auto-installing. Exclude the old `project` directory from compilation because IDE-generated files can survive the community port's directory migration and otherwise cause duplicate assembly attributes. Original license/author credit remain.

## Usage and validation

Open a trader's task objectives and hold **Left Ctrl** to reveal SKIP buttons, or enable **Always display Skip button** through F12. Clicking SKIP opens the existing confirmation dialog before completing an objective. Settings live in `BepInEx/config/com.terkoiz.skipper.cfg`. Source defaults: enabled true, always-display false, hotkey LeftControl. In James's installed config, always-display was observed **true** after testing and was preserved.

Build: **0 warnings, 0 errors**. Startup confirmed the quest-view patch, injected behaviour registration, and Update execution. A temporary runtime harness passed exact patch-target resolution, native progress-checker getter and UnityEvent callback execution after managed GC, and the null-objective guard. It uses isolated native objects and does not complete real objectives. Final line-ending cleanup was rebuilt after that smoke test without behavioral changes.

To repeat: build `tests/RuntimeTests.csproj` with the same SPTPath, temporarily install its `Skipper.RuntimeTests.dll` beside the production plugin, launch a test profile, and look for `Skipper Runtime Tests` / `PASS:` in `BepInEx/LogOutput.log`. Stop the client and remove the test DLL afterward. This is an in-client harness, not a `dotnet test` project.

**Unverified:** actual trader UI layout, hotkey/show/hide behavior, confirmation interaction, quest-objective completion and persistence after restarting. No objectives were intentionally skipped during these checks. Use a backed-up test profile to validate those operations. Main-story skipping is outside this branch's scope.

## Community v1 documentation (historical paths/build instructions)

SPT 5.0 的任务目标跳过 Mod。支线任务的任务条件添加一个 SKIP 按钮，点击后可以直接完成该条件。

基于 Terkoiz 原版 Skipper 移植到 SPT 5.0.0_BE

功能
在任务详情窗口的每个任务条件右侧显示一个 SKIP 按钮

按住热键（默认 左 Ctrl）显示所有 SKIP 按钮，松开隐藏

点击 SKIP 弹出确认窗口，确认后跳过对应条件

安装

将 terkoiz-skipper文件夹 放在：

.\BepInEx\plugins\

首次运行时会在以下路径生成配置文件：

<SPT目录>\BepInEx\config\com.terkoiz.skipper.cfg

从源码编译
依赖
.NET SDK 6.0 或更高

已安装的 SPT 5.0（脚本会自动从 TarkovDir 读取游戏 DLL）

编译
直接运行：

bat
build.bat
或手动：

bat
dotnet build Terkoiz.Skipper.csproj -c Release
编译产物会自动复制到：

<TarkovDir>\BepInEx\plugins\Terkoiz.Skipper\terkoiz-skipper.dll
同时在项目目录下生成 release/Terkoiz.Skipper.zip，方便分发。

路径配置
如果你的 SPT 目录不是 D:\SPT-5.0.0-47242-BE\，修改 Terkoiz.Skipper.csproj 里的这一行：

xml
<TarkovDir Condition=" '$(TarkovDir)' == '' ">D:\你的SPT目录\</TarkovDir>

致谢
原 Mod 作者：Terkoiz

SPT 团队

BepInEx 团队

Il2CppInterop 团队
