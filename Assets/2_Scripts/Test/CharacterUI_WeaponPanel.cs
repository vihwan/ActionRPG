using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG 
{
    public class CharacterUI_WeaponPanel : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private TMP_Text weaponName;
        [SerializeField] private TMP_Text weaponKind;
        [SerializeField] private TMP_Text weaponAttack;
        [SerializeField] private TMP_Text weaponDurability;
        [SerializeField] private Transform weaponRarityTransform;
        [SerializeField] private GameObject RareStar;
        [SerializeField] private TMP_Text weaponExplain;
        [SerializeField] private GameObject currentEquipObject;

        [Header("Need Component")]
        [SerializeField] private PlayerInventory playerInventory;
        public void Init()
        {
            weaponName = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            weaponKind = UtilHelper.Find<TMP_Text>(transform, "UI Background/Kind");
            weaponAttack = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/ATK/Text");
            weaponDurability = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/Durability/Text");
            weaponRarityTransform = transform.Find("UI Background/Status/Rarity");
            weaponExplain = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/Explain/ExplainText");
            currentEquipObject = transform.Find("UI Background/CurrentState").gameObject;

            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null)
                Debug.LogWarning("playerInventory를 찾지 못했습니다.");

            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
        }

        public void SetParameter()
        {
            WeaponItem playerWeapon = playerInventory.rightWeapon;
            weaponName.text = playerWeapon.itemName;
            weaponKind.text = playerWeapon.kind;
            weaponAttack.text = playerWeapon.attack.ToString();
            weaponDurability.text = 
                playerWeapon.currentDurability.ToString() + " / " + playerWeapon.maxDurability.ToString();

            CreateRarityStar(playerWeapon);

            weaponExplain.text = playerWeapon.itemDescription;
            currentEquipObject.SetActive(!playerWeapon.isUnarmed);
        }

        private void CreateRarityStar(WeaponItem playerWeapon)
        {
            int rarityCount = weaponRarityTransform.childCount;
            if (rarityCount < playerWeapon.rarity)
            {
                while (rarityCount < playerWeapon.rarity)
                {
                    Instantiate(RareStar, weaponRarityTransform);
                    rarityCount = weaponRarityTransform.childCount;
                }
            }
            else
                return;
        }
    }
}
