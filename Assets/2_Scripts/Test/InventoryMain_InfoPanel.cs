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
        [SerializeField] private Transform itemRarityTransform;
        [SerializeField] private List<GameObject> rareStars;
        [SerializeField] private GameObject currentEquipObject;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        private readonly StringBuilder sb = new StringBuilder();
        public void Init()
        {
            itemName = UtilHelper.Find<TMP_Text>(transform, "Name");
            itemKind = UtilHelper.Find<TMP_Text>(transform, "Kind");
            itemEnhanceLevel = UtilHelper.Find<TMP_Text>(transform, "EnhanceLevel");
            itemStatus = UtilHelper.Find<TMP_Text>(transform, "Status/Text");
            itemDurability = UtilHelper.Find<TMP_Text>(transform, "Durability/Text");
            itemExplain = UtilHelper.Find<TMP_Text>(transform, "Explain/ExplainText");
            itemRarityTransform = transform.Find("Rarity").transform;
            currentEquipObject = transform.Find("CurrentState").gameObject;
            itemDurabilityTitle = transform.Find("Durability").gameObject;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
        }

        public void SetParameter(InventoryContentSlot slot)
        {
            if(slot.weaponItem != null)
                SetParameterInfoPanel(slot.weaponItem);
            else if (slot.equipItem != null)
                SetParameterInfoPanel(slot.equipItem);
            else if(slot.consumableItem != null)
                SetParameterInfoPanel(slot.consumableItem);
            else if(slot.ingredientItem != null)
                SetParameterInfoPanel(slot.ingredientItem);
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
            itemExplain.text = consumableItem.itemDescription;
            itemStatus.text = null;

            CreateRarityStar(itemRarityTransform, rareStars, consumableItem);
            //SetCurrentStateObjects(consumableItem.isArmed);
        }

        private void SetParameterInfoPanel(IngredientItem ingredientItem)
        {
            itemName.text = ingredientItem.itemName;
            itemKind.text = ingredientItem.kind;
            itemEnhanceLevel.text = null;
            itemDurability.text = null;
            itemDurabilityTitle.SetActive(false);
            itemExplain.text = ingredientItem.itemDescription;
            itemStatus.text = null;

            CreateRarityStar(itemRarityTransform, rareStars, ingredientItem);
        }

        private void SetCurrentStateObjects(bool state)
        {
            currentEquipObject.SetActive(state);
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
