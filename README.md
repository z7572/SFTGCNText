# SFTGCNText
<p align="center">
  <a href="https://github.com/z7572/SFTGCNText/releases/latest">
    <img src="https://img.shields.io/github/downloads/z7572/SFTGCNText/total?label=Github%20downloads&logo=github">
  </a>
</p>


柴油汉化包，解压至 `StickFightTheGame` 文件夹，全部替换

## 文件结构：

```
StickFightTheGame\
├── StickFight_Data\
│   ├── globalgamemanagers
│   ├── sharedassets0.assets
│   └── level0
└── BepInEx\
    └── plugins\
        └── CNText.dll
```

## 文件说明：

### globalgamemanagers
- 为新增字体建立依赖和容器（包含 `fonts` 及 `materials/xxx sdf`）
- 可通过富文本标签 `<font=xxx sdf>` 调用字体

### sharedassets0.assets
- 字体替换（名称保持不变）：
  - `Anton` -> `Anton+SSZhengYaTi`
  - `Roboto-Bold SDF` -> `Roboto+MiSans Part I`
- 新增字体：
  - `Roboto+MiSans Part II`（需 Mod 前置）
  - `Anton+MiSans`

### level0
- 包含汉化文本
- 玩家名称字体改为 `Anton+MiSans`（需依赖 `sharedassets0.assets`，否则会崩溃）

### CNText.dll
- 一个 [`BepInEx`](https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_x86_5.4.23.4.zip) Mod
- 对 `Roboto+MiSans Part II` 字体进行绑定，通过赋值 `fallbackfont` 实现多字体合并（因贴图大小限制8192x）
- 功能支持：
  - 地图编辑器汉化
  - 聊天支持输入中文字符
    - 注意：对方需有字体才能正常显示（双方都装了汉化包），否则还是会显示为方框 □
    - ~~如果你能够通过其他途径（如日志）获取到聊天文本也可以看到中文~~
  - 玩家名支持显示所有字符
    - 4500字库外的文字改用Text和ttf字体（一旦有这些文字，富文本格式将只支持`<i>`, `<color=#...>`, `<#...>`）
      - 不支持的字符会直接不显示，无方框

## 附
- 支持 3953 个中文字符
- ~~理论上支持更多字符，但每增加一个 8192*8192 贴图的字体，游戏本体的资源文件就会增加 64M~~
- ~~也许修改TextMeshPro (Unity编辑器内) 的源码使之支持更大贴图可以把字体合并成一个~~
- ~~我在生成完字体后才发现字库里竟然没有 `嗯` 这个常用字 ???~~ ¯\\\_(ツ)_/¯
