using UnityEditor;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Frame
{
    [CustomPropertyDrawer(typeof(Frame))]
    class IngredientDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.LabelField(position, ""); //label.text.Replace("Element", ""));

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect indexLabelRect = new Rect(position.x, position.y, 40, position.height);
            Rect indexRect = new Rect(position.x + 40, position.y, 30, position.height);

            Rect speedLabelRect = new Rect(position.x + 70, position.y, 40, position.height);
            Rect speedRect = new Rect(position.x + 110, position.y, 30, position.height);

            Rect triggerLabelRect = new Rect(position.x + 140, position.y, 45, position.height);
            Rect triggerRect = new Rect(position.x + 185, position.y, position.width - 185, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PrefixLabel(indexLabelRect, new GUIContent("index"));
            EditorGUI.PropertyField(indexRect, property.FindPropertyRelative("_index"), GUIContent.none);
            EditorGUI.PrefixLabel(speedLabelRect, new GUIContent("speed"));
            EditorGUI.PropertyField(speedRect, property.FindPropertyRelative("_speed"), GUIContent.none);
            EditorGUI.PrefixLabel(triggerLabelRect, new GUIContent("trigger"));
            EditorGUI.PropertyField(triggerRect, property.FindPropertyRelative("_triggerName"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}