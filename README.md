# 汉化包文件说明

## \StickFight_Data

### globalgamemanagers
- 为新增字体建立依赖和容器（包含 fonts 及 materials/xxx sdf）
- 可通过富文本标签 `<font=xxx sdf>` 调用字体

### sharedassets0.assets
- 字体替换（名称保持不变）：
  - Anton -> Anton+SSZhengYaTi
  - Roboto-Bold SDF -> Roboto+MiSans Part I
- 新增字体：
  - Roboto+MiSans Part II（需 mod 前置，另一半中文字体）
  - Anton+MiSans

### level0
- 包含汉化文本
- 玩家名称字体改为 Anton+MiSans（需依赖 sharedassets0.assets，否则会崩溃）


## \BepInEx\plugins\MapEditorCNText.dll
- 对 Roboto+MiSans Part II 字体进行绑定，通过赋值 fallbackfont 实现多字体合并（因贴图大小限制）
- 功能支持：
  - 地图编辑器汉化
  - 聊天支持输入中文字符
  - 注意：对方需有字体才能正常显示（双方都装了汉化包），否则还是会显示为方框 □
  - ~~如果你能够通过其他途径（如日志）获取到聊天文本也可以看到中文~~


### 附：字体说明
- 支持 3953 个中文字符
- ~~理论上支持更多字符，但每增加一个 8192*8192 贴图的字体就会增加 64M~~
