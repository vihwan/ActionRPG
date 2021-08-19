using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace SG 
{
    public class CharacterUI_WeaponPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private TMP_Text weaponName;
        [SerializeField] private TMP_Text weaponKind;
        [SerializeField] private TMP_Text weaponStatus;
        [SerializeField] private TMP_Text weaponDurability;
        [SerializeField] private Transform weaponRarityTransform;
        [SerializeField] private GameObject RareStar;
        [SerializeField] private List<GameObject> rareStars;
        [SerializeField] private TMP_Text weaponExplain;
        [SerializeField] private GameObject currentEquipObject;
        [SerializeField] internal Button openPanelBtn;
        [SerializeField] internal Button changeWeaponBtn;

        [Header("Need Component")]
        [SerializeField] private PlayerInventory playerInventory;
        private CharacterWindowUI characterWindowUI; //상위 계층 

        private StringBuilder sb = new StringBuilder();
        public void Init()
        {
            weaponName = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            weaponKind = UtilHelper.Find<TMP_Text>(transform, "UI Background/Kind");
            weaponStatus = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/Text");
            weaponDurability = UtilHelper.Find<TMP_Text>(transform, "UI Background/Durability/Text");
            weaponRarityTransform = transform.Find("UI Background/Rarity");
            weaponExplain = UtilHelper.Find<TMP_Text>(transform, "UI Background/Explain/ExplainText");
            currentEquipObject = transform.Find("UI Background/CurrentState").gameObject;

            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null)
                Debug.LogWarning("playerInventory를 찾지 못했습니다.");

            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            openPanelBtn = UtilHelper.Find<Button>(transform, "UI Background/OpenPanelBtn");
            if (openPanelBtn != null)
                openPanelBtn.onClick.AddListener(OpenChangeWeaponInventory);

            changeWeaponBtn = UtilHelper.Find<Button>(transform, "UI Background/ChangeWeaponBtn");
            if (changeWeaponBtn != null)
            {
                changeWeaponBtn.onClick.AddListener(null);
                changeWeaponBtn.gameObject.SetActive(false);
            }
            characterWindowUI = GetComponentInParent<CharacterWindowUI>();
        }

        public void OnEnable()
        {
            SetParameter(playerInventory.currentWeapon);
        }
        private void AddStatusText(int value, string statName, bool isPercent = false)
        {
            if(value != 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.Append(statName);
                switch (statName.Length)
                {
                    case 2:
                        sb.Append("                      ");
                        break;

                    case 3:
                        sb.Append("                   ");
                        break;

                    case 4:
                        sb.Append("                ");
                        break;

                    case 6:
                        sb.Append("           ");
                        break;
                }


                if (value > 0)
                {
                    sb.Append("+");
                    sb.Append(" ");
                }

                if(isPercent)
                {
                    sb.Append(value);
                    sb.Append("%");
                }
                else
                {
                    sb.Append(value);
                }
            }
        }
        private void SetItemStatusText(WeaponItem playerWeapon)
        {
            sb.Length = 0;
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Hp].value, "체력");
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Attack].value, "공격력");
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Defense].value, "방어력");
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Critical].value, "치명타", isPercent: true);
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.CriticalDamage].value, "치명타 배율", isPercent: true);
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Stamina].value, "스태미나");

            weaponStatus.text = sb.ToString();
        }

        public void SetParameter(WeaponItem playerWeapon)
        {
            weaponName.text = playerWeapon.itemName;
            weaponKind.text = playerWeapon.kind;

            SetItemStatusText(playerWeapon);

            weaponDurability.text = 
                playerWeapon.currentDurability.ToString() + " / " + playerWeapon.maxDurability.ToString();

            CreateRarityStar(playerWeapon);
            weaponExplain.text = playerWeapon.itemDescription;
            SetCurrentStateObjects(playerWeapon.isArmed);
        }

        private void SetCurrentStateObjects(bool state)
        {
            //현재 장착중인 아이템이라면, 무기 교체 버튼이 사라져야함
            //장착중인 아이템이 아니라면, 무기 교체 버튼이 나타나야함
            currentEquipObject.SetActive(state);
            changeWeaponBtn.gameObject.SetActive(!state);
        }

        private void CreateRarityStar(WeaponItem playerWeapon)
        {
            int rarityCount = weaponRarityTransform.childCount;
            if (rarityCount < playerWeapon.rarity)
            {
                while (rarityCount < playerWeapon.rarity)
                {
                    GameObject star = Instantiate(RareStar, weaponRarityTransform);
                    rareStars.Add(star);
                    rarityCount++;
                }
            }
            else if (rarityCount > playerWeapon.rarity)
            {
                while (rarityCount > playerWeapon.rarity)
                {
                    Destroy(rareStars[0]); 
                    rareStars.RemoveAt(0);
                    rarityCount--;
                }
            }
                return;
        }


        //무기 교체 버튼을 누르면, 왼쪽 패널의 무기 인벤토리를 엽니다.
        private void OpenChangeWeaponInventory()
        {
            //왼쪽 패널에 무기 인벤토리를 생성
            characterWindowUI.weaponInventoryList.gameObject.SetActive(true);
            //뒤로가기 버튼 활성화
            characterWindowUI.backBtn.gameObject.SetActive(true);
            openPanelBtn.gameObject.SetActive(false);
        }
    }
}
