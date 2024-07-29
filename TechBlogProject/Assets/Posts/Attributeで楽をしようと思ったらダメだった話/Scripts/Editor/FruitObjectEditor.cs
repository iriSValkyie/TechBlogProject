using System;
using System.Collections.Generic;
using UnityEditor;

namespace TechBlogPosts.EditorOrAttribute
{
    [CustomEditor(typeof(FruitObject))]
    public class FruitObjectEditor: Editor
    {
        private SerializedProperty m_Type;
        private SerializedProperty m_AppleNum;
        private SerializedProperty m_GrapeNum;
        public void OnEnable()
        {
            
            m_Type = serializedObject.FindProperty("m_Type");
            m_AppleNum = serializedObject.FindProperty("m_AppleNum");
            m_GrapeNum = serializedObject.FindProperty("m_GrapeNum");
        }

        public override void OnInspectorGUI()
        {

            EditorGUILayout.PropertyField(m_Type);

            if (m_Type.intValue == (int)Fruit.APPLE)
            {
                EditorGUILayout.PropertyField(m_AppleNum);
            }
            
            if (m_Type.intValue == (int)Fruit.GRAPE)
            {
                EditorGUILayout.PropertyField(m_GrapeNum);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}