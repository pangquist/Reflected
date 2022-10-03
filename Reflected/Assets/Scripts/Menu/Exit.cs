using UnityEngine;
using UnityEditor;

public static class Exit 
{
    /// <summary>
    /// Exits this application.
    /// </summary>
    public static void ExitApplication()
    {
        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #else

        Application.Quit();

        #endif
    }
}
