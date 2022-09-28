using UnityEngine;
using UnityEditor;

/// <summary>
/// EditorTemplate description
/// </summary>
[CustomEditor(typeof(AiDirector))]
public class AiDirectorEditor : Editor
{
    public enum DisplayCategory
    {
        Statistics, Combat
    }

    public DisplayCategory categoryToDisplay;

    public override void OnInspectorGUI()
    {
        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        EditorGUILayout.Space();

        switch (categoryToDisplay)
        {
            case DisplayCategory.Statistics:
                DisplayStatisticInfo();
                break;
            case DisplayCategory.Combat:
                DisplayCombatInfo();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DisplayStatisticInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeToClearRoom"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("difficultyLevel"));
    }

    void DisplayCombatInfo()
    {
        EditorGUILayout.Space();
    }
}

