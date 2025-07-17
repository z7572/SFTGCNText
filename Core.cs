using BepInEx;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HarmonyLib;
using LevelEditor;

namespace CNText
{
    [BepInPlugin("z7572.cntext", "CNText", "1.0")]
    public class MapEditorCNText : BaseUnityPlugin
    {
        public void Awake()
        {
            var font = Resources.Load<TMP_FontAsset>("fonts & materials/roboto-bold sdf");
            var font2 = Resources.Load<TMP_FontAsset>("fonts & materials/roboto-bold sdf ii");
            if (font2 == null)
            {
                Logger.LogError("汉化包未安装！");
                return;
            }
            if (font.fallbackFontAssets[0] == null || font.fallbackFontAssets[0] != font2)
            {
                font.fallbackFontAssets[0] = font2;
            }
            Logger.LogInfo("本mod为地图编辑器的汉化和汉化包字体的补全(part2)，以及代码中文本的汉化");
            Logger.LogInfo("GitHub: https://github.com/z7572/SFTGCNText/releases/");
            try
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        [HarmonyPatch]
        public class Patches
        {
            [HarmonyPostfix]
            [HarmonyPatch(typeof(LevelCreator), "Start")]
            private static void LevelCreatorStartPostfix()
            {
                var UI = GameObject.Find("Canvas").transform.Find("UI");

                UI.Find("SLU-Grid").GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "自动保存 : 关";
                UI.Find("SLU-Grid").GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "保存";
                UI.Find("SLU-Grid").GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "加载";
                UI.Find("SLU-Grid").GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "上传";
                UI.Find("NameInput").GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "地图名称...";
                UI.Find("QuestionBox").GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "确定吗？";
                UI.Find("QuestionBox").GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "是";
                UI.Find("QuestionBox").GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "否";
                UI.Find("QuestionBox").GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "取消";
                UI.Find("MirrorGrid").GetChild(0).GetComponent<TextMeshProUGUI>().text = "镜像放置";
                UI.Find("MirrorGrid").GetChild(1).GetComponent<TextMeshProUGUI>().text = "镜像旋转";
                UI.Find("SnapButton").GetComponent<TextMeshProUGUI>().text = "吸附";
                UI.Find("ClearButtton").GetChild(0).GetComponent<TextMeshProUGUI>().text = "清除";
                UI.Find("Info Panel").GetChild(0).GetComponent<TextMeshProUGUI>().text = "编辑器信息";
                UI.Find("Info Panel").GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "关闭";
                UI.Find("Info Panel").GetChild(1).GetComponent<TextMeshProUGUI>().text =
                "按键绑定\n" +
                "旋转 - R\n" +
                "翻转 - F\n" +
                "切换镜像放置 - M\n" +
                "切换镜像旋转 - Shift + M\n" +
                "切换吸附 - S\n" +
                "取消选择物体 - ESC\n" +
                "选取所指物体 - I\n" +
                "显示网格 - X\n" +
                "切换网格 - LCTRL + X\n" +
                "切换物体可拖动 - LCTRL + D\n" +
                "调节关卡大小 - 鼠标滚轮\n";
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSaveUI), "OnAutosaveClicked")]
            private static IEnumerable<CodeInstruction> OnAutosaveClickedTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "AUTOSAVE : ", "自动保存 : ");
                    ReplaceLdstr(codes[i], "OFF", "关");
                    ReplaceLdstr(codes[i], "MIN", "分钟");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSaveUI), "OnClearClicked")]
            private static IEnumerable<CodeInstruction> OnClearClickedTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Do You Want To Clear Your Current Level?", "你确定要清除当前关卡吗？");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSaveUI), "OnSavedClicked", new Type[] { typeof(bool) })]
            private static IEnumerable<CodeInstruction> OnSavedClickedTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Save Sucessful!", "保存成功！");
                    ReplaceLdstr(codes[i], "Do You Want Do Overwrite Level With Name: ", "你确定要覆盖名为: ");
                    ReplaceLdstr(codes[i], " ? ", " 的关卡吗？");
                    ReplaceLdstr(codes[i], "Save Error: ", "保存错误: ");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSaveUI), "PopulateGrid")]
            private static IEnumerable<CodeInstruction> PopulateGridTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Loading This Map Will Overwrite Any Unsaved Progress, Continue?", "加载此地图将覆盖任何未保存的进度， 继续吗？");
                    ReplaceLdstr(codes[i], "Do You Want To Delete Map: ", "你确定要删除地图: ");
                    ReplaceLdstr(codes[i], "Do You Want To Unsubscribe From Map: ", "你确定要取消订阅地图: ");
                    ReplaceLdstr(codes[i], " ?", " 吗？");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSaveUI), "OnUploadClicked")]
            private static IEnumerable<CodeInstruction> OnUploadClickedTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Update Existing or Upload New Map: ", "更新现有或上传新地图: ");
                    ReplaceLdstr(codes[i], " ?", " ？");
                    ReplaceLdstr(codes[i], "PublishNew", "上传新地图");
                    ReplaceLdstr(codes[i], "Update Existing", "更新现有地图");
                    ReplaceLdstr(codes[i], "Cancel", "取消");
                    ReplaceLdstr(codes[i], "Upload Error: ", "上传错误: ");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSaveUI), "GetErrorMessage")]
            private static IEnumerable<CodeInstruction> GetErrorMessageTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Name Is Empty!", "名称为空！");
                    ReplaceLdstr(codes[i], "Name Contains Invalid Characters!", "名称包含无效字符！");
                    ReplaceLdstr(codes[i], "Steam Is Not Initialized!", "Steam 未初始化！");
                    ReplaceLdstr(codes[i], "Error: ", "错误: ");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorLoadSave), "SaveAndPublish")]
            private static IEnumerable<CodeInstruction> SaveAndPublishTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Uploading...", "上传中...");
                    ReplaceLdstr(codes[i], "Updating...", "更新中...");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(DialougePanelUI), "GiveChoice", new Type[] { typeof(string), typeof(Action), typeof(Action) })]
            private static IEnumerable<CodeInstruction> GiveChoiceTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Yes", "是");
                    ReplaceLdstr(codes[i], "No", "否");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(DialougePanelUI), "Prompt")]
            private static IEnumerable<CodeInstruction> PromptTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Ok", "确定");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(EditorSystemUI), "OnQuitClicked")]
            private static IEnumerable<CodeInstruction> OnQuitClickedTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Do You Want To Quit The Editor? Unsaved progress will be lost", "你确定要退出编辑器吗？ 未保存的进度将会丢失");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(WorkshopItemCreator), "UpdateItem")]
            private static IEnumerable<CodeInstruction> UpdateItemTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Upload Successful! Do you want to go to the steampage?", "上传成功！ 你想去 STEAM 页面吗？");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(WorkshopItemCreator), "OnSubmitItemUpdateResult")]
            private static IEnumerable<CodeInstruction> OnSubmitItemUpdateResultTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "Upload Error: ", "上传错误: ");
                }
                return codes;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(ServerLobbyTextUI), "OnEnable")]
            private static IEnumerable<CodeInstruction> OnEnableTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    ReplaceLdstr(codes[i], "WAITING FOR PLAYERS", "等待玩家加入");
                    ReplaceLdstr(codes[i], "INVITE YOUR FRIENDS THROUGH STEAM", "通过 STEAM 邀请你的好友");
                }
                return codes;
            }
        }

        private static void ReplaceLdstr(CodeInstruction code, string target, string replaceWith)
        {
            if (code.opcode == OpCodes.Ldstr && code.operand.ToString() == target)
            {
                code.operand = replaceWith;
                //Debug.Log($"Replaced {target} with {replaceWith}");
            }
        }

        // Wrong position
        [Obsolete]
        private static Text TMP2Text(TextMeshProUGUI textMeshProUGUI, Font font = null, string nameOfText = "Text")
        {
            var transform = textMeshProUGUI.transform;
            var transform2 = transform.Find(nameOfText);

            if (transform2 == null)
            {
                var textObject = new GameObject(nameOfText);
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

            text.color = textMeshProUGUI.color;
            text.text = textMeshProUGUI.text;
            textMeshProUGUI.enabled = false;
            text.enabled = true;
            text.supportRichText = textMeshProUGUI.richText;

            return text;
        }
    }
}
