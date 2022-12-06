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
        Basic, Combat, Abilities, Dimension
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
            case DisplayCategory.Abilities:
                DisplayAbilityInfo();
                break;
            case DisplayCategory.Dimension:
                DisplayDimensionInfo();
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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("anim"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hitbox"));
    }

    void DisplayAbilityInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("basicSwordAbility"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("specialAbility"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("swapAbilities"));
    }
    void DisplayCombatInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSpeed"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentWeapon"));
    }

    void DisplayDimensionInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("chargeBar"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("swapAbilities"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TimeFlowWhileSwapping"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("trueSwapSound"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mirrorSwapSound"));
    }
}