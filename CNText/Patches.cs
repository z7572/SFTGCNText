using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CNText.Helper;

namespace CNText;


[HarmonyPatch]
public class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(OnlinePlayerUI), "Update")]
    private static void OnlinePlayerUIUpdatePostfix(OnlinePlayerUI __instance)
    {
        var texts = Traverse.Create(__instance).Field("mPlayerTexts").GetValue<TextMeshProUGUI[]>();
        foreach (var textMeshProUGUI in texts)
        {
            textMeshProUGUI.enabled = true;
            var hasUnsupportedChars = textMeshProUGUI.GetParsedText().Contains("□") && !textMeshProUGUI.text.Contains("□");

            var rawText = textMeshProUGUI.text;
            var hasRichTextTags = textMeshProUGUI.GetParsedText() != rawText;
            var hasEmoji = rawText.Any(char.IsSurrogate);

            if (hasEmoji && !hasRichTextTags)
            {
                hasUnsupportedChars = false;
            }

            if (hasUnsupportedChars)
            {
                var text = TMP2FontText(textMeshProUGUI, CNText.Impact_MiSans_ttf, "CustomText");
                text.fontSize = ConfigHandler.customTextFontSize.Value;
            }
            else
            {
                var customTextTransform = textMeshProUGUI.transform.Find("CustomText");
                if (customTextTransform != null)
                {
                    var text = customTextTransform.GetComponent<Text>();
                    if (text != null)
                    {
                        text.enabled = false;
                    }
                }
                textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, 1f);
            }
        }
    }
}

