using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SG;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var myQuest = (Quest)target;

        var style = new GUIStyle();
        style.wordWrap = true;
        style.margin = new RectOffset(0, 0, 10, 10);
        style.normal.textColor = Color.white;

        GUILayout.Label("아래의 버튼을 눌러 퀘스트가 요구하는 Objective를 생성하고 설정하세요.", style);

        if (GUILayout.Button("Add Objective"))
        {
            myQuest.AddObjective();
        }
    }
}