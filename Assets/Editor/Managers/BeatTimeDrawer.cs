// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(Puzzle.BeatTime))]

public class BeatTimeDrawer : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		{
			Rect contentPosition = EditorGUI.PrefixLabel(position, label);
			contentPosition.width /= 3f;
			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = 14f;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("hours"), new GUIContent("H"));
			contentPosition.x += contentPosition.width;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("minutes"), new GUIContent("M"));
			contentPosition.x += contentPosition.width;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("seconds"), new GUIContent("S"));
		} EditorGUI.EndProperty();
	}
}
