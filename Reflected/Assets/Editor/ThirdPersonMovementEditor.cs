//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using UnityEngine;
using UnityEditor;

/// <summary>
/// EditorTemplate description
/// </summary>
[CustomEditor(typeof(ThirdPersonMovement))]
public class MovementEditorTemplate : Editor
{
	public enum DisplayCategory
	{
		Basic, Movement, Jumping, Dashing
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
			case DisplayCategory.Movement:
				DisplayMovementInfo();
				break;
			case DisplayCategory.Jumping:
				DisplayJumpingInfo();
				break;
			case DisplayCategory.Dashing:
				DisplayDashingInfo();
				break;
		}

		serializedObject.ApplyModifiedProperties();
	}

	void DisplayBasicInfo()
    {
		EditorGUILayout.PropertyField(serializedObject.FindProperty("stats"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("controller"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("cam"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));
	}

	void DisplayMovementInfo()
	{
		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("turnSmoothTime"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("footstepSounds"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("footstepEffect"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("feetPositions"));
	}

	void DisplayJumpingInfo()
	{
		EditorGUILayout.PropertyField(serializedObject.FindProperty("gravityEffect"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpHeight"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("groundDistance"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("groundMask"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("effectMask"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("isGrounded"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("canSpawnEffect"));
	}

	void DisplayDashingInfo()
    {
		EditorGUILayout.PropertyField(serializedObject.FindProperty("dashAbility"));
	}
}