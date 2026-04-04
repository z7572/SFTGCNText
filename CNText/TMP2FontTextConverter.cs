using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// Adapted from Moncak's stick.plugins.cntext
// Add removal of sup, sub, sprite, size, material, quad and space tags
// Add support of br tags
public static class TMPToFontTextConverter
{
    public static string Convert(string input, bool removeUnsupportedTags = true, bool closeUnclosedTags = true)
    {
        string text = PreprocessColorTags(input);
        text = ProcessColorTags(text);
        text = Regex.Replace(text, @"<(br|BR)\s*/?\s*>", "\n");
        if (closeUnclosedTags)
            text = CloseUnclosedTags(text);
        text = RemoveUnsupportedTags(text, removeUnsupportedTags);
        return text;
    }

    private static string PreprocessColorTags(string input)
    {
        return Regex.Replace(input, "<(#?[0-9a-fA-F]{3,8})>", delegate (Match match)
        {
            string value = match.Groups[1].Value;
            return "<color=" + (value.StartsWith("#") ? value : ("#" + value)) + ">";
        });
    }

    private static string ProcessColorTags(string input)
    {
        return Regex.Replace(input, "<color=(.*?)>", delegate (Match match)
        {
            string text = match.Groups[1].Value.Trim();
            string str;
            if (ColorNameToHex.TryGetValue(text.ToLower(), out str))
            {
                return "<color=" + str + ">";
            }
            if (text.StartsWith("#"))
            {
                return FormatHexColor(text.Replace("#", ""));
            }
            if (text.StartsWith("rgba"))
            {
                return ConvertRGBA(text);
            }
            return "<color=" + text + ">";
        }, RegexOptions.IgnoreCase);
    }

    private static string FormatHexColor(string hex)
    {
        hex = hex.ToUpper();
        int length = hex.Length;
        if (length == 3)
        {
            return string.Concat(new string[]
            {
                "<color=#",
                new string(hex[0], 2),
                new string(hex[1], 2),
                new string(hex[2], 2),
                ">"
            });
        }
        if (length == 6)
        {
            return "<color=#" + hex + "FF>";
        }
        if (length != 8)
        {
            return "<color=#" + hex + ">";
        }
        return "<color=#" + hex + ">";
    }

    private static string RemoveUnsupportedTags(string input, bool removeUnsupported)
    {
        if (!removeUnsupported)
        {
            return input;
        }

        foreach (string tag in new string[]
        {
            "font", "s", "u", "align", "sup", "sub",
            "sprite", "size", "material", "quad", "space"
        })
        {
            input = Regex.Replace(input, $"<{tag}[^>]*>", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, $"</{tag}>", "", RegexOptions.IgnoreCase);
        }
        return input;
    }

    private static string CloseUnclosedTags(string input)
    {
        Stack<string> stack = new Stack<string>();
        StringBuilder stringBuilder = new StringBuilder();
        int i = 0;

        while (i < input.Length)
        {
            if (input[i] == '<')
            {
                int endIndex = input.IndexOf('>', i);
                if (endIndex == -1)
                {
                    break;
                }

                string tag = input.Substring(i, endIndex - i + 1);
                bool isClosingTag = tag.StartsWith("</");
                string tagName = Regex.Match(tag, "</?(\\w+)").Groups[1].Value.ToLower();

                stringBuilder.Append(tag);

                if (!isClosingTag && !tagName.EndsWith("/"))
                {
                    stack.Push(tagName);
                }
                else if (isClosingTag && stack.Count > 0 && stack.Peek() == tagName)
                {
                    stack.Pop();
                }

                i = endIndex + 1;
            }
            else
            {
                stringBuilder.Append(input[i++]);
            }
        }

        while (stack.Count > 0)
        {
            string tagName = stack.Pop();
            stringBuilder.Append("</" + tagName + ">");
        }

        return stringBuilder.ToString();
    }

    private static string ConvertRGBA(string rgba)
    {
        MatchCollection matchCollection = Regex.Matches(rgba, "\\d+\\.?\\d*");
        if (matchCollection.Count < 3)
        {
            return "#FFFFFFFF";
        }
        StringBuilder stringBuilder = new StringBuilder("#");
        for (int i = 0; i < 3; i++)
        {
            stringBuilder.Append((int.Parse(matchCollection[i].Value) * 255).ToString("X2"));
        }
        stringBuilder.Append((matchCollection.Count > 3) ? ((int)(float.Parse(matchCollection[3].Value) * 255f)).ToString("X2") : "FF");
        return string.Format("<color={0}>", stringBuilder);
    }

    private static readonly Dictionary<string, string> ColorNameToHex = new Dictionary<string, string>
    {
        { "white", "#FFFFFF" },
        { "black", "#000000" },
        { "red", "#FF0000" },
        { "green", "#00FF00" },
        { "blue", "#0000FF" },
        { "yellow", "#FFFF00" },
        { "cyan", "#00FFFF" },
        { "magenta", "#FF00FF" },
        { "gray", "#808080" },
        { "grey", "#808080" }
    };
}
