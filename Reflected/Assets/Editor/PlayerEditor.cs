//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using UnityEngine;
using UnityEditor;

/// <summary>
/// EditorTemplate description
/// </summary>
[CustomEditor(typeof(Player))]
public class PlayerEditorTemplate : Editor
{
    public enum DisplayCategory
    {
        Basic, Combat
    }

    public DisplayCategory categoryToDisplay;

    public override void OnInspectorGUI()
    {
        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        EditorGUILayout.Space();

        switch (categoryToDisplay)
        {
            case DisplayCategory.Basic:
                DisplayBasicInfo();
                break;
            case DisplayCategory.Combat:
                DisplayCombatInfo();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DisplayBasicInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("stats"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHealth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("movementSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpForce"));
    }

    void DisplayCombatInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSpeed"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weapons"));
    }
}