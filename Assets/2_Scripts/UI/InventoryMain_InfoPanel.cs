using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace SG
{
    public class InventoryMain_InfoPanel : MonoBehaviour
    {
        [Header("Basic")]
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemKind;
        [SerializeField] private TMP_Text itemEnhanceLevel;
        [SerializeField] private TMP_Text itemStatus;
        [SerializeField] private GameObject itemDurabilityTitle;
        [SerializeField] private TMP_Text itemDurability;
        [SerializeField] private TMP_Text itemExplain;
        [SerializeField] private TMP_Text itemExplainCI;
        [SerializeField] private Transform itemRarityTransform;
        [SerializeField] private List<GameObject> rareStars;
        [SerializeField] private GameObject currentEquipObject;
        [SerializeField] internal Button consumableEquipBtn;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        private InventoryMainContents mainContents;
        private PlayerInventory playerInventory;

        private readonly StringBuilder sb = new StringBuilder();
        public void Init()
        {
            mainContents = GetComponentInParent<InventoryMainContents>();
            playerInventory = FindObjectOfType<PlayerInventory>();

            itemName = UtilHelper.Find<TMP_Text>(transform, "Name");
            itemKind = UtilHelper.Find<TMP_Text>(transform, "Kind");
            itemEnhanceLevel = UtilHelper.Find<TMP_Text>(transform, "EnhanceLevel");
            itemStatus = UtilHelper.Find<TMP_Text>(transform, "Status/Text");
            itemDurability = UtilHelper.Find<TMP_Text>(transform, "Durability/Text");
            itemExplain = UtilHelper.Find<TMP_Text>(transform, "Explain/ExplainText");
            itemExplainCI = UtilHelper.Find<TMP_Text>(transform, "Explain/ExplainTextCI");
            itemRarityTransform = transform.Find("Rarity").transform;
            currentEquipObject = transform.Find("CurrentState").gameObject;
            itemDurabilityTitle = transform.Find("Durability").gameObject;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
            consumableEquipBtn = UtilHelper.Find<Button>(transform, "EquipConsumableBtn");
            if (consumableEquipBtn != null)
            {
                consumableEquipBtn.onClick.AddListener(null);
                consumableEquipBtn.gameObject.SetActive(false);
            }
        }
        public void SetParameter(InventoryContentSlot slot)
        {
            if (slot.weaponItem != null)
                SetParameterInfoPanel(slot.weaponItem);
            else if (slot.equipItem != null)
                SetParameterInfoPanel(slot.equipItem);
            else if (slot.consumableItem != null)
                SetParameterInfoPanel(slot.consumableItem);
            else if (slot.ingredientItem != null)
                SetParameterInfoPanel(slot.ingredientItem);
        }

        public void SetParameter(Item item, string type)
        {
            switch (type)
            {
                case "Weapon":
                    SetParameterInfoPanel(item as WeaponItem);
                    break;
                case "Equip":
                    SetParameterInfoPanel(item as EquipItem);
                    break;
                case "Consume":
                    SetParameterInfoPanel(item as ConsumableItem);
                    break;
                case "Ingredient":
                    SetParameterInfoPanel(item as IngredientItem);
                    break;

                default:
                    break;
            }
        }

        private void SetParameterInfoPanel(WeaponItem weaponItem)
        {
            itemName.text = weaponItem.itemName;
            itemKind.text = weaponItem.kind;
            itemEnhanceLevel.text = null;
            itemDurabilityTitle.SetActive(true);
            itemDurability.text = weaponItem.currentDurability + " / " + weaponItem.maxDurability;
            itemExplain.text = weaponItem.itemDescription;

            SetItemStatusText(itemStatus, weaponItem);
            SetActiveExplainText(true);
            CreateRarityStar(itemRarityTransform, rareStars, weaponItem);
            SetCurrentStateObjects(weaponItem.isArmed);
        }
        private void SetParameterInfoPanel(EquipItem equipItem)
        {
            itemName.text = equipItem.itemName;
            itemKind.text = equipItem.kind;
            itemEnhanceLevel.text = "+" + equipItem.enhanceLevel;
            itemDurabilityTitle.SetActive(true);
            itemDurability.text = equipItem.currentDurability + " / " + equipItem.maxDurability;
            itemExplain.text = equipItem.itemDescription;

            SetItemStatusText(itemStatus, equipItem);
            SetActiveExplainText(true);
            CreateRarityStar(itemRarityTransform, rareStars, equipItem);
            SetCurrentStateObjects(equipItem.isArmed);
        }
        private void SetParameterInfoPanel(ConsumableItem consumableItem)
        {
            itemName.text = consumableItem.itemName;
            itemKind.text = consumableItem.kind;
            itemEnhanceLevel.text = null;
            itemDurability.text = null;
            itemDurabilityTitle.SetActive(false);
            itemExplainCI.text = consumableItem.itemDescription;

            SetItemStatusText(itemStatus, consumableItem);
            SetActiveExplainText(false);
            CreateRarityStar(itemRarityTransform, rareStars, consumableItem);
            SetCurrentStateObjects(consumableItem.isArmed);

            if (consumableEquipBtn.gameObject.activeSelf == true)
            {
                consumableEquipBtn.onClick.RemoveAllListeners();
                consumableEquipBtn.onClick.AddListener(() => ChangeConsumableEquipBtnListener(consumableItem));
            }
        }
        private void SetParameterInfoPanel(IngredientItem ingredientItem)
        {
            itemName.text = ingredientItem.itemName;
            itemKind.text = ingredientItem.kind;
            itemEnhanceLevel.text = null;
            itemDurability.text = null;
            itemDurabilityTitle.SetActive(false);
            itemExplainCI.text = ingredientItem.itemDescription;

            SetItemStatusText(itemStatus, ingredientItem);
            SetActiveExplainText(false);
            CreateRarityStar(itemRarityTransform, rareStars, ingredientItem);
        }
        private void SetCurrentStateObjects(bool state)
        {
            currentEquipObject.SetActive(state);

            if (mainContents.ConsumableList.gameObject.activeSelf == true)
                consumableEquipBtn.gameObject.SetActive(!state);
            else if (mainContents.ConsumableList.gameObject.activeSelf == false)
                consumableEquipBtn.gameObject.SetActive(false);
        }
        public void ChangeConsumableEquipBtnListener(ConsumableItem consumableItem)
        {
            //클릭하면
            //장착중인 아이템표시 변경
            //퀵슬롯에 해당 아이템 등록
            mainContents.playerInventory.ChangeCurrentConsumable(consumableItem);
            SetCurrentStateObjects(consumableItem.isArmed);
            mainContents.ConsumableList.UpdateSlots();
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
        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, ConsumableItem consumableItem)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < consumableItem.rarity)
            {
                while (rarityCount < consumableItem.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
        }
        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, IngredientItem ingredientItem)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < ingredientItem.rarity)
            {
                while (rarityCount < ingredientItem.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
        }
        private void SetActiveExplainText(bool state)
        {
            itemExplain.gameObject.SetActive(state);
            itemExplainCI.gameObject.SetActive(!state);
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
            tMP_Text.fontSize = 34;
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
            tMP_Text.fontSize = 34;
        }
        private void SetItemStatusText(TMP_Text tMP_Text, ConsumableItem consumableItem)
        {
            sb.Length = 0;
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Hp].value, "체력회복");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Mp].value, "마나회복");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Stamina].value, "스태미나");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Attack].value, "공격력");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Defense].value, "방어력");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Critical].value, "치명타");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.CriticalDamage].value, "치명타 피해");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.Speed].value, "속도");
            AddConsumableText(consumableItem.consumableAttributes[(int)ConsumeAttribute.SkillDamage].value, "스킬 데미지");

            tMP_Text.text = sb.ToString();
            tMP_Text.fontSize = 28;
        }
        private void SetItemStatusText(TMP_Text tMP_Text, IngredientItem ingredientItem)
        {
            sb.Length = 0;
            AddIngredientText(ingredientItem.kind);

            tMP_Text.text = sb.ToString();
            tMP_Text.fontSize = 28;
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
        private void AddConsumableText(int value, string statName, bool isPercent = false)
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
        private void AddIngredientText(string kind)
        {
            switch (kind)
            {
                default: break;
                case "광석": sb.Append("사용처 : 대장간"); break;
                case "소재":
                    sb.Append("사용처 : 캐릭터 육성");
                    sb.AppendLine();
                    sb.Append("사용처 : 아이템 제작");
                    break;
            }
        }
    }
}
