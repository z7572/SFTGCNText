using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CNText;

public static class Helper
{
    public static void ReplaceLdstr(CodeInstruction code, string target, string replaceWith)
    {
        if (code.opcode == OpCodes.Ldstr && code.operand.ToString() == target)
        {
            code.operand = replaceWith;
            //Debug.Log($"Replaced {target} with {replaceWith}");
        }
    }

    // https://github.com/Infinite-75/PVZRHCustomization/blob/master/BepInEx/CustomizeLib.BepInEx/CustomCore.cs#L67
    public static AssetBundle GetAssetBundle(Assembly assembly, string name)
    {
        var logger = BepInEx.Logging.Logger.CreateLogSource("CNText");
        try
        {
            using Stream stream = assembly.GetManifestResourceStream(assembly.FullName!.Split(',')[0] + "." + name) ?? assembly.GetManifestResourceStream(name)!;
            using MemoryStream stream1 = new();
            stream.CopyTo(stream1);
            var ab = AssetBundle.LoadFromMemory(stream1.ToArray());
            logger.LogInfo($"加载 AssetBundle {name} 成功.");

            return ab;
        }
        catch (Exception e)
        {
            logger.LogError(e.Source);
            logger.LogError($"加载 AssetBundle {name} 失败：\n{e}");
            return null;
        }
    }

    public static Text TMP2FontText(TextMeshProUGUI textMeshProUGUI, Font font = null, string nameOfText = "Text")
    {
        var transform = textMeshProUGUI.transform;
        var transform2 = transform.Find(nameOfText);

        if (transform2 == null)
        {
            var textObject = new GameObject(nameOfText, typeof(RectTransform));
            transform2 = textObject.transform;
            transform2.SetParent(transform, false);
            transform2.localPosition = Vector3.zero;
            transform2.localRotation = Quaternion.identity;
            transform2.localScale = Vector3.one;
        }

        var text = transform2.GetComponent<Text>();
        if (text == null)
        {
            text = transform2.gameObject.AddComponent<Text>();
            text.font = font ?? Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.fontSize = (int)textMeshProUGUI.fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.alignByGeometry = true;
        }

        var rectTransform = transform2.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = textMeshProUGUI.rectTransform.pivot;
        }

        text.horizontalOverflow = textMeshProUGUI.enableWordWrapping ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
        text.fontSize = (int)textMeshProUGUI.fontSize;

        text.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 1f);
        text.text = TMPToFontTextConverter.Convert(textMeshProUGUI.text, ConfigHandler.removeUnsupportedTags.Value, ConfigHandler.closeUnclosedTags.Value);
        textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 0f);
        text.enabled = true;
        text.supportRichText = textMeshProUGUI.richText;

        return text;
    }
}