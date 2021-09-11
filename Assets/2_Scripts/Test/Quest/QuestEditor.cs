using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SG;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    Quest myQuest;
    private void OnEnable()
    {
        myQuest = (Quest)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(10f);

        //for (int i = 0; i < myQuest.objectives.Count; i++)
        //{
        //    if(myQuest.objectives[i].kind == QuestObjectiveType.Collect)
        //    {
        //        EditorGUILayout.ObjectField("Object Field", null, typeof(Sprite), true);
        //        continue;
        //    }
        //}

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
        if(GUI.changed)
            EditorUtility.SetDirty(target);
    }
}