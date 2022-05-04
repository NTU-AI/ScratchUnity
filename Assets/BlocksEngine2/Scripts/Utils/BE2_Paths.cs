using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// v2.3 - new helper class to support the setting of needed paths
public class BE2_Paths
{
    static public string TranslateMarkupPath(string pathMarkup)
    {
        string fullPath = pathMarkup;

        fullPath = fullPath.Replace("[dataPath]", Application.dataPath);
        fullPath = fullPath.Replace("[persistentDataPath]", Application.persistentDataPath);

        return fullPath;
    }

    static public string PathToResources(string pathMarkup)
    {
        string resourcesPath = pathMarkup;

        if (!pathMarkup.ToLower().Contains("resources"))
        {
            Debug.LogError("The path is not set to a Resources folder.");

            return resourcesPath;
        }

        int idx = resourcesPath.ToLower().IndexOf("resources/");
        resourcesPath = resourcesPath.Substring(idx + 10);

        return resourcesPath;
    }

    static public string NewInstructionPath
    {
        get => BE2_Inspector.newInstructionPath;
        set => BE2_Inspector.newInstructionPath = value;
    }

    static public string NewBlockPrefabPath
    {
        get => BE2_Inspector.newBlockPrefabPath;
        set => BE2_Inspector.newBlockPrefabPath = value;
    }

    static public string SavedCodesPath
    {
        get => BE2_UI_ContextMenuManager.savedCodesPath;
        set => BE2_UI_ContextMenuManager.savedCodesPath = value;
    }
}
