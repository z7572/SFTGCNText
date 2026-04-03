using HarmonyLib;
using TMPro;
using UnityEngine;
using System;

namespace CNText;

[HarmonyPatch]
public static class TargetedFontReplacePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TextMeshPro), "Awake")]
    public static void TMPAwakePostfix(TextMeshPro __instance)
    {
        if (__instance.font == null) return;

        string fontName = __instance.font.name;

        if (fontName.Equals("Anton SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Anton_SSZY != null)
            {
                __instance.font = CNText.Anton_SSZY;
            }
        }
        else if (fontName.Equals("Roboto-Bold SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Roboto_MiSans != null)
            {
                __instance.font = CNText.Roboto_MiSans;
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TextMeshProUGUI), "Awake")]
    public static void TMPUGUIAwakePostfix(TextMeshProUGUI __instance)
    {
        if (__instance.font == null) return;

        string fontName = __instance.font.name;

        if (fontName.Equals("Anton SDF", StringComparison.OrdinalIgnoreCase) ||
            fontName.Equals("MiSans SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (__instance.transform.parent != null &&
                __instance.transform.parent.GetComponent<OnlinePlayerUI>() != null)
            {
                if (CNText.Impact_MiSans != null)
                {
                    __instance.font = CNText.Impact_MiSans;
                }
            }
            else
            {
                if (CNText.Anton_SSZY != null)
                {
                    __instance.font = CNText.Anton_SSZY;
                }
            }
        }
        else if (fontName.Equals("Roboto-Bold SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Roboto_MiSans != null)
            {
                __instance.font = CNText.Roboto_MiSans;
            }
        }
    }

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(Resources), "Load", new Type[] { typeof(string), typeof(Type) })]
    //public static bool ResourcesLoadPrefix(string path, Type systemTypeInstance, ref UnityEngine.Object __result)
    //{
    //    // 如果请求加载的是 TMP 字体
    //    if (systemTypeInstance == typeof(TMP_FontAsset))
    //    {
    //        if (path.IndexOf("Anton SDF", StringComparison.OrdinalIgnoreCase) >= 0)
    //        {
    //            // 动态加载时没有 GameObject 父级上下文，所以返回通用的 Anton 替换
    //            if (CNText.Anton_SSZY != null)
    //            {
    //                __result = CNText.Anton_SSZY;
    //                return false;
    //            }
    //        }
    //        else if (path.IndexOf("Roboto-Bold SDF", StringComparison.OrdinalIgnoreCase) >= 0 ||
    //                 path.IndexOf("roboto-bold sdf ii", StringComparison.OrdinalIgnoreCase) >= 0)
    //        {
    //            if (CNText.Roboto_MiSans != null)
    //            {
    //                __result = CNText.Roboto_MiSans;
    //                return false;
    //            }
    //        }
    //    }
    //    return true;
    //}
}