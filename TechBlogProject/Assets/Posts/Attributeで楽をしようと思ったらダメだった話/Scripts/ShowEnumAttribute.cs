using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace TechBlogPosts.EditorOrAttribute
{
    public class ShowEnumAttribute : PropertyAttribute
    {
        public string EnumName;
        public int Value;


        public ShowEnumAttribute(string enumName, int value)
        {
            EnumName = enumName;
            Value = value;
        }
    }
}
