using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace SG
{
    public class PopUpMessageMultiSelection : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text textMsg;
        [SerializeField] private TMP_Text yesText;
        [SerializeField] private TMP_Text noText;
        [SerializeField] private Button yesBtn;
        [SerializeField] private Button noBtn;

        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Slider slider;
        [SerializeField] private Button amountPlusBtn;
        [SerializeField] private Button amountMinusBtn;
        [SerializeField] private int maxAmount;

        public TMP_Text TextMsg { get => textMsg; set => textMsg = value; }
        public TMP_Text YesText { get => yesText; set => yesText = value; }
        public TMP_Text NoText { get => noText; set => noText = value; }

        public delegate void MultiSelectCallBack();
        private event MultiSelectCallBack noMultiSelectCallBack;

        // 확인 버튼 눌렀을 때 동작할 이벤트
        private event Action<int> OnAmountInputOK;

        // Start is called before the first frame update
        void Awake()
        {
            itemNameText = UtilHelper.Find<TMP_Text>(transform, "ItemName");
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

            slider = GetComponentInChildren<Slider>(true);
            if (slider != null)
            {
                slider.onValueChanged.AddListener(OnSlider);
            }

            inputField = GetComponentInChildren<TMP_InputField>(true);
            if(inputField != null)
            {
                //입력제한이 걸린 이벤트 함수 추가
                inputField.onValueChanged.AddListener(str =>
                {
                    int.TryParse(str, out int amount);
                    bool flag = false;
                    slider.value = amount;

                    if (amount < 1)
                    {
                        flag = true;
                        amount = 1;
                    }
                    else if (amount > maxAmount)
                    {
                        flag = true;
                        amount = maxAmount;
                    }

                    if (flag)
                        inputField.text = amount.ToString();                
                });
            }



            amountPlusBtn = UtilHelper.Find<Button>(transform, "PlusBtn");
            if(amountPlusBtn != null)
            {
                amountPlusBtn.onClick.AddListener(OnPlus);
            }

            amountMinusBtn = UtilHelper.Find<Button>(transform, "MinusBtn");
            if (amountMinusBtn != null)
            {
                amountMinusBtn.onClick.AddListener(OnMinus);
            }


            //임시 초기화
            maxAmount = 20;
            SetSliderMaxValue(maxAmount);
        }

        private void OnSlider(float value)
        {
            if(value < 1)
                value = 1;

            slider.value = (int) value;

            inputField.text = slider.value.ToString();
        }

        public void OnYes() { OnAmountInputOK?.Invoke(int.Parse(inputField.text)); }
        public void OnNo() { noMultiSelectCallBack?.Invoke(); }

        private void OnPlus()
        {
            int.TryParse(inputField.text, out int amount);
            if (amount < maxAmount)
            {
                // Shift 누르면 10씩 증가
                //int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount + 10 : amount + 1;
                int nextAmount = amount + 1;
                if (nextAmount > maxAmount)
                    nextAmount = maxAmount;
                inputField.text = nextAmount.ToString();
                slider.value = nextAmount;
            }
        }

        private void OnMinus()
        {
            int.TryParse(inputField.text, out int amount);
            if (amount > 1)
            {
                // Shift 누르면 10씩 감소
                int nextAmount = amount - 1;
                if (nextAmount < 1)
                    nextAmount = 1;
                inputField.text = nextAmount.ToString();
                slider.value = nextAmount;
            }
        }

        public void OpenAmountInputPopup(Action<int> okCallback, ConsumableItem item)
        {
            maxAmount = item.quantity;
            inputField.text = "1";
            slider.value = 1;

            SetSliderMaxValue(item.quantity);
            itemNameText.text = item.itemName;
            OnAmountInputOK = okCallback;
        }

        private void SetSliderMaxValue(int amount)
        {
            slider.maxValue = amount;
        }
    }
}
