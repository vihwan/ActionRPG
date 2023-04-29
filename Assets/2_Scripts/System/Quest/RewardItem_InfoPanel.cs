using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class RewardItem_InfoPanel : MonoBehaviour
    {

        [Header("Basic")]
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemKind;
        [SerializeField] private TMP_Text itemEnhanceLevel;
        [SerializeField] private TMP_Text itemStatus;
        //[SerializeField] private GameObject itemDurabilityTitle;
        //[SerializeField] private TMP_Text itemDurability;
        [SerializeField] private TMP_Text itemExplain;
        [SerializeField] private Transform itemRarityTransform;
        [SerializeField] private List<GameObject> rareStars;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        private readonly StringBuilder sb = new StringBuilder();
        public void Init()
        {
            itemName = UtilHelper.Find<TMP_Text>(transform, "Name");
            itemKind = UtilHelper.Find<TMP_Text>(transform, "Kind");
            itemEnhanceLevel = UtilHelper.Find<TMP_Text>(transform, "EnhanceLevel");
            itemStatus = UtilHelper.Find<TMP_Text>(transform, "Status/Text");
            // itemDurability = UtilHelper.Find<TMP_Text>(transform, "Durability/Text");
            itemExplain = UtilHelper.Find<TMP_Text>(transform, "Explain/ExplainText");
            itemRarityTransform = transform.Find("Rarity").transform;
            // itemDurabilityTitle = transform.Find("Durability").gameObject;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
        }
        public void SetParameter(RewardItemSlot slot)
        {
            if (slot.item != null)
                SetParameterInfoPanel(slot.item);
        }
        private void SetParameterInfoPanel(Item item)
        {
            itemName.text = item.itemName;
            itemKind.text = item.kind;

            //itemDurabilityTitle.SetActive(true);
            //itemDurability.text = null;
            //itemDurability.text = weaponItem.currentDurability + " / " + weaponItem.maxDurability;
            itemExplain.text = item.itemDescription;

            if (item.itemType.Equals(ItemType.Weapon))
            {
                SetItemStatusText(itemStatus, item as WeaponItem);
                itemEnhanceLevel.text = "0단계";
            }
            else if (item.itemType.Equals(ItemType.Consumable))
            {
                itemEnhanceLevel.text = null;
                SetItemStatusText(itemStatus, item as ConsumableItem);
            }
            else if (item.itemType.Equals(ItemType.Ingredient))
            {
                itemEnhanceLevel.text = null;
                SetItemStatusText(itemStatus, item as IngredientItem);
            }
            else
            {
                itemEnhanceLevel.text = "0단계";
                SetItemStatusText(itemStatus, item as EquipItem);
            }
            CreateRarityStar(itemRarityTransform, rareStars, item);
        }

        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, Item item)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < item.rarity)
            {
                while (rarityCount < item.rarity)
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

