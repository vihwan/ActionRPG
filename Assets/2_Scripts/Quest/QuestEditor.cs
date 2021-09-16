using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SG
{

    [CustomEditor(typeof(Quest)), CanEditMultipleObjects]
    public class QuestEditor : Editor
    {
        SerializedProperty property;
        Quest myQuest;
        private void OnEnable()
        {
            myQuest = target as Quest;
            property = serializedObject.FindProperty("objectives");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            //serializedObject.Update();
            //DrawList(property, "Objective Information ");

            EditorGUILayout.Space(10f);

            var style = new GUIStyle();
            style.wordWrap = true;
            style.margin = new RectOffset(0, 0, 10, 10);
            style.normal.textColor = Color.white;

            GUILayout.Label("아래의 버튼을 눌러 퀘스트가 요구하는 Objective를 생성하고 설정하세요.", style);
            if (GUILayout.Button("Add Objective"))
            {
                myQuest.AddObjective();
            }

            //최종적으로 EditorUtility.SetDirty(target);
            //코드가 있어야 사용자의 변경 사항이 실제로 적용됩니다.
            //이 코드가 없으면 인스펙터 창의 값들은 바뀌는데
            //실제로 스크립트에 적용되진 않은 모습을 보실 수 있습니다.
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        void DrawList(SerializedProperty _list, string _labalName)
        {
            //리스트 갯수 표시
            EditorGUILayout.PropertyField(_list.FindPropertyRelative("Array.size"), new GUIContent("리스트 갯수 표시"));
            int Count = _list.arraySize;
            for (int i = 0; i < Count; ++i)
            {
                EditorGUILayout.PropertyField(_list.GetArrayElementAtIndex(i), new GUIContent(_labalName + i));
            }
        }
    }
}