using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SG
{
    public class CustomDrawer : PropertyDrawer
    {

    }

    [CustomPropertyDrawer(typeof(Range2Attribute))]
    internal sealed class RangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position,
                          SerializedProperty property, GUIContent label)
        {
            Range2Attribute range2 = (Range2Attribute)attribute;

            if (property.propertyType == SerializedPropertyType.Integer)
                EditorGUI.IntSlider(position, property, range2.min, range2.max, label);
            else
                EditorGUI.PropertyField(position, property, label);
        }
    }


    [System.AttributeUsage(System.AttributeTargets.Field,
                                   Inherited = true, AllowMultiple = false)]
    public class Range2Attribute : PropertyAttribute
    {
        public readonly int min;
        public readonly int max;

        public Range2Attribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }


    public class DisableAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(DisableAttribute))]
    internal sealed class DisableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position,
                          SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndDisabledGroup();
        }
    }
}
