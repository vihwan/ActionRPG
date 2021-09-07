using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnforceMaterialSelectList : MonoBehaviour
    {
        [Header("Selected Item")]
        [SerializeField] private Item selectedItem;

        [Header("Material Slot")]
        [SerializeField] internal EnforceItemSlot[] enforceItemSlots;
        [SerializeField] private List<Item> tempItems = new List<Item>();
        [SerializeField] private Transform enforceSlotsParent;

        [Header("Material InfoPanel")]
        [SerializeField] private GameObject materialInfoPanel;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text kindText;
        [SerializeField] private TMP_Text enforceLevelText;
        [SerializeField] private Transform itemRarityTransform;
        [SerializeField] private List<GameObject> rareStars;
        [SerializeField] private TMP_Text itemStatus;
        [SerializeField] private TMP_Text itemDurability;
        [SerializeField] private Button selectBtn;
        [SerializeField] private readonly int statusFontSize = 32;
        [SerializeField] private TMP_Text nontListText;


        [SerializeField] private Button backBtn;

        [Header("Prefab")]
        [SerializeField] private EnforceItemSlot enforceItemSlotPrefab;
        [SerializeField] private GameObject RareStar;

        private EnforceWindowUI enforceWindowUI;

        private StringBuilder sb = new StringBuilder();

        public void Init()
        {
            enforceWindowUI = GetComponentInParent<EnforceWindowUI>();

            enforceSlotsParent = transform.Find("Inventory Slot Parent").transform;
            materialInfoPanel = transform.Find("Material InfoPanel").gameObject;
            nameText = UtilHelper.Find<TMP_Text>(materialInfoPanel.transform, "Name");
            kindText = UtilHelper.Find<TMP_Text>(materialInfoPanel.transform, "Kind");
            enforceLevelText = UtilHelper.Find<TMP_Text>(materialInfoPanel.transform, "EnforceLevel");
            itemStatus = UtilHelper.Find<TMP_Text>(materialInfoPanel.transform, "Status/Text");
            itemDurability = UtilHelper.Find<TMP_Text>(materialInfoPanel.transform, "Durability/Text");
            itemRarityTransform = materialInfoPanel.transform.Find("Rarity").transform;
            selectBtn = UtilHelper.Find<Button>(materialInfoPanel.transform, "SelectBtn");
            nontListText = UtilHelper.Find<TMP_Text>(transform, "NoneListText");


            backBtn = UtilHelper.Find<Button>(transform, "BackBtn");
            if (backBtn != null)
                backBtn.onClick.AddListener(OnClickBackBtn);

            enforceItemSlotPrefab = Resources.Load<EnforceItemSlot>("Prefabs/InventorySlots/EnforceItemSlotPrefab");
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            materialInfoPanel.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void OnClickBackBtn()
        {
            this.gameObject.SetActive(false);
        }

        //선택한 장비와 동일한 이름을 가진 장비들을 전부 불러와서 리스트로 보여준다.
        public void OpenList(Item item)
        {
            this.gameObject.SetActive(true);
            this.selectedItem = item;

            if (materialInfoPanel.activeSelf.Equals(true))
                materialInfoPanel.SetActive(false);

            if(this.selectedItem.itemType == ItemType.Weapon)
            {
                UpdateListWeapon(item as WeaponItem);
            }
            else
            {
                UpdateListEquipment(item as EquipItem);
            }      
        }
        private void UpdateListWeapon(WeaponItem weaponItem)
        {
            //리스트로 보여줘야할 아이템과 인벤토리 안에 있는 이름이 같은 아이템의 갯수를 구한다.
            // *주의* 선택한 장비는 리스트에 미포함시켜야한다.
            // 1. 인벤토리를 탐색하여, 추가해야할 슬롯의 갯수를 구합니다.
            // 2. 구한 슬롯의 갯수에 따라 아이템 슬롯을 활성화/비활성화 합니다.
            // 3. 슬롯의 갯수가 0개인지를 확인합니다.
            // 4. 슬롯의 갯수가 0개가 아니라면, 각 슬롯을 설정하고 버튼 이벤트를 추가합니다.

            tempItems.Clear();
            int slotNum = 0;
            for (int i = 0; i < PlayerInventory.Instance.weaponsInventory.Count; i++)
            {
                if (PlayerInventory.Instance.weaponsInventory[i].itemName == weaponItem.itemName)
                {
                    if(PlayerInventory.Instance.weaponsInventory[i] == selectedItem)
                    {
                        continue;
                    }
                    tempItems.Add(PlayerInventory.Instance.weaponsInventory[i]);
                    slotNum++;
                }
            }



            //동일한 아이템의 갯수에 따라 
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (enforceItemSlots.Length < slotNum)
            {
                int dex = slotNum - enforceItemSlots.Length;
                for (int i = 0; i < dex; i++)
                {
                    Instantiate(enforceItemSlotPrefab, enforceSlotsParent);
                }
                enforceItemSlots = enforceSlotsParent.GetComponentsInChildren<EnforceItemSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = enforceItemSlots.Length - slotNum;
                for (int i = 0; i < diff; i++)
                {
                    enforceItemSlots[enforceItemSlots.Length - 1 - i].ClearEnforceItemSlot();
                }
            }

            //만약 슬롯의 갯수가 0개라면 리턴
            if (slotNum == 0)
            {
                Debug.Log("동일한 아이템이 존재하지 않습니다.");
                nontListText.gameObject.SetActive(true);
                return;
            }
            else
                nontListText.gameObject.SetActive(false);

            //위의 생성된 슬롯들을 토대로 임시 저장한 리스트에서 가져와 아이템을 세팅
            for (int i = 0; i < slotNum; i++)
            {
                Item slotItem = tempItems[i];
                enforceItemSlots[i].SetEnforceItemSlot(slotItem);
                enforceItemSlots[i].AddBtnListener(() => SetParameterPanel(slotItem as WeaponItem));
            }

            SetAllSlotsDeselect();
        }
        private void UpdateListEquipment(EquipItem equipItem)
        {
            //리스트로 보여줘야할 아이템과 인벤토리 안에 있는 이름이 같은 아이템의 갯수를 구한다.
            // *주의* 선택한 장비는 리스트에 미포함시켜야한다.
            // 1. 인벤토리를 탐색하여, 추가해야할 슬롯의 갯수를 구합니다.
            // 2. 구한 슬롯의 갯수에 따라 아이템 슬롯을 활성화/비활성화 합니다.
            // 3. 슬롯의 갯수가 0개인지를 확인합니다.
            // 4. 슬롯의 갯수가 0개가 아니라면, 각 슬롯을 설정하고 버튼 이벤트를 추가합니다.

            tempItems.Clear();
            int slotNum = 0;
            for (int i = 0; i < PlayerInventory.Instance.equipmentsInventory[equipItem.itemType].Count; i++)
            {
                if (PlayerInventory.Instance.equipmentsInventory[equipItem.itemType][i].itemName == equipItem.itemName)
                {
                    if (PlayerInventory.Instance.equipmentsInventory[equipItem.itemType][i] == selectedItem)
                    {
                        continue;
                    }

                    tempItems.Add(PlayerInventory.Instance.equipmentsInventory[equipItem.itemType][i]);
                    slotNum++;
                }
            }


            //동일한 아이템의 갯수에 따라 
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (enforceItemSlots.Length < slotNum)
            {
                int dex = slotNum - enforceItemSlots.Length;
                for (int i = 0; i < dex; i++)
                {
                    Instantiate(enforceItemSlotPrefab, enforceSlotsParent);
                }
                enforceItemSlots = enforceSlotsParent.GetComponentsInChildren<EnforceItemSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = enforceItemSlots.Length - slotNum;
                for (int i = 0; i < diff; i++)
                {
                    enforceItemSlots[enforceItemSlots.Length - 1 - i].ClearEnforceItemSlot();
                }
            }

            //만약 슬롯의 갯수가 0개라면 리턴
            if (slotNum == 0)
            {
                Debug.Log("동일한 아이템이 존재하지 않습니다.");
                nontListText.gameObject.SetActive(true);
                return;
            }
            else
                nontListText.gameObject.SetActive(false);


            //위의 생성된 슬롯들을 토대로 임시 저장한 리스트에서 가져와 아이템을 세팅
            for (int i = 0; i < slotNum; i++)
            {
                Item slotItem = tempItems[i];
                enforceItemSlots[i].SetEnforceItemSlot(slotItem);
                enforceItemSlots[i].AddBtnListener(() => SetParameterPanel(slotItem as EquipItem));
            }

            SetAllSlotsDeselect();
        }

        public void SetAllSlotsDeselect()
        {
            foreach (EnforceItemSlot slot in enforceItemSlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }

        private void SetParameterPanel(WeaponItem weaponItem)
        {
            if (materialInfoPanel.gameObject.activeSelf.Equals(false))
                materialInfoPanel.gameObject.SetActive(true);

            nameText.text = weaponItem.itemName;
            kindText.text = weaponItem.kind;
            enforceLevelText.text = "+" + weaponItem.enforceLevel;
            itemDurability.text = weaponItem.currentDurability + " / " + weaponItem.maxDurability;

            SetItemStatusText(itemStatus, weaponItem);
            CreateRarityStar(itemRarityTransform, rareStars, weaponItem);

            selectBtn.onClick.RemoveAllListeners();
            selectBtn.onClick.AddListener(()=> OnClickSelectBtn(weaponItem));
        }
        private void SetParameterPanel(EquipItem equipItem)
        {
            if (materialInfoPanel.gameObject.activeSelf.Equals(false))
                materialInfoPanel.gameObject.SetActive(true);

            nameText.text = equipItem.itemName;
            kindText.text = equipItem.kind;
            enforceLevelText.text = "+" + equipItem.enforceLevel;
            itemDurability.text = equipItem.currentDurability + " / " + equipItem.maxDurability;

            SetItemStatusText(itemStatus, equipItem);
            CreateRarityStar(itemRarityTransform, rareStars, equipItem);

            selectBtn.onClick.RemoveAllListeners();
            selectBtn.onClick.AddListener(() => OnClickSelectBtn(equipItem));
        }

        private void OnClickSelectBtn(Item item)
        {
            //enforceWindowUI.enforceUI_RightPanel에 아이템을 추가.
            Debug.Log("enforceWindowUI.enforceUI_RightPanel 에 아이템을 추가.");
            enforceWindowUI.enforceUI_RightPanel.SetMaterialSlot(item);
            this.gameObject.SetActive(false);
        }

        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, WeaponItem playerWeapon)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < playerWeapon.rarity)
            {
                while (rarityCount < playerWeapon.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
        }
        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, EquipItem equipItem)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < equipItem.rarity)
            {
                while (rarityCount < equipItem.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
        }
        private void SetItemStatusText(TMP_Text tMP_Text, WeaponItem weaponItem)
        {
            sb.Length = 0;
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Hp].value, "체력");
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Attack].value, "공격력");
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Defense].value, "방어력");
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Critical].value, "치명타", isPercent: true);
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.CriticalDamage].value, "치명타 배율", isPercent: true);
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Stamina].value, "스태미나");

            tMP_Text.text = sb.ToString();
            tMP_Text.fontSize = statusFontSize;
        }
        private void SetItemStatusText(TMP_Text tMP_Text, EquipItem equipItem)
        {
            sb.Length = 0;
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Hp].value, "체력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Attack].value, "공격력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Defense].value, "방어력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Critical].value, "치명타", isPercent: true);
            AddStatusText(equipItem.itemAttributes[(int)Attribute.CriticalDamage].value, "치명타 배율", isPercent: true);
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Stamina].value, "스태미나");

            tMP_Text.text = sb.ToString();
            tMP_Text.fontSize = statusFontSize;
        }
        private void AddStatusText(int value, string statName, bool isPercent = false)
        {
            if (value != 0)
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

                if (isPercent)
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
    }
}
