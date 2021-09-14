using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SG
{
    [System.Serializable]
    public class DialogueChoiceBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text choiceText;
        [SerializeField] private Image choiceIcon;
        [SerializeField] private Button choiceBtn;

        private Action choiceAction;
        public void Init()
        {
            choiceText = UtilHelper.Find<TMP_Text>(transform, "Text");
            choiceIcon = UtilHelper.Find<Image>(transform, "Icon");
            choiceBtn = GetComponent<Button>();
            if (choiceBtn != null)
                choiceBtn.onClick.AddListener(OnClick);
        }

        public void SetDialogChoice(string text, Sprite icon, Action action)
        {
            choiceText.text = text;
            choiceIcon.sprite = icon;
            choiceAction = action;
        }

        private void OnClick()
        {
            choiceAction?.Invoke();
        }
    }
}

