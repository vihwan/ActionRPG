using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SG
{

    public class QuestObjectiveTypeAttribute : PropertyAttribute
    {

    }

    //[CustomPropertyDrawer(typeof(QuestObjectiveTypeAttribute))]
    public class QuestObjectiveTypeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            QuestObjectiveTypeAttribute flagSettings = (QuestObjectiveTypeAttribute)attribute;


            SerializedProperty kind = property.FindPropertyRelative("type");
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.EndProperty();
        }
    }



    //[CustomPropertyDrawer(typeof(QuestObjective))]
    public class QuestObjectivePropertyDrawer : PropertyDrawer
    {
        Object target = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty index = property.FindPropertyRelative("index");
            SerializedProperty title = property.FindPropertyRelative("title");
            SerializedProperty kind = property.FindPropertyRelative("type");
            SerializedProperty state = property.FindPropertyRelative("state");
            SerializedProperty progress = property.FindPropertyRelative("progress");
            SerializedProperty currentProgressCount = property.FindPropertyRelative("currentProgressCount");
            SerializedProperty maxProgressCount = property.FindPropertyRelative("maxProgressCount");

            //Rect(x좌표,y좌표,가로길이,세로길이)
            var indexRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var titleRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);
            var kindRect = new Rect(position.x, position.y + 40f, position.width, EditorGUIUtility.singleLineHeight);
            var targetRect = new Rect(position.x, position.y + 60f, position.width, EditorGUIUtility.singleLineHeight);
            var stateRect = new Rect(position.x, position.y + 80f, position.width, EditorGUIUtility.singleLineHeight);
            var progressRect = new Rect(position.x, position.y + 120f, position.width, EditorGUIUtility.singleLineHeight);
            var currentProgressCountRect = new Rect(position.x, position.y + 140f, position.width, EditorGUIUtility.singleLineHeight);
            var maxProgressCountRect = new Rect(position.x, position.y + 160f, position.width, EditorGUIUtility.singleLineHeight);

            //들여쓰기 레벨
            var oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect contentPosition = position;

            EditorGUI.BeginProperty(position, label, property);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                if (position.height > 20f)
                {
                    position.height = 20f;
                    EditorGUI.indentLevel += 1;
                    contentPosition = EditorGUI.IndentedRect(position);
                    contentPosition.y += 22f;
                }
            }
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;
            EditorGUILayout.BeginVertical();

            GUI.enabled = false;
            EditorGUI.PropertyField(indexRect, index, new GUIContent("Index"));
            EditorGUI.FloatField(progressRect, "Progress", progress.floatValue);
            GUI.enabled = true;

            title.stringValue = EditorGUI.TextField(titleRect, "Title", title.stringValue);
            kind.intValue = EditorGUI.Popup(kindRect, "Quest Type", kind.intValue, kind.enumNames);
            state.intValue = EditorGUI.Popup(stateRect, "Quest State", state.intValue, state.enumNames);
            currentProgressCount.intValue = EditorGUI.IntField(currentProgressCountRect, "CurrentCount", currentProgressCount.intValue);
            maxProgressCount.intValue = EditorGUI.IntField(maxProgressCountRect, "MaxCount", maxProgressCount.intValue);

            switch ((QuestObjectiveType)kind.intValue)
            {
                case QuestObjectiveType.Travel:
                    target = EditorGUI.ObjectField(targetRect, "Objective Target", target, typeof(Transform), false);
                    break;
                case QuestObjectiveType.Destroy:
                    target = EditorGUI.ObjectField(targetRect, "Objective Target", target, typeof(EnemyManager), true);
                    break;
                case QuestObjectiveType.Talk:
                    target = EditorGUI.ObjectField(targetRect, "Objective Target", target, typeof(GameObject), true);
                    break;
                case QuestObjectiveType.Collect:
                    target = EditorGUI.ObjectField(targetRect, "Objective Target", target, typeof(Item), true);
                    break;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        ////This will need to be adjusted based on what you are displaying
        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{

        //    return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * 2);
        //}
    }
}

