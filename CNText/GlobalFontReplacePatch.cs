using HarmonyLib;
using TMPro;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace CNText;

/// <summary>
/// 
/// </summary>
[HarmonyPatch]
public static class GlobalFontReplacePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TextMeshPro), "Awake")]
    public static void TMPAwakePostfix(TextMeshPro __instance)
    {
        if (__instance.font == null) return;

        var fontName = __instance.font.name;

        if (fontName.Equals("MiSans SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Impact_MiSans != null)
            {
                __instance.font = CNText.Impact_MiSans;
            }
        }
        else if (fontName.Equals("Anton SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Anton_SSZY != null)
            {
                __instance.font = CNText.Anton_SSZY;
            }
        }
        else if (fontName.Equals("Roboto-Bold SDF", StringComparison.OrdinalIgnoreCase) ||
                 fontName.Equals("roboto-bold sdf ii", StringComparison.OrdinalIgnoreCase))
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

        var fontName = __instance.font.name;

        if (fontName.Equals("MiSans SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Impact_MiSans != null)
            {
                __instance.font = CNText.Impact_MiSans;
            }
        }
        else if (fontName.Equals("Anton SDF", StringComparison.OrdinalIgnoreCase))
        {
            if (__instance.transform.parent != null &&
                __instance.transform.parent.GetComponent<OnlinePlayerUI>() != null &&
                ConfigHandler.needReplaceTextComp.Value)
            {
                if (CNText.Impact_MiSans != null)
                {
                    __instance.font = CNText.Impact_MiSans;
                    __instance.fontSize = ConfigHandler.customTextFontSize.Value;
                    __instance.fontStyle = FontStyles.Normal;
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
        else if (fontName.Equals("Roboto-Bold SDF", StringComparison.OrdinalIgnoreCase) ||
                 fontName.Equals("Roboto-Bold SDF II", StringComparison.OrdinalIgnoreCase))
        {
            if (CNText.Roboto_MiSans != null)
            {
                __instance.font = CNText.Roboto_MiSans;
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Resources), "Load", typeof(string), typeof(Type))]
    public static bool ResourcesLoadPrefix(string path, Type systemTypeInstance, ref Object __result)
    {
        if (systemTypeInstance == typeof(TMP_FontAsset))
        {
            if (path.IndexOf("MiSans SDF", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (CNText.Impact_MiSans != null)
                {
                    __result = CNText.Impact_MiSans;
                    return false;
                }
            }
            else if (path.IndexOf("Anton SDF", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (CNText.Anton_SSZY != null)
                {
                    __result = CNText.Anton_SSZY;
                    return false;
                }
            }
            else if (path.IndexOf("Roboto-Bold SDF", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     path.IndexOf("Roboto-Bold SDF II", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (CNText.Roboto_MiSans != null)
                {
                    __result = CNText.Roboto_MiSans;
                    return false;
                }
            }
        }
        return true;
    }
}