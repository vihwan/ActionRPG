using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SG
{
    public class PopUpMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMsg;
        [SerializeField] private TMP_Text yesText;
        [SerializeField] private TMP_Text noText;
        [SerializeField] private TMP_Text addText;
        [SerializeField] private Button yesBtn;
        [SerializeField] private Button noBtn;

        public TMP_Text TextMsg { get => textMsg; set => textMsg = value; }
        public TMP_Text YesText { get => yesText; set => yesText = value; }
        public TMP_Text NoText { get => noText; set => noText = value; }
        public TMP_Text AddText { get => addText; set => addText = value; }

        public delegate void YesNoCallBack();
        private event YesNoCallBack yesCallBack;
        private event YesNoCallBack noCallBack;
        private void Awake()
        {
            textMsg = UtilHelper.Find<TMP_Text>(transform, "TextMsg");
            yesBtn = UtilHelper.Find<Button>(transform, "YesBtn");
            if (yesBtn != null)
            {
                yesText = UtilHelper.Find<TMP_Text>(yesBtn.transform, "YesText");
                yesBtn.onClick.AddListener(OnYes);
            }

            noBtn = UtilHelper.Find<Button>(transform, "NoBtn");
            if (noBtn != null)
            {
                noText = UtilHelper.Find<TMP_Text>(noBtn.transform, "NoText");
                noBtn.onClick.AddListener(OnNo);
            }

            addText = UtilHelper.Find<TMP_Text>(transform, "AddText");
        }
        public void SetYesCallback(YesNoCallBack listener) { yesCallBack += listener; }
        public void SetNoCallback(YesNoCallBack listener) { noCallBack += listener; }
        public void SetOpenMessagePopup(string textMsg, string yesText, string noText, string addText = null)
        {
            this.textMsg.text = textMsg;
            this.yesText.text = yesText;
            this.noText.text = noText;
            this.addText.text = addText;
        }
        private void OnYes() { yesCallBack?.Invoke(); }
        private void OnNo() { noCallBack?.Invoke(); }
    }
}
