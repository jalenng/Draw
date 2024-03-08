using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(DialogueEntry))]
public class DialogueEntry_Property : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Calculate the positions and sizes of the rects
        float width = position.width - 5;
        Rect avatarNameRect = new Rect(position.x, position.y + 20, width, 18);
        Rect contentRect = new Rect(position.x, position.y + 40, width, 72);

        // Begin drawing the property in the Editor GUI
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty avatarNameProp = property.FindPropertyRelative("avatarName");
        SerializedProperty contentProp = property.FindPropertyRelative("content");

        // Populate the editor with the properties
        EditorGUI.PropertyField(avatarNameRect, avatarNameProp, new GUIContent("Avatar Name"));
        EditorGUI.PropertyField(contentRect, contentProp, new GUIContent("Content"));

        // End drawing the property in the Editor GUI
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Start with height at 0
        float height = 0;

        // Get properties
        SerializedProperty avatarNameProp = property.FindPropertyRelative("avatarName");
        SerializedProperty contentProp = property.FindPropertyRelative("content");

        // Add the height of each property
        height += EditorGUI.GetPropertyHeight(avatarNameProp);
        height += EditorGUI.GetPropertyHeight(contentProp);

        // Return the height
        return height;
    }
}
