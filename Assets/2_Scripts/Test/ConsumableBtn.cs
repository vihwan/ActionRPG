using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class ConsumableBtn : MonoBehaviour
    {
        private System.Action clickEvent;
        private Button button;
        private Image buttonIcon;
        private Image cooldownImage;
        private SkillTimer timer;
        private bool isAct = false;

        [SerializeField]
        private float coolTime = 5f;
        private float elapsedTime = 0f;

        private bool isUpdate;
        public Button Button { get => button; }
        public void Init()
        {
            button = GetComponent<Button>();
            if (Button != null)
                Button.onClick.AddListener(OnClick);

            buttonIcon = UtilHelper.Find<Image>(transform, "Mask/Icon");

            cooldownImage = UtilHelper.Find<Image>(transform, "Cooldown");
            if (cooldownImage != null)
                cooldownImage.gameObject.SetActive(false);

            //텍스트에 연결되어있는 타이머 컴포넌트를 찾는다. true를 넣었기에, 게임 오브젝트 상태가 false여도 찾아온다.
            timer = GetComponentInChildren<SkillTimer>(true);
            if (timer != null)
                timer.Init();
        }

        private void SetCoolTime(int time)
        {
            coolTime = time;
        }

        private void SetConsumableBtnIcon(ConsumableItem consumableItem)
        {
            if (consumableItem == null)
            {
                buttonIcon.sprite = null;
                buttonIcon.enabled = false;
                return;
            }
            buttonIcon.sprite = consumableItem.itemIcon;
            buttonIcon.enabled = true;
        }

        //외부에서 실행될 함수를 받아올 함수
        public void SetEvent(System.Action action)
        {
            clickEvent += action;
        }

        public void SetActiveBtn(ConsumableItem consumableItem)
        {
            if (consumableItem != null)
            {
                isAct = true;
                SetCoolTime(consumableItem.coolTime);
                SetConsumableBtnIcon(consumableItem);
            }
            else
            {
                isAct = false;
                SetConsumableBtnIcon(consumableItem);
            }
        }


        public void OnClick()
        {
            if (clickEvent != null)
                clickEvent();

            if (isAct == false)
                return;

            //버튼이 클릭되었을 때, 버튼의 기능을 꺼둡니다.
            Button.enabled = false;
            cooldownImage.fillAmount = 1f;
            cooldownImage.gameObject.SetActive(true);
            isUpdate = true;

            print("onClick");

            if (timer != null)
            {
                timer.Execute((int)coolTime);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!isUpdate)
            {
                return;
            }

            elapsedTime += Time.deltaTime / coolTime;
            elapsedTime = Mathf.Clamp01(elapsedTime);
            // 이미지가 가득차있는 fillAmount값은 1입니다.
            // 따라서 1에서 경과시간을 빼주면 쿨타임 처리가 되게 됩니다.
            cooldownImage.fillAmount = 1 - elapsedTime;

            if (elapsedTime >= 1.0f)
            {
                elapsedTime = 0f;
                isUpdate = false;
                Button.enabled = true;
                cooldownImage.fillAmount = 0f;
                cooldownImage.gameObject.SetActive(false);
            }
        }

    }
}
