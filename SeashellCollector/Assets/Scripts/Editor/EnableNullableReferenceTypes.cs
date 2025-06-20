using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEngine;

[InitializeOnLoad]
public static class EnableNullableReferenceTypes
{
    static EnableNullableReferenceTypes()
    {
        EditorApplication.delayCall += PatchCsprojFiles;
    }

    [DidReloadScripts]
    private static void OnScriptsReloaded() => PatchCsprojFiles();

    private static void PatchCsprojFiles()
    {
        string[] csprojFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj");
        foreach (var file in csprojFiles)
        {
            string contents = File.ReadAllText(file);
            if (contents.Contains("<Nullable>enable</Nullable>")) continue;

            if (contents.Contains("<LangVersion>latest</LangVersion>"))
            {
                contents = contents.Replace(
                    "<LangVersion>latest</LangVersion>",
                    "<LangVersion>latest</LangVersion>\n    <Nullable>enable</Nullable>");
                File.WriteAllText(file, contents);
                Debug.Log($"Patched nullable into {Path.GetFileName(file)}");
            }
        }
    }
}
