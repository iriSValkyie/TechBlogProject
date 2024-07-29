using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TechBlogPosts.EditorOrAttribute
{
    [CustomPropertyDrawer(typeof(ShowEnumAttribute))]
    public class ShowEnumAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ShowEnumAttribute;
            var enumProperty = property.serializedObject.FindProperty(attr.EnumName);
            if (enumProperty == null || attr.Value != enumProperty.intValue) return;

            var height = GetPropertyHeight(property,label);
            position.height = height;
            
            EditorGUI.PropertyField(position, property);

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}