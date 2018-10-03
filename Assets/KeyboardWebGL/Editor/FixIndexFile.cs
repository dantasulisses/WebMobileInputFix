using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace WebGLKeyboard
{
    /// <summary>
    /// Edits the index.html file (the Unity default one) after the build to add the necessary elements to the fix work
    /// </summary>
    public class FixIndexFile : MonoBehaviour
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.WebGL)
            {
                string text = File.ReadAllText(Path.Combine(pathToBuiltProject, "index.html"));
                string original = "<div class=\"webgl-content\">";
                string replace = "<script>\r\n\t\tfunction FixInputOnSubmit() {\r\n\t\t\tdocument.getElementById(\"fixInput\").blur();\r\n\t\t\tevent.preventDefault();\r\n\t\t}\r\n\t</script>\r\n    <div>\r\n\t\t<form onsubmit=\"FixInputOnSubmit()\" autocomplete=\"off\" style=\"width: 0px; height: 0px; position: absolute; top: -9999px;\">\r\n\t\t\t<input type=\"text\" id=\"fixInput\" oninput=\"gameInstance.Module.ccall('FixInputUpdate', null)\" onblur=\"gameInstance.Module.ccall('FixInputOnBlur', null)\" style=\"font-size: 42px;\">\r\n\t\t</form>\r\n\t</div>\r\n\t<div class=\"webgl-content\">";
                
                text = text.Replace(original, replace);
                File.WriteAllText(Path.Combine(pathToBuiltProject, "index.html"), text);
            }
        }
    }
}