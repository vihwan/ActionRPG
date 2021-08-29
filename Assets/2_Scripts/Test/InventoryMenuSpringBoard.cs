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
        [SerializeField] private List<Button> springMenuBtns;
        [SerializeField] private Button weaponBtn;
        [SerializeField] private Button topsBtn;
        [SerializeField] private Button bottomsBtn;
        [SerializeField] private Button glovesBtn;
        [SerializeField] private Button shoesBtn;
        [SerializeField] private Button accessoryBtn;
        [SerializeField] private Button specialEquipBtn;
        [SerializeField] private Button consumableBtn;
        [SerializeField] private Button ingredientBtn;

        private InventoryWindowUI inventoryWindowUI;

        public List<Button> SpringMenuBtns { get => springMenuBtns; private set => springMenuBtns = value; }

        public void Init()
        {
            inventoryWindowUI = GetComponentInParent<InventoryWindowUI>();

            selectText = GetComponentInChildren<TMP_Text>();

            weaponBtn = UtilHelper.Find<Button>(transform, "Weapon");
            if (weaponBtn != null)
            {
                springMenuBtns.Add(weaponBtn);
                weaponBtn.onClick.AddListener(
                   () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.WeaponList, true));
                weaponBtn.onClick.AddListener(() => ChangeSpringButtonColor(weaponBtn));
            }
               
            topsBtn = UtilHelper.Find<Button>(transform, "Tops");
            if (topsBtn != null)
            {
                springMenuBtns.Add(topsBtn);
                topsBtn.onClick.AddListener(
                   () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.TopsList, true));
                topsBtn.onClick.AddListener(() => ChangeSpringButtonColor(topsBtn));
            }
               
            bottomsBtn = UtilHelper.Find<Button>(transform, "Bottoms");
            if (bottomsBtn != null)
            {
                springMenuBtns.Add(bottomsBtn);
                bottomsBtn.onClick.AddListener(
                      () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.BottomsList, true));
                bottomsBtn.onClick.AddListener(() => ChangeSpringButtonColor(bottomsBtn));
            }
                

            glovesBtn = UtilHelper.Find<Button>(transform, "Gloves");
            if (glovesBtn != null)
            {
                springMenuBtns.Add(glovesBtn);
                glovesBtn.onClick.AddListener(
                    () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.GlovesList, true));
                glovesBtn.onClick.AddListener(() => ChangeSpringButtonColor(glovesBtn));
            }

            

            shoesBtn = UtilHelper.Find<Button>(transform, "Shoes");
            if (shoesBtn != null)
            {
                springMenuBtns.Add(shoesBtn);
                shoesBtn.onClick.AddListener(
                   () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.ShoesList, true));
                shoesBtn.onClick.AddListener(() => ChangeSpringButtonColor(shoesBtn));
            }
           

            accessoryBtn = UtilHelper.Find<Button>(transform, "Accessory");
            if (accessoryBtn != null)
            {
                springMenuBtns.Add(accessoryBtn);
                accessoryBtn.onClick.AddListener(
                    () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.AccessoryList, true));
                accessoryBtn.onClick.AddListener(() => ChangeSpringButtonColor(accessoryBtn));
            }
                

            specialEquipBtn = UtilHelper.Find<Button>(transform, "SpecialEquip");
            if (specialEquipBtn != null)
            {
                springMenuBtns.Add(specialEquipBtn);
                specialEquipBtn.onClick.AddListener(
                       () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.SpecialEquipList, true));
                specialEquipBtn.onClick.AddListener(() => ChangeSpringButtonColor(specialEquipBtn));
            }
               

            consumableBtn = UtilHelper.Find<Button>(transform, "Consumable");
            if (consumableBtn != null)
            {
                springMenuBtns.Add(consumableBtn);
                consumableBtn.onClick.AddListener(
                     () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.ConsumableList, true));
                consumableBtn.onClick.AddListener(() => ChangeSpringButtonColor(consumableBtn));
            }

            ingredientBtn = UtilHelper.Find<Button>(transform, "Ingredient");
            if (ingredientBtn != null)
            {
                springMenuBtns.Add(ingredientBtn);
                ingredientBtn.onClick.AddListener(
                    () => inventoryWindowUI.mainContents.SetActiveContentList(inventoryWindowUI.mainContents.IngredientList, true));
                ingredientBtn.onClick.AddListener(() => ChangeSpringButtonColor(ingredientBtn));
            }          
        }


        private void OnClickChangeSpringMenu()
        {

        }

        private void ChangeSpringMenuText()
        {
            
        }
        public void ChangeSpringButtonColor(Button button)
        {  
            for (int i = 0; i < springMenuBtns.Count; i++)
            {
                springMenuBtns[i].GetComponent<Image>().color = Color.white;
            }
            button.GetComponent<Image>().color = Color.cyan;
        }
    }
}
