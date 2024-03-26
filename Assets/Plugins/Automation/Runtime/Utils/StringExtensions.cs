using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Automation.Runtime.Utils
{
    public static class StringExtensions
    {
        public static string FromArrayToString(this IEnumerable<string> strings, char insertAfter = default, bool insertLast = true)
        {
            var stringBuilder = new StringBuilder();
            var strArray =  strings.ToArray();
            
            if (!strArray.Any())
                return string.Empty;
            
            for (var i = 0; i < strArray.Count(); i++)
            {
                stringBuilder.Append(strArray[i]);
                
                if (insertAfter != default && i != strArray.Length - 1 || (i == strArray.Length - 1 && insertLast))
                    stringBuilder.Append(insertAfter);
            }

            return stringBuilder.ToString();
        }
        public static string PathToCommandLine(this string path)
        {
            var split = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            path = string.Empty;
            
            for (var i = 0; i < split.Length; i++)
            {
                if (split[i].Contains(' '))
                    split[i] = $"\"{split[i]}\"";

                path += split[i] + Path.AltDirectorySeparatorChar;
            }
            
            return path;
        }

        public static string SetColor(this string text, string color, bool bold) => 
            "<color=" + color + ">" + (bold ? "<b>" : "") + text + (bold ? "</b>" : "") + "</color>";
    }
}