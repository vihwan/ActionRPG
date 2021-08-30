using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
                deleteBtn.onClick.AddListener(OnClickDelete);

            inventoryMainContents = GetComponentInParent<InventoryMainContents>();
        }

        public void SetBeforeSelectSlotInfoPanel()
        {
            if (beforeSelectSlot != null)
                inventoryMainContents.infoPanel.SetParameter(beforeSelectSlot);
            else
            {
                if(inventoryContentSlots[0] != null)
                {
                    inventoryMainContents.infoPanel.SetParameter(inventoryContentSlots[0]);
                }         
            }

            SortInventoryListToGANADA();
            itemSort_Dropdown.value = 0;
            itemSort_Dropdown.RefreshShownValue();
        }

        private void InitDropdown()
        {
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
            beforeSelectSlot = slot;
        }

        public void UpdateUI()
        {
            UpdateUIDependOnGameObjectName();
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

        public void UpdateUIDependOnGameObjectName()
        {
            switch (this.gameObject.name)
            {
                case "WeaponList":
                    UpdateUIWeapon();
                    break;

                case "TopsList":
                    UpdateUIEquipment(ItemType.Tops);
                    break;

                case "BottomsList":
                    UpdateUIEquipment(ItemType.Bottoms);
                    break;

                case "GlovesList":
                    UpdateUIEquipment(ItemType.Gloves);
                    break;

                case "ShoesList":
                    UpdateUIEquipment(ItemType.Shoes);
                    break;

                case "AccessoryList":
                    UpdateUIEquipment(ItemType.Accessory);
                    break;

                case "SpecialEquipList":
                    UpdateUIEquipment(ItemType.SpecialEquip);
                    break;

                case "ConsumableList":
                    UpdateUIConsumable();
                    break;

                case "IngredientList":
                    UpdateUIIngredient();
                    break;

                default:
                    break;
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
                for (int i = diff; i > 0; i--)
                {
                    inventoryContentSlots[diff].ClearItem();
                }
            }


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

            //DeSelectAllSlots();
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
                for (int i = 1; i <= diff; i++)
                {
                    inventoryContentSlots[inventoryContentSlots.Length - i].ClearItem();
                }
            }

            int count = 0;
            foreach (EquipItem item in inventoryMainContents.playerInventory.equipmentsInventory[itemType])
            {
                inventoryContentSlots[count].AddItem(item);
                count++;
            }

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
                for (int i = diff; i > 0; i--)
                {
                    inventoryContentSlots[diff].ClearItem();
                }
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

            DeSelectAllSlots();
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
                for (int i = diff; i > 0; i--)
                {
                    inventoryContentSlots[diff].ClearItem();
                }
            }

            for (int i = 0; i < inventoryMainContents.playerInventory.ingredientInventory.Count; i++)
            {
                inventoryContentSlots[i].AddItem(inventoryMainContents.playerInventory.ingredientInventory[i]);
            }
        }


        private void OnClickDelete()
        {

        }


        public void DeSelectAllSlots()
        {
            foreach (InventoryContentSlot slot in inventoryContentSlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }
        internal void OnClickSortInventoryList(int value)
        {
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
        }
    }
}
