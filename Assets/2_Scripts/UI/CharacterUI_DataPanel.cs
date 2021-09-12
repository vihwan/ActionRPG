using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System;

namespace SG
{
    public class CharacterUI_DataPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text characterName;
        [SerializeField] TMP_Text characterVoice;
        [SerializeField] TMP_Text characterStatus;
        [SerializeField] TMP_Text characterExplain;

        private StringBuilder sb = new StringBuilder(); 
        public void Init()
        {
            Transform t = transform.Find("UI Background").transform;
            characterName = UtilHelper.Find<TMP_Text>(t, "Name");
            characterVoice = UtilHelper.Find<TMP_Text>(t, "CV");
            characterStatus = UtilHelper.Find<TMP_Text>(t, "Status/Text");
            characterExplain = UtilHelper.Find<TMP_Text>(t, "Explain/ExplainText");

        }
        public void OnOpenPanel()
        {
            SetCharacterDataPanel();
        }
        private void SetCharacterDataPanel()
        {
            characterName.text = "Diluc";
            characterVoice.text = "성우 : 최승훈";

            SetCharacterStatusText();
        }

        private void SetCharacterStatusText()
        {
            sb.Length = 0;

            sb.Append("생일 : 4월 30일");
            sb.AppendLine();
            sb.Append("소속 : 다운 와이너리");
            sb.AppendLine();
            sb.Append("신의 눈 : 불... 이지만 이런 게임엔 쓸모가 없지.");
            sb.AppendLine();
            sb.Append("운명의 자리 : 밤올빼미자리");

            characterStatus.text = sb.ToString();
        }
    }

}