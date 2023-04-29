using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnforceMenuSpringBoard : MonoBehaviour
    {
        [Header("Button Menu")]
        [SerializeField] private List<Button> springMenuBtns;
        [SerializeField] internal Button weaponBtn;
        [SerializeField] private Button topsBtn;
        [SerializeField] private Button bottomsBtn;
        [SerializeField] private Button glovesBtn;
        [SerializeField] private Button shoesBtn;
        [SerializeField] private Button accessoryBtn;
        [SerializeField] private Button specialEquipBtn;

        private EnforceWindowUI enforceWindowUI;
        public void Init()
        {
            enforceWindowUI = GetComponentInParent<EnforceWindowUI>();

            weaponBtn = UtilHelper.Find<Button>(transform, "Weapon");
            if (weaponBtn != null)
            {
                springMenuBtns.Add(weaponBtn);
                weaponBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.Weapon));
                weaponBtn.onClick.AddListener(() => ChangeSpringButtonColor(weaponBtn));
            }

            topsBtn = UtilHelper.Find<Button>(transform, "Tops");
            if (topsBtn != null)
            {
                springMenuBtns.Add(topsBtn);
                topsBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.Tops));
                topsBtn.onClick.AddListener(() => ChangeSpringButtonColor(topsBtn));
            }

            bottomsBtn = UtilHelper.Find<Button>(transform, "Bottoms");
            if (bottomsBtn != null)
            {
                springMenuBtns.Add(bottomsBtn);
                bottomsBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.Bottoms));
                bottomsBtn.onClick.AddListener(() => ChangeSpringButtonColor(bottomsBtn));
            }

            glovesBtn = UtilHelper.Find<Button>(transform, "Gloves");
            if (glovesBtn != null)
            {
                springMenuBtns.Add(glovesBtn);
                glovesBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.Gloves));
                glovesBtn.onClick.AddListener(() => ChangeSpringButtonColor(glovesBtn));
            }

            shoesBtn = UtilHelper.Find<Button>(transform, "Shoes");
            if (shoesBtn != null)
            {
                springMenuBtns.Add(shoesBtn);
                shoesBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.Shoes));
                shoesBtn.onClick.AddListener(() => ChangeSpringButtonColor(shoesBtn));
            }

            accessoryBtn = UtilHelper.Find<Button>(transform, "Accessory");
            if (accessoryBtn != null)
            {
                springMenuBtns.Add(accessoryBtn);
                accessoryBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.Accessory));
                accessoryBtn.onClick.AddListener(() => ChangeSpringButtonColor(accessoryBtn));
            }

            specialEquipBtn = UtilHelper.Find<Button>(transform, "SpecialEquip");
            if (specialEquipBtn != null)
            {
                springMenuBtns.Add(specialEquipBtn);
                specialEquipBtn.onClick.AddListener(() => enforceWindowUI.SetActiveTrueLeftInventory(ItemType.SpecialEquip));
                specialEquipBtn.onClick.AddListener(() => ChangeSpringButtonColor(specialEquipBtn));
            }
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
