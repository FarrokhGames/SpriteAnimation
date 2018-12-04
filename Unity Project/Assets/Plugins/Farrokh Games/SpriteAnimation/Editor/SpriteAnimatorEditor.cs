using System;
using System.Collections.Generic;
using System.Linq;
using FarrokhGames.SpriteAnimation.Frame;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [CustomEditor(typeof(SpriteAnimator))]
    public class SpriteAnimatorEditor : Editor
    {
        SerializedProperty _sprites;
        SerializedProperty _clips;
        ReorderableList _clipList;

        float Height { get { return EditorGUIUtility.singleLineHeight * 1.1f; } }

        public void OnEnable()
        {
            _sprites = serializedObject.FindProperty("_sprites");
            _clips = serializedObject.FindProperty("_clips");
            _clipList = new ReorderableList(serializedObject, _clips);
            _clipList.drawHeaderCallback += DrawClipHeader;
            _clipList.drawElementCallback += DrawClipElement;
            _clipList.elementHeight = 12 + (Height * 4f);

        }

        void DrawClipHeader(Rect rect)
        {
            GUI.Label(rect, "Clips");
        }

        void DrawClipElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var item = _clips.GetArrayElementAtIndex(index); // as IClip;

            EditorGUI.BeginChangeCheck();

            rect.y += 4;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, Height), item.FindPropertyRelative("_name"), new GUIContent("Name"));
            rect.y += Height;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, Height), item.FindPropertyRelative("_loop"), new GUIContent("Loop"));
            rect.y += Height;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, Height), item.FindPropertyRelative("_randomStart"), new GUIContent("Random Start"));
            rect.y += Height;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, Height), item.FindPropertyRelative("_frameRate"), new GUIContent("Frame Rate"));
            rect.y += Height;

            // End line
            rect.y += 4;
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), Color.gray);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            return;
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_sprites, true);
            _clipList.DoLayoutList();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}