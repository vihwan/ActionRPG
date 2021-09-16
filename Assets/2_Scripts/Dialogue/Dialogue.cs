using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SG
{
    public enum DialogType
    {
        Normal,
        Select
    }

    public enum DialogChoiceType
    {
        OpenShop,
        OpenQuest,
        OpenEnforce,
        EndDialog,
        GetMoney, //디버그용
        Yes,  //확인, 수락 등의 선택지
        No,   //취소, 거절 등의 선택지
    }

    [System.Serializable]
    public class Dialogue
    {
        public string characterName;
        public Sprite image;
        public Sentences[] sentences;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public Sprite icon;
        public string selectText;
        public DialogChoiceType dialogChoiceType;
        public Action action;
        public void AddListener(Action action)
        {
            this.action = action;
        }
    }

    [System.Serializable]
    public class Sentences
    {
        [TextArea(3, 10)]
        public string sentence;
        public DialogType dialogType = DialogType.Normal;
    }
}
