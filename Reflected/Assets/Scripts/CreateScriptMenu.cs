using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Helps us create scripts through the menu
/// </summary>
public static class CreateScriptMenu
{
    static string templateFilePath = "C:/Users/valte/Desktop/Unity/Templates";
    const string menuName = "Assets/Script Shortcuts";

    #region Templates

    [MenuItem(menuName + "/Create New Template")]
    static void CreateTemplate()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Template", templateFilePath, "NewTemplate.txt", "txt");
        string pathToTemplate = templateFilePath + "/BaseTemplate.txt";
        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }

    #endregion

    #region Basic Scripts
    [MenuItem(menuName + "/Create Basic Script/Create MonoBehaviour Script")]
    static void CreateMonoBehaviourItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Mono Behaviour", GetCurrentPath(), "NewMonoBehaviour.cs", "cs");
        string pathToTemplate = templateFilePath + "/MonoBehaviourTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }

    [MenuItem(menuName + "/ Create Basic Script/Create Custom Editor Script")]
    static void CreateEditorMenuItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create New Editor", GetCurrentPath(), "NewEditor.cs", "cs");
        string pathToTemplate = templateFilePath + "/EditorTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }

    #endregion

    #region Standard Scripts

    [MenuItem(menuName + "/Create Standard Script/Create Player")]
    static void CreatePlayerScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create New Player", GetCurrentPath(), "Player.cs", "cs");
        string pathToTemplate = templateFilePath + "/PlayerTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }

    #endregion

    #region Polymorphic Scripts

    [MenuItem(menuName + "/Create Polymorphic Script/Entities/Create All")]
    static void CreateAllEntities()
    {
        CreateEntityScript();
        CreateEntityPlayerScript();
        CreateEntityEnemyScript();
    }
    [MenuItem(menuName + "/Create Polymorphic Script/Entities/Create Entity")]
    static void CreateEntityScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create New Entity", GetCurrentPath(), "Entity.cs", "cs");
        string pathToTemplate = templateFilePath + "/EntityTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }
    [MenuItem(menuName + "/Create Polymorphic Script/Entities/Create Player")]
    static void CreateEntityPlayerScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create New Player", GetCurrentPath(), "Player.cs", "cs");
        string pathToTemplate = templateFilePath + "/EntityPlayerTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }

    [MenuItem(menuName + "/Create Polymorphic Script/Entities/Create Enemy")]
    static void CreateEntityEnemyScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create New Enemy", GetCurrentPath(), "Enemy.cs", "cs");
        string pathToTemplate = templateFilePath + "/EntityEnemyTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }
    #endregion

    #region Utility Scripts

    [MenuItem(menuName + "/Create Utility Script/UI/Create LookAtCamera")]
    static void CreateLookAtCameraScript()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create New LookAtCamera", GetCurrentPath(), "LookAtCamera.cs", "cs");
        string pathToTemplate = templateFilePath + "/LookAtCameraTemplate.txt";

        WriteFileFromTemplate(pathToNewFile, pathToTemplate);
    }

    #endregion

    static void WriteFileFromTemplate(string pathToNewFile, string pathToTemplate)
    {
        if (string.IsNullOrEmpty(pathToNewFile))
            return;

        FileInfo fileInfo = new FileInfo(pathToNewFile);
        string nameOfScript = Path.GetFileNameWithoutExtension(fileInfo.Name);

        string text = File.ReadAllText(pathToTemplate);

        text = text.Replace("#SCRIPTNAME#", nameOfScript);
        text = text.Replace("#YEAR#", DateTime.Now.Year.ToString());

        text = text.Replace("#SCRIPTNAMEWITHOUTEDITOR#", nameOfScript.Replace("Editor", ""));

        File.WriteAllText(pathToNewFile, text);
        AssetDatabase.Refresh();
    }

    static string GetCurrentPath()
    {
        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

        if(path.Contains("."))
        {
            int index = path.LastIndexOf("/");
            path = path.Substring(0, index);
        }

        return path;
    }

}


