using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace SG
{
    public class Message_GetItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Image itemImage;
        [SerializeField] private Item item;
        [SerializeField] private float moveSpeed = 10f;

        public Vector3 targetPosition;
        private Vector2 tempPosition;
        public bool canMove = false;

        private void Awake()
        {
            nameText = UtilHelper.Find<TMP_Text>(transform, "NameText");
            amountText = UtilHelper.Find<TMP_Text>(transform, "AmountText");
            itemImage = UtilHelper.Find<Image>(transform, "IconMask/Icon");        
        }

        private void OnEnable()
        {
            RemoveEffect(5f);
        }

        private void Update()
        {
            if (canMove)
                MoveMessage();
        }

        private void MoveMessage()
        {
            //자신과 옮길 목표 위치 사이의 절대값이 0.1 이상이면 계속 Lerp를 실행
            if (Mathf.Abs(targetPosition.x - transform.position.x) > .1 ||
                Mathf.Abs(targetPosition.y - transform.position.y) > .1)
            {
                tempPosition = new Vector2(targetPosition.x, targetPosition.y);
                transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed * Time.deltaTime);
            }
            else
            {   //이동 완료
                transform.position = new Vector2(targetPosition.x, targetPosition.y);
                canMove = false;
            }
        }
        private void ActiveFalsethisObject()
        {
            PopUpGenerator.Instance.ReturnObjectToPool(this.gameObject);
        }

        public void RemoveEffect(float time = 3f)
        {
            Invoke(nameof(ActiveFalsethisObject), time);
        }

        public void SetGetItemMessage(Item _item, int amount)
        {
            item = _item;
            nameText.text = item.itemName;
            amountText.text = "× " + amount;
            itemImage.sprite = item.itemIcon;
        }

        public void SetGetGoldMessage(Sprite gold, int money)
        {
            nameText.text = "골드";
            amountText.text = money + " G";
            itemImage.sprite = gold;
        }

        public void SetGetExpMessage(Sprite exp, int expAmount)
        {
            nameText.text = "경험치";
            amountText.text = string.Format("{0} Exp", expAmount);
            itemImage.sprite = exp;
        }
    }
}
