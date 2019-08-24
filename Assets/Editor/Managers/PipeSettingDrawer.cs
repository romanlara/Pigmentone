// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(Puzzle.PipeSetting))]

public class PipeSettingDrawer : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		{
			Rect contentPosition = EditorGUI.PrefixLabel(position, label);
			contentPosition.width *= 0.5f;
			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = 14f;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("normal"), new GUIContent("N"));
			contentPosition.x += contentPosition.width;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("mirror"), new GUIContent("M"));
		} EditorGUI.EndProperty();
	}
}
