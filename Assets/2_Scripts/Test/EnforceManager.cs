using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SG
{
    //강화할 아이템의 강화 수치에 따른 확률 조정, 필요 골드, 상승 수치 등을 관리하는 매니저
    public class EnforceManager : MonoBehaviour
    {
        public static EnforceManager Instance;

        [Header("Enforce Target")]
        [SerializeField] private Item targetItem;

        [Header("Target Item Enforce Element")]
        [SerializeField] private float successProb; //강화 성공 확률
        [SerializeField] private int enforceNeedGold;
        [SerializeField] private int enforceRiseStatus;

        //Property
        public float SuccessProb
        {
            get => successProb;
            private set
            {
                successProb = value;
                if (successProb >= 100)
                    successProb = 100;

                if (successProb < 0)
                    successProb = 0;
            }
        }
        public int EnforceNeedGold
        {
            get => enforceNeedGold;
            private set
            {
                enforceNeedGold = value;
                if (enforceNeedGold < 0)
                    enforceNeedGold = 0;
            }
        }
        public int EnforceRiseStatus
        {
            get => enforceRiseStatus;
            private set
            {
                enforceRiseStatus = value;
                if (enforceRiseStatus < 0)
                    enforceRiseStatus = 0;
            }
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        public void SetEnforceItem(Item item)
        {
            targetItem = item;
            if (targetItem != null)
                SetTargetItemEnforceElement(targetItem);
        }
        private void SetTargetItemEnforceElement(Item targetItem)
        {
            //대상 아이템의 정보에 따라 강화에 필요한 확률, 필요 골드, 상승 수치를 정해주는 메소드
            SuccessProb = SetSuccessProbability(targetItem);
            EnforceNeedGold = (int)SetEnforceNeedGold(targetItem.rarity);
            EnforceRiseStatus = SetEnforceRiseStatus(targetItem.rarity);
        }
        private int SetEnforceRiseStatus(int rarity)
        {
            return rarity;
        }
        private float SetSuccessProbability(Item item)
        {
            int x = item.rarity - 1;

            int enforceLevel = 0;
            if (item.itemType == ItemType.Weapon)
                enforceLevel = (item as WeaponItem).enforceLevel;
            else
                enforceLevel = (item as EquipItem).enforceLevel;

            switch (enforceLevel)
            {
                case 0: return x * (x - 10) + 100;
                case 1: return x * (x - 12) + 90;
                case 2: return x * (x - 14) + 80;
                case 3: return x * (x - 16) + 70;
                case 4: return x * (x - 18) + 60;
                default: break;
            }
            return 0f;
        }
        private float SetEnforceNeedGold(int rarity)
        {
            return 100 * Mathf.Pow(rarity, 2) + 3000;
        }
        internal bool TryEnforceItem()
        {
            Debug.Log("현재 강화 성공 확률 : " + successProb);
            float r = Random.Range(0f, 100f);
            if(r <= successProb)
            {
                Debug.Log("강화 성공");
                return true;
            }
            else
            {
                Debug.Log("강화 실패");
                return false;
            }
        }
    }
}
