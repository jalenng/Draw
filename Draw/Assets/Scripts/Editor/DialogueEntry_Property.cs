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
        Rect avatarRect = new Rect(position.x, position.y, width, 18);
        Rect contentRect = new Rect(position.x, position.y + 20, width, 72);
        Rect useCPSRect = new Rect(position.x, position.y + 94, width, 18);
        Rect cpsRect = new Rect(position.x, position.y + 114, width, 18);

        // Begin drawing the property in the Editor GUI
        EditorGUI.BeginProperty(position, label, property);

        // Get properties
        SerializedProperty avatarProp = property.FindPropertyRelative("avatar");
        SerializedProperty contentProp = property.FindPropertyRelative("content");
        SerializedProperty useCPSProp = property.FindPropertyRelative("useCPS");
        SerializedProperty cpsProp = property.FindPropertyRelative("CPS");

        // Populate the editor with the properties
        EditorGUI.PropertyField(avatarRect, avatarProp, new GUIContent("Avatar"));
        EditorGUI.PropertyField(contentRect, contentProp, new GUIContent("Content"));
        EditorGUI.PropertyField(useCPSRect, useCPSProp, new GUIContent("Use CPS"));

        // If the useCPS property is true, show the CPS property
        if (useCPSProp.boolValue)
            EditorGUI.PropertyField(cpsRect, cpsProp, new GUIContent("CPS"));
        
        // End drawing the property in the Editor GUI
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Start with height at 0
        float height = 0;

        // Get properties
        SerializedProperty avatarProp = property.FindPropertyRelative("avatar");
        SerializedProperty contentProp = property.FindPropertyRelative("content");
        SerializedProperty useCPSProp = property.FindPropertyRelative("useCPS");
        SerializedProperty cpsProp = property.FindPropertyRelative("CPS");

        // Add the height of each property
        height += EditorGUI.GetPropertyHeight(avatarProp);
        height += EditorGUI.GetPropertyHeight(contentProp);
        height += EditorGUI.GetPropertyHeight(useCPSProp);

        // If the useCPS property is true, add the height of the CPS property
        if (property.FindPropertyRelative("useCPS").boolValue)
            height += EditorGUI.GetPropertyHeight(cpsProp);
        
        // Return the height
        return height;
    }
}
