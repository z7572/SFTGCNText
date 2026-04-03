using BepInEx.Configuration;
using UnityEngine;

namespace CNText;

public static class ConfigHandler
{
    public static ConfigEntry<bool> needReplaceTextComp;
    public static ConfigEntry<int> customTextFontSize;
    public static ConfigEntry<bool> removeUnsupportedTags;
    public static ConfigEntry<bool> closeUnclosedTags;

    public static void InitConfigs(ConfigFile config)
    {
        needReplaceTextComp = config.Bind("OnlinePlayerUI", "NeedReplaceTextComp", true, "是否显示替换的头顶玩家名称文本组件");
        customTextFontSize = config.Bind("OnlinePlayerUI", "CustomTextFontSize", 40, "替换后头顶玩家名称字号");
        removeUnsupportedTags = config.Bind("OnlinePlayerUI", "RemoveUnsupportedTags", true, "是否移除不支持的标签");
        closeUnclosedTags = config.Bind("OnlinePlayerUI", "CloseUnclosedTags", true, "是否自动闭合未闭合的标签");
    }
}
