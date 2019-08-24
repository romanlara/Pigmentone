// -----------------------------------------------------
//                      Pigmentone
//                          by 
//                 Sliverbroom Studios
//       Copyright 2015 Roman Lara & Humberto Lara
// -----------------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(Puzzle.EmitterSetting))]

public class EmitterSettingDrawer : PropertyDrawer 
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);
		{
			Rect contentPosition = EditorGUI.PrefixLabel(position, label);
			contentPosition.width /= 3f;
			EditorGUI.indentLevel = 0;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("locate"), GUIContent.none);
			contentPosition.x += contentPosition.width;
			EditorGUIUtility.labelWidth = 14f;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("colorToEmit"), new GUIContent("C"));
			contentPosition.x += contentPosition.width;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("offset"), new GUIContent("O"));
		} EditorGUI.EndProperty();
	}
}
