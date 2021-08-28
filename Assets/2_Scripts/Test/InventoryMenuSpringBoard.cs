using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class InventoryMenuSpringBoard : MonoBehaviour
    {
        [Header("Select Text")]
        [SerializeField] private TMP_Text selectText;

        [Header("Button Menu")]
        [SerializeField] private Button weaponBtn;
        [SerializeField] private Button topsBtn;
        [SerializeField] private Button bottomsBtn;
        [SerializeField] private Button glovesBtn;
        [SerializeField] private Button shoesBtn;
        [SerializeField] private Button accessoryBtn;
        [SerializeField] private Button specialEquipBtn;
        [SerializeField] private Button consumableBtn;
        [SerializeField] private Button ingredientBtn;

        InventoryWindowUI inventoryWindowUI;
        public void Init()
        {
            inventoryWindowUI = GetComponentInParent<InventoryWindowUI>();

            selectText = GetComponentInChildren<TMP_Text>();

            weaponBtn = UtilHelper.Find<Button>(transform, "Weapon");
            if (weaponBtn != null)
                weaponBtn.onClick.AddListener(null);

            topsBtn = UtilHelper.Find<Button>(transform, "Tops");
            if (topsBtn != null)
                topsBtn.onClick.AddListener(null);

            bottomsBtn = UtilHelper.Find<Button>(transform, "Bottoms");
            if (bottomsBtn != null)
                bottomsBtn.onClick.AddListener(null);

            glovesBtn = UtilHelper.Find<Button>(transform, "Gloves");
            if (glovesBtn != null)
                glovesBtn.onClick.AddListener(null);

            shoesBtn = UtilHelper.Find<Button>(transform, "Shoes");
            if (shoesBtn != null)
                shoesBtn.onClick.AddListener(null);

            accessoryBtn = UtilHelper.Find<Button>(transform, "Accessory");
            if (accessoryBtn != null)
                accessoryBtn.onClick.AddListener(null);

            specialEquipBtn = UtilHelper.Find<Button>(transform, "SpecialEquip");
            if (specialEquipBtn != null)
                specialEquipBtn.onClick.AddListener(null);

            consumableBtn = UtilHelper.Find<Button>(transform, "Consumable");
            if (consumableBtn != null)
                consumableBtn.onClick.AddListener(null);

            ingredientBtn = UtilHelper.Find<Button>(transform, "Ingredient");
            if (ingredientBtn != null)
                ingredientBtn.onClick.AddListener(null);
        }
    }
}
