using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace SG
{
    public class Shop_InfoPanel : MonoBehaviour
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
        [SerializeField] private Button buyBtn;
        [SerializeField] private TMP_Text currentItemAmountText;

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
            itemExplainCI = UtilHelper.Find<TMP_Text>(transform, "Explain/ExplainTextCI");
            itemRarityTransform = transform.Find("Rarity").transform;
            itemDurabilityTitle = transform.Find("Durability").gameObject;
            currentItemAmountText = UtilHelper.Find<TMP_Text>(transform, "CurrentAmountImage/CurrentAmountText");
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            buyBtn = UtilHelper.Find<Button>(transform, "BuyBtn");
            if (buyBtn != null)
                buyBtn.onClick.AddListener(null); //해당 아이템 구입
        }
        public void SetParameter(Item item)
        {
            switch (item.itemType)
            {
                case ItemType.Tops: SetParameterInfoPanel(item as EquipItem); break;
                case ItemType.Bottoms: goto case ItemType.Tops;
                case ItemType.Gloves: goto case ItemType.Tops;
                case ItemType.Shoes: goto case ItemType.Tops;
                case ItemType.Accessory: goto case ItemType.Tops;
                case ItemType.SpecialEquip: goto case ItemType.Tops;
                case ItemType.Weapon: SetParameterInfoPanel(item as WeaponItem); break;
                case ItemType.Consumable: SetParameterInfoPanel(item as ConsumableItem); break;
                case ItemType.Ingredient: SetParameterInfoPanel(item as IngredientItem); break;
            }

            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() => OnClickBuyBtn(item));
        }
        private void OnClickBuyBtn(Item item)
        {
            //최소 소지 골드 사전 검사
            if (PlayerInventory.Instance.CurrentGold < item.price)
            {
                Debug.Log("돈이 없다.");
                return;
            }

            //아이템 구매 확인 팝업창
            PopUpMultiSelection popUpMulti =
                PopUpGenerator.Instance.CreatePopupMultiSelection(this.transform.parent, item, item.itemType, "구매");

            popUpMulti.SetYesCallback(num =>
            {
                OpenPopupMessage(item, num, popUpMulti.gameObject);
            });

            popUpMulti.SetNoCallback(() =>
            {
                Destroy(popUpMulti.gameObject);
            });
        }

        private void OpenPopupMessage(Item item, int num, GameObject popupMulti)
        {
            
            PopUpMessage popUp =
                PopUpGenerator.Instance.CreatePopupMessage(this.transform.parent
                                                           , string.Format("정말 구매하시겠습니까? \n <size=28> {0} : {1}개",item.itemName,num)
                                                           , "확인"
                                                           , "취소");
            // PopUpMessage popUp =
            //     PopUpGenerator.Instance.CreatePopupMessage(this.transform.parent
            //                                                , "정말 구매하시겠습니까? \n" + item.itemName + ": " + num + "개"
            //                                                , "확인"
            //                                                , "취소");
            popUp.SetYesCallback(() =>
            {
                //아이템 획득 : 플레이어 인벤토리에 추가, 골드 소모, 현재 골드 텍스트 업데이트
                if (PlayerInventory.Instance.HaveGold(item.price * num))
                {
                    PlayerInventory.Instance.SaveGetItemToInventory(item, num);
                    PlayerInventory.Instance.UseGold(item.price * num);
                    PlayerInventory.Instance.InvokeUpdateGoldText();
                    SetParameter(item);

                    Debug.Log("아이템 구매 완료");
                }
                else
                {
                    Debug.Log("돈이 없다.");
                }

                Destroy(popupMulti);
                Destroy(popUp.gameObject);
            });

            popUp.SetNoCallback(() =>
            {
                Destroy(popupMulti);
                Destroy(popUp.gameObject);
            });
        }

        private int GetCurrentAmountText(ConsumableItem consumableItem)
        {
            int count = 0;
            for (int i = 0; i < PlayerInventory.Instance.consumableInventory.Count; i++)
            {
                if (PlayerInventory.Instance.consumableInventory[i].itemName == consumableItem.itemName)
                {
                    count = PlayerInventory.Instance.consumableInventory[i].quantity;
                    break;
                }
            }
            return count;
        }
        private int GetCurrentAmountText(IngredientItem ingredientItem)
        {
            int count = 0;
            for (int i = 0; i < PlayerInventory.Instance.ingredientInventory.Count; i++)
            {
                if (PlayerInventory.Instance.ingredientInventory[i].itemName == ingredientItem.itemName)
                {
                    count = PlayerInventory.Instance.ingredientInventory[i].quantity;
                    break;
                }
            }
            return count;
        }
        private void SetParameterInfoPanel(WeaponItem weaponItem)
        {
            itemName.text = weaponItem.itemName;
            itemKind.text = weaponItem.kind;
            itemEnhanceLevel.text = null;
            itemDurabilityTitle.SetActive(true);
            itemExplain.text = weaponItem.itemDescription;

            SetItemStatusText(itemStatus, weaponItem);
            SetActiveExplainText(true);
            CreateRarityStar(itemRarityTransform, rareStars, weaponItem);
            currentItemAmountText.text = null;
        }
        private void SetParameterInfoPanel(EquipItem equipItem)
        {
            itemName.text = equipItem.itemName;
            itemKind.text = equipItem.kind;
            itemEnhanceLevel.text = "+" + equipItem.enforceLevel;
            itemExplain.text = equipItem.itemDescription;

            SetItemStatusText(itemStatus, equipItem);
            SetActiveExplainText(true);
            CreateRarityStar(itemRarityTransform, rareStars, equipItem);
            currentItemAmountText.text = null;
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
            currentItemAmountText.text = "현재 갯수 : " + GetCurrentAmountText(consumableItem) + "개";
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
            currentItemAmountText.text = "현재 갯수 : " + GetCurrentAmountText(ingredientItem) + "개";
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
