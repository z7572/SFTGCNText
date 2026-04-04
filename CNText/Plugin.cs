using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace CNText;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class CNText : BaseUnityPlugin
{
    public static Font Impact_MiSans_ttf;

    public static TMP_FontAsset Anton_SSZY;
    public static TMP_FontAsset Impact_MiSans;
    public static TMP_FontAsset Roboto_MiSans;


    public void Awake()
    {
        Logger.LogInfo("汉化包加载中...");
        ConfigHandler.InitConfigs(Config);

        try
        {
            var ab_impact = Helper.GetAssetBundle(Assembly.GetExecutingAssembly(), "impact+misans");
            Impact_MiSans = ab_impact?.LoadAsset<TMP_FontAsset>("Impact+MiSans SDF");
            Helper.RegisterCustomFont(Impact_MiSans);
            Impact_MiSans_ttf = ab_impact?.LoadAsset<Font>("Impact+MiSans");

            var ab_anton = Helper.GetAssetBundle(Assembly.GetExecutingAssembly(), "anton+sszy");
            Anton_SSZY = ab_anton?.LoadAsset<TMP_FontAsset>("Anton+SSZhengYaTi SDF");
            Helper.RegisterCustomFont(Anton_SSZY);

            var ab_roboto = Helper.GetAssetBundle(Assembly.GetExecutingAssembly(), "roboto+misans");
            Roboto_MiSans = ab_roboto?.LoadAsset<TMP_FontAsset>("Roboto+MiSans SDF");
            Helper.RegisterCustomFont(Roboto_MiSans);
        }
        catch (Exception e)
        {
            Logger.LogError("字体加载失败！请确保汉化包完整安装！");
            Logger.LogError(e);
            return;
        }

        var sdf = Resources.Load<TMP_FontAsset>("fonts & materials/roboto-bold sdf");
        var sdf2 = Resources.Load<TMP_FontAsset>("fonts & materials/roboto-bold sdf ii");
        if (sdf2 != null)
        {
            if (sdf.fallbackFontAssets[0] == null || sdf.fallbackFontAssets[0] != sdf2)
            {
                sdf.fallbackFontAssets[0] = sdf2;
            }
            Logger.LogInfo("安装了旧版汉化包，已补全fallback字体");
        }
        else
        {
            Logger.LogInfo("未安装旧版汉化包");
        }

        try
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("汉化包加载成功！");
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
    }

    public const string PLUGIN_GUID = "z7572.CNText";
    public const string PLUGIN_NAME = "CNText";
    public const string PLUGIN_VERSION = "2.0";
}