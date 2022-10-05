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
        Statistics, Player, ActiveRoom, Map
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
            case DisplayCategory.Player:
                DisplayPlayerInfo();
                break;
            case DisplayCategory.ActiveRoom:
                DisplayActiveRoomInfo();
                break;
            case DisplayCategory.Map:
                DisplayMapInfo();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DisplayStatisticInfo()
    {
        EditorGUILayout.TextField("DifficultyLevel");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("difficultyLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawntime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("amountOfEnemiesToSpawn"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("avergaeTimeToClearRoom"));
        EditorGUILayout.Space();
        EditorGUILayout.TextField("Room Statistics");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemiesInRoom"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeToClearRoom"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("activeRoom"));
        EditorGUILayout.Space();
        EditorGUILayout.TextField("Player Statistics");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerCurrentHelathPercentage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("temporaryCurrency"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfEnemiesKilled"));
        EditorGUILayout.Space();
        EditorGUILayout.TextField("Map Statistics");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfRoomsCleared"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfRoomsLeftOnMap"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NumberOfRoomsSinceShop"));
    }

    void DisplayPlayerInfo()
    {
        EditorGUILayout.TextField("Player Statistics");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerCurrentHelathPercentage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("temporaryCurrency"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfEnemiesKilled"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("chest"));
    }

    void DisplayActiveRoomInfo()
    {
        EditorGUILayout.TextField("Room Statistics");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemiesInRoom"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("timeToClearRoom"));
    }

    void DisplayMapInfo()
    {
        EditorGUILayout.TextField("Map Statistics");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfRoomsCleared"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("numberOfRoomsLeftOnMap"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NumberOfRoomsSinceShop"));
    }
}

