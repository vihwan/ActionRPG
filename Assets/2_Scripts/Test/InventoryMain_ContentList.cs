using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace SG
{
    public class InventoryMain_ContentList : MonoBehaviour
    {
        private Transform inventorySlotsParent;
        private TMP_Dropdown itemSort_Dropdown;
        private Button deleteBtn;
        [SerializeField] internal InventoryContentSlot[] inventoryContentSlots;

        private InventoryMainContents inventoryMainContents;
        [SerializeField] private InventoryContentSlot beforeSelectSlot;
        [SerializeField] private Item beforeSelectSlotItem;

        public InventoryContentSlot BeforeSelectSlot
        {
            get => beforeSelectSlot;
            private set
            {
                beforeSelectSlot = value;
                if (beforeSelectSlot.weaponItem != null)
                    beforeSelectSlotItem = beforeSelectSlot.weaponItem;
                else if (beforeSelectSlot.equipItem != null)
                    beforeSelectSlotItem = beforeSelectSlot.equipItem;
                else if (beforeSelectSlot.consumableItem != null)
                    beforeSelectSlotItem = beforeSelectSlot.consumableItem;
                else if (beforeSelectSlot.ingredientItem != null)
                    beforeSelectSlotItem = beforeSelectSlot.ingredientItem;
            }
        }

        public void Init()
        {
            Transform t = transform;
            inventorySlotsParent = UtilHelper.Find<Transform>(t, "Slot Parent");

            itemSort_Dropdown = UtilHelper.Find<TMP_Dropdown>(t, "Dropdown");
            if (itemSort_Dropdown != null)
            {
                InitDropdown();
                itemSort_Dropdown.onValueChanged.AddListener(OnClickSortInventoryList);
            }

            deleteBtn = UtilHelper.Find<Button>(t, "DeleteButton");
            if (deleteBtn != null)
                deleteBtn.onClick.AddListener(() => OnClickDelete(beforeSelectSlot));

            inventoryMainContents = GetComponentInParent<InventoryMainContents>();
        }
        public void SetBeforeSelectSlotInfoPanel()
        {
            //리스트를 열 때, 이전에 선택한 적이 없을 때(처음 그 리스트를 여는 것일 경우
            //첫번째 있는 인벤토리 슬롯을 이전에 선택한 슬롯으로 정하고 선택표시 및 아이템 정보를 출력합니다.
            //선택한 적이 있는 경우, 그 선택한 아이템의 슬롯 표시 및 정보를 출력합니다.

            if (beforeSelectSlotItem != null)
            {
                if (this.gameObject.name == "WeaponList")
                {
                    inventoryMainContents.infoPanel.SetParameter(beforeSelectSlotItem, "Weapon");
                }
                else if (this.gameObject.name == "TopsList" ||
                         this.gameObject.name == "BottomsList" ||
                         this.gameObject.name == "GlovesList" ||
                         this.gameObject.name == "ShoesList" ||
                         this.gameObject.name == "AccessoryList" ||
                         this.gameObject.name == "SpecialEquipList")
                {
                    inventoryMainContents.infoPanel.SetParameter(beforeSelectSlotItem, "Equip");
                }
                else if (this.gameObject.name == "ConsumableList")
                {
                    inventoryMainContents.infoPanel.SetParameter(beforeSelectSlotItem, "Consume");
                }
                else if (this.gameObject.name == "IngredientList")
                {
                    inventoryMainContents.infoPanel.SetParameter(beforeSelectSlotItem, "Ingredient");
                }
            }
            else
            {
                if (inventoryContentSlots.Length != 0)
                {
                    if (inventoryContentSlots[0] != null)
                    {
                        SortInventoryListToGANADA();
                        itemSort_Dropdown.value = 0;
                        itemSort_Dropdown.RefreshShownValue();

                        beforeSelectSlot = inventoryContentSlots[0];
                        beforeSelectSlot.isSelect = true;
                        beforeSelectSlot.ChangeBackgroundColor();
                        inventoryMainContents.infoPanel.SetParameter(beforeSelectSlot);
                    }
                }
            }
        }
        private void InitDropdown()
        {
            //드롭다운 초기화
            itemSort_Dropdown.ClearOptions();

            List<string> options = new List<string>();
            options.Add("가나다순");
            options.Add("레어도순");

            itemSort_Dropdown.AddOptions(options);
            itemSort_Dropdown.value = 0;
            itemSort_Dropdown.RefreshShownValue();
        }
        public void SetBeforeSelectSlot(InventoryContentSlot slot)
        {
            //이전에 선택한 아이템이 어떤 것인지를 설정하는 메소드
            //슬롯 버튼의 이벤트 함수로 추가됩니다.
            BeforeSelectSlot = slot;
        }
        private void SetNoneContentList(bool state)
        {
            inventoryMainContents.SetNoneText(state);
            SetActiveDropdownandDeleteButton(!state);
        }
        private void SetActiveDropdownandDeleteButton(bool state)
        {
            itemSort_Dropdown.gameObject.SetActive(state);
            deleteBtn.gameObject.SetActive(state);
        }

        public void UpdateSlots()
        {
            //List<EquipItem> tempList = playerInventory.equipmentsInventory[selectEquipType];
            for (int i = 0; i < inventoryMainContents.playerInventory.consumableInventory.Count; i++)
            {
                inventoryContentSlots[i].UpdateSlot(inventoryMainContents.playerInventory.consumableInventory[i]);
            }

            Debug.Log("소비 아이콘 색상 갱신 완료");
        }
        public void UpdateUI()
        {
            UpdateUIDependOnGameObjectName();
            SetAllSlotsDeselect();
        }
        public void UpdateUIDependOnGameObjectName()
        {
            switch (this.gameObject.name)
            {
                case "WeaponList": UpdateUIWeapon(); break;
                case "TopsList": UpdateUIEquipment(ItemType.Tops); break;
                case "BottomsList": UpdateUIEquipment(ItemType.Bottoms); break;
                case "GlovesList": UpdateUIEquipment(ItemType.Gloves); break;
                case "ShoesList": UpdateUIEquipment(ItemType.Shoes); break;
                case "AccessoryList": UpdateUIEquipment(ItemType.Accessory); break;
                case "SpecialEquipList": UpdateUIEquipment(ItemType.SpecialEquip); break;
                case "ConsumableList": UpdateUIConsumable(); break;
                case "IngredientList": UpdateUIIngredient(); break;
                default: break;
            }
        }
        public void UpdateUIWeapon()
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (inventoryContentSlots.Length < inventoryMainContents.playerInventory.weaponsInventory.Count)
            {
                int diff = inventoryMainContents.playerInventory.weaponsInventory.Count - inventoryContentSlots.Length;
                for (int i = 0; i < diff; i++)
                {
                    Instantiate(Resources.Load<InventoryContentSlot>("Prefabs/InventorySlots/InventoryContentSlotPrefab")
                                    , inventorySlotsParent);
                }
                inventoryContentSlots = inventorySlotsParent.GetComponentsInChildren<InventoryContentSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = inventoryContentSlots.Length - inventoryMainContents.playerInventory.weaponsInventory.Count;
                for (int i = 0; i < diff; i++)
                {
                    inventoryContentSlots[inventoryContentSlots.Length - 1 - i].ClearItem();
                }
            }

            if (inventoryMainContents.playerInventory.weaponsInventory.Count == 0)
            {
                SetNoneContentList(true);
                return;
            }

            //각 인벤토리 슬롯에 아이템 정보를 추가한다.
            for (int i = 0; i < inventoryMainContents.playerInventory.weaponsInventory.Count; i++)
            {
                if (inventoryMainContents.playerInventory.weaponsInventory[i].isArmed)
                {
                    inventoryContentSlots[0].AddItem(inventoryMainContents.playerInventory.weaponsInventory[i]);
                    continue;
                }
                else
                {
                    inventoryContentSlots[i].AddItem(inventoryMainContents.playerInventory.weaponsInventory[i]);
                }
            }

            SetNoneContentList(false);
        }
        public void UpdateUIEquipment(ItemType itemType)
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (inventoryContentSlots.Length < inventoryMainContents.playerInventory.equipmentsInventory[itemType].Count)
            {
                int diff = inventoryMainContents.playerInventory.equipmentsInventory[itemType].Count - inventoryContentSlots.Length;
                for (int i = 0; i < diff; i++)
                {
                    Instantiate(Resources.Load<InventoryContentSlot>("Prefabs/InventorySlots/InventoryContentSlotPrefab")
                                    , inventorySlotsParent);
                }
                inventoryContentSlots = inventorySlotsParent.GetComponentsInChildren<InventoryContentSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = inventoryContentSlots.Length - inventoryMainContents.playerInventory.equipmentsInventory[itemType].Count;
                for (int i = 0; i < diff; i++)
                {
                    inventoryContentSlots[inventoryContentSlots.Length - 1 - i].ClearItem();
                }
            }

            if (inventoryMainContents.playerInventory.equipmentsInventory[itemType].Count == 0)
            {
                SetNoneContentList(true);
                return;
            }

            int count = 0;
            foreach (EquipItem item in inventoryMainContents.playerInventory.equipmentsInventory[itemType])
            {
                inventoryContentSlots[count].AddItem(item);
                count++;
            }

            SetNoneContentList(false);
            //DeSelectAllSlots();
        }
        public void UpdateUIConsumable()
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (inventoryContentSlots.Length < inventoryMainContents.playerInventory.consumableInventory.Count)
            {
                int diff = inventoryMainContents.playerInventory.consumableInventory.Count - inventoryContentSlots.Length;
                for (int i = 0; i < diff; i++)
                {
                    Instantiate(Resources.Load<InventoryContentSlot>("Prefabs/InventorySlots/InventoryContentSlotPrefab")
                                    , inventorySlotsParent);
                }
                inventoryContentSlots = inventorySlotsParent.GetComponentsInChildren<InventoryContentSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = inventoryContentSlots.Length - inventoryMainContents.playerInventory.consumableInventory.Count;
                for (int i = 0; i < diff; i++)
                {
                    inventoryContentSlots[inventoryContentSlots.Length - 1 - i].ClearItem();
                }
            }

            if (inventoryMainContents.playerInventory.consumableInventory.Count == 0)
            {
                SetNoneContentList(true);
                return;
            }

            for (int i = 0; i < inventoryMainContents.playerInventory.consumableInventory.Count; i++)
            {
                if (inventoryMainContents.playerInventory.consumableInventory[i].isArmed)
                {
                    inventoryContentSlots[0].AddItem(inventoryMainContents.playerInventory.consumableInventory[i]);
                    continue;
                }
                else
                {
                    inventoryContentSlots[i].AddItem(inventoryMainContents.playerInventory.consumableInventory[i]);
                }
            }

            SetNoneContentList(false);
        }
        public void UpdateUIIngredient()
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (inventoryContentSlots.Length < inventoryMainContents.playerInventory.ingredientInventory.Count)
            {
                int diff = inventoryMainContents.playerInventory.ingredientInventory.Count - inventoryContentSlots.Length;
                for (int i = 0; i < diff; i++)
                {
                    Instantiate(Resources.Load<InventoryContentSlot>("Prefabs/InventorySlots/InventoryContentSlotPrefab")
                                    , inventorySlotsParent);
                }
                inventoryContentSlots = inventorySlotsParent.GetComponentsInChildren<InventoryContentSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = inventoryContentSlots.Length - inventoryMainContents.playerInventory.ingredientInventory.Count;
                for (int i = 0; i < diff; i++)
                {
                    inventoryContentSlots[inventoryContentSlots.Length - 1 - i].ClearItem();
                }
            }

            if (inventoryMainContents.playerInventory.ingredientInventory.Count == 0)
            {
                SetNoneContentList(true);
                return;
            }

            for (int i = 0; i < inventoryMainContents.playerInventory.ingredientInventory.Count; i++)
            {
                inventoryContentSlots[i].AddItem(inventoryMainContents.playerInventory.ingredientInventory[i]);
            }

            SetNoneContentList(false);
        }

        private void OnClickDelete(InventoryContentSlot beforeSelectSlot)
        {
            //삭제 버튼을 클릭 했을 때, 실행되는 이벤트 함수입니다.

            //삭제를 실행하면, 팝업 프리팹을 생성하고, 그 전에 해당 아이템이 장착중 혹은 잠김 아이템인지 확인합니다.
            //(확인하는 함수는 팝업창이 열리기 전 따로 검사하게 하는 것이 나을 듯 합니다.)
            //삭제할 버튼의 아이템 정보를 받아와 해당 아이템을 플레이어 인벤토리에서 대조하여 삭제시킵니다.
            //삭제가 종료되면, 정리된 인벤토리를 바탕으로 다시 리스트 목록을 갱신합니다.
            //삭제한 아이템의 index -1에 해당하는 아이템의 정보를 출력합니다.

            //삭제를 취소하면, 아무런 함수 실행 없이 팝업 프리팹을 파괴합니다.

            if (beforeSelectSlot.GetIsArmed().Equals(true))
            {
                Debug.Log("장착중인 아이템은 버릴 수 없습니다.");
                return;
            }

            int index = 0;
            for (int i = 0; i < inventoryContentSlots.Length; i++)
            {
                if (inventoryContentSlots[i] == beforeSelectSlot)
                {
                    index = i;
                    break;
                }
            }

            GameObject popupObj = Instantiate(Resources.Load<GameObject>("Prefabs/PopUpMsg"), this.transform.parent, false);
            popupObj.SetActive(true);

            PopUpMessage popUpMessage = popupObj.GetComponent<PopUpMessage>();
            popUpMessage.TextMsg.text = "정말 버리시겠습니까? \n" + beforeSelectSlotItem.itemName;
            popUpMessage.TextMsg.fontSize -= 3;
            popUpMessage.YesText.text = "예스";
            popUpMessage.NoText.text = "노";

            popUpMessage.SetYesCallback(() =>
            {
                Debug.Log(beforeSelectSlotItem.itemName + "를 버립니다.");
                inventoryMainContents.playerInventory.SaveDeleteItemToInventory(beforeSelectSlotItem);
                UpdateUI();
                SetIndexMinusOneItemAppear(index);
                Destroy(popupObj);
            });
            popUpMessage.SetNoCallback(() =>
            {
                Debug.Log("취소합니다.");
                Destroy(popupObj);
            });
        }

        private void SetIndexMinusOneItemAppear(int index)
        {
            SetAllSlotsDeselect();
            if (index == 0)
                index = 1; //indexOutofRange

            inventoryMainContents.infoPanel.SetParameter(inventoryContentSlots[index - 1]);
            inventoryContentSlots[index - 1].isSelect = true;
            inventoryContentSlots[index - 1].ChangeBackgroundColor();
        }

        public void SetAllSlotsDeselect()
        {
            //모든 슬롯을 미선택 상태로 바꾸는 함수입니다.
            foreach (InventoryContentSlot slot in inventoryContentSlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }
        internal void OnClickSortInventoryList(int value)
        {
            //선택한 항목에 따라 인벤토리 정렬을 수행하는 드롭다운의 이벤트 함수입니다.

            switch (value)
            {
                case 0:
                    SortInventoryListToGANADA();
                    break;
                case 1:
                    SortInventoryListToRarity();
                    break;
            }
        }
        private void SortInventoryListToGANADA()
        {
            switch (this.gameObject.name)
            {
                case "WeaponList":
                    inventoryMainContents.playerInventory.SortWeaponInventoryListToGANADA();
                    break;

                case "TopsList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToGANADA(ItemType.Tops);
                    break;

                case "BottomsList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToGANADA(ItemType.Bottoms);
                    break;

                case "GlovesList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToGANADA(ItemType.Gloves);
                    break;

                case "ShoesList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToGANADA(ItemType.Shoes);
                    break;

                case "AccessoryList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToGANADA(ItemType.Accessory);
                    break;

                case "SpecialEquipList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToGANADA(ItemType.SpecialEquip);
                    break;

                case "ConsumableList":
                    inventoryMainContents.playerInventory.SortConsumableInventoryListToGANADA();
                    break;

                case "IngredientList":
                    inventoryMainContents.playerInventory.SortIngredientInventoryListToGANADA();
                    break;

                default:
                    break;
            }
            UpdateUI();
            SetBeforeSelectSlotChangeColor();
        }
        private void SortInventoryListToRarity()
        {
            switch (this.gameObject.name)
            {
                case "WeaponList":
                    inventoryMainContents.playerInventory.SortWeaponInventoryListToRarity();
                    break;

                case "TopsList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToRarity(ItemType.Tops);
                    break;

                case "BottomsList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToRarity(ItemType.Bottoms);
                    break;

                case "GlovesList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToRarity(ItemType.Gloves);
                    break;

                case "ShoesList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToRarity(ItemType.Shoes);
                    break;

                case "AccessoryList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToRarity(ItemType.Accessory);
                    break;

                case "SpecialEquipList":
                    inventoryMainContents.playerInventory.SortEquipmentInventoryListToRarity(ItemType.SpecialEquip);
                    break;

                case "ConsumableList":
                    inventoryMainContents.playerInventory.SortConsumableInventoryListToRarity();
                    break;

                case "IngredientList":
                    inventoryMainContents.playerInventory.SortIngredientInventoryListToRarity();
                    break;

                default:
                    break;
            }
            UpdateUI();
            SetBeforeSelectSlotChangeColor();
        }
        private void SetBeforeSelectSlotChangeColor()
        {
            //인벤토리 정렬 이후 실행되는 함수
            //아이템 인벤토리가 정렬되면, 슬롯이 직접 움직이는 것이 아닌, 정보가 덧씌워지는 방식이기 때문에
            //이전에 선택한 슬롯의 정보를 새롭게 정렬된 슬롯들과 비교하여, 그 슬롯을 찾아 색상을 켜주어야합니다.

            if (beforeSelectSlot == null)
                return;

            if (beforeSelectSlot.weaponItem != null)
            {
                for (int i = 0; i < inventoryContentSlots.Length; i++)
                {
                    if (inventoryContentSlots[i].weaponItem == beforeSelectSlotItem)
                    {
                        inventoryContentSlots[i].isSelect = true;
                        inventoryContentSlots[i].ChangeBackgroundColor();
                    }
                }
            }
            else if (beforeSelectSlot.equipItem != null)
            {
                for (int i = 0; i < inventoryContentSlots.Length; i++)
                {
                    if (inventoryContentSlots[i].equipItem == beforeSelectSlotItem)
                    {
                        inventoryContentSlots[i].isSelect = true;
                        inventoryContentSlots[i].ChangeBackgroundColor();
                    }
                }
            }
            else if (beforeSelectSlot.consumableItem != null)
            {
                for (int i = 0; i < inventoryContentSlots.Length; i++)
                {
                    if (inventoryContentSlots[i].consumableItem == beforeSelectSlotItem)
                    {
                        inventoryContentSlots[i].isSelect = true;
                        inventoryContentSlots[i].ChangeBackgroundColor();
                    }
                }
            }
            else if (beforeSelectSlot.ingredientItem != null)
            {
                for (int i = 0; i < inventoryContentSlots.Length; i++)
                {
                    if (inventoryContentSlots[i].ingredientItem == beforeSelectSlotItem)
                    {
                        inventoryContentSlots[i].isSelect = true;
                        inventoryContentSlots[i].ChangeBackgroundColor();
                    }
                }
            }
        }
    }
}
