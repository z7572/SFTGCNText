# SFTGCNText
<p align="center">
  <a href="https://github.com/z7572/SFTGCNText/releases/latest">
    <img src="https://img.shields.io/github/downloads/z7572/SFTGCNText/total?label=Github%20downloads&logo=github">
  </a>
</p>


柴油汉化包，解压至 `StickFightTheGame` 文件夹，全部替换

## 文件结构：

你的游戏目录结构看起来应该像这样：

```
StickFightTheGame\
├── BepInEx\
│   ├─cache
│   ├─config
│   ├─core
│   ├─patchers
│   └─plugins --> 所有BepInEx模组都装在这里
│       └── CNText.dll --> 必要的插件
└── StickFight_Data\
    ├── globalgamemanagers --> v2.0+无需替换此文件
    ├── sharedassets0.assets --> v2.0+无需替换此文件
    └── level0 --> 可选的汉化文件
```

## 从 v1.3 迁移到 v2.0：

如果你有原版游戏的文件的备份，可以直接把 `globalgamemanagers` 和 `sharedassets0.assets` 替换成原版游戏的文件；

如果你没有原版游戏的文件备份，请备份好你的游戏目录下的模组、配置等文件，然后Steam校验文件完整性来恢复原始游戏的`globalgamemanagers` 和`sharedassets0.assets`文件；

最终，再把 `level0` 替换为本仓库中提供的版本，然后把 `CNText.dll` 放到 `BepInEx/plugins` 文件夹即可。

## 文件说明(v2.0 更新后)：

新版本直接通过BepInEx插件实现直接从内嵌资源加载TMP字体，无需再修改游戏本体文件，单文件替换更方便，并且扩充到了7500字库，支持更多中文字符

### level0
- 包含主游戏中大部分文本的汉化（除胜利文本、武器名、地图名、BOSS阶段等）
- 不替换这个文件也可以正常运行游戏，此时 `CNText.dll` 只起到字体扩充的作用（以及关卡编辑器的汉化）
  - 或许之后会考虑把这个文件也优化掉？但是每次加载场景都会重新替换文本，性能上可能不如直接替换文件


### CNText.dll
- 一个 **超级大(100M+)** 的 [BepInEx](https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_x86_5.4.23.4.zip) Mod
- 直接从内嵌资源中导入TMP字体，在TMP文本组件初始化时直接替换其字体
- 多种字体（最高16K贴图），7500字库
- 功能支持：
  - 地图编辑器文本汉化
  - 主游戏中部分由代码驱动的文本汉化
  - 聊天支持输入中文字符
    - 注意：对方需有字体才能正常显示（双方都装了汉化包），否则还是会显示为方框 □
  - 玩家名支持显示几乎所有字符
    - 如果有7500字库外的文字，则改用 `Text` 组件和 ttf 字体（富文本格式将只支持`<i>`, `<color=#...>`, `<#...>`）
      - 不支持的字符会直接不显示，无方框
      - 如果有Emoji且没有富文本标签，取消回退到 `Text` 组件，还是用TMP显示Emoji/方框

## 文件说明(v1.3 及之前)：

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
- 一个 [BepInEx](https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.4/BepInEx_x86_5.4.23.4.zip) Mod
- 对 `Roboto+MiSans Part II` 字体进行绑定，通过赋值 `fallbackfont` 实现多字体合并（因贴图大小限制8192x）
- 功能支持：
  - 地图编辑器汉化
  - 聊天支持输入中文字符
    - 注意：对方需有字体才能正常显示（双方都装了汉化包），否则还是会显示为方框 □
    - ~~如果你能够通过[其他途径](https://blog.monblog.top/openlist/stick.plugins.chatrecorder)获取到聊天文本也可以看到中文~~
  - 玩家名支持显示几乎所有字符
    - 4500字库外的文字改用Text和ttf字体（一旦有这些文字，富文本格式将只支持`<i>`, `<color=#...>`, `<#...>`）
      - 不支持的字符会直接不显示，无方框

## 附
- 支持 ~~3953~~ 7046 个中文字符
- ~~理论上支持更多字符，但每增加一个 8192*8192 贴图的字体，游戏本体的资源文件就会增加 64M~~
  - 换成ab包加载后能有效压缩文件体积，16K分辨率、256M大的贴图能直接压缩到60M左右
- ~~也许修改TextMeshPro (Unity编辑器内) 的源码使之支持更大贴图可以把字体合并成一个~~
  - 嗯的确可以改，见[此仓库](https://github.com/z7572/TextMeshPro-1.0.55.56.0b9)
- ~~我在生成完字体后才发现字库里竟然没有 `嗯` 这个常用字 ??? ¯\\\_(ツ)_/¯~~
  - 于是新版直接换成7500字库了（
- 都看到这了，点个Star吧awa
