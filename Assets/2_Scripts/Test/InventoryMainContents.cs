using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class InventoryMainContents : MonoBehaviour
    {
        [Header("Lists")]
        [SerializeField] private List<InventoryMain_ContentList> contentLists;
        private InventoryMain_ContentList weaponList;
        private InventoryMain_ContentList topsList;
        private InventoryMain_ContentList bottomsList;
        private InventoryMain_ContentList glovesList;
        private InventoryMain_ContentList shoesList;
        private InventoryMain_ContentList accessoryList;
        private InventoryMain_ContentList specialEquipList;
        private InventoryMain_ContentList consumableList;
        private InventoryMain_ContentList ingredientList;


        [Header("Info")]

        [Header("Need Component"), HideInInspector]
        internal PlayerInventory playerInventory;
        public void Init()
        {
            weaponList = UtilHelper.Find<InventoryMain_ContentList>(transform, "WeaponList");
            if (weaponList != null)
                contentLists.Add(weaponList);

            topsList = UtilHelper.Find<InventoryMain_ContentList>(transform, "TopsList");
            if (topsList != null)
                contentLists.Add(topsList);

            bottomsList = UtilHelper.Find<InventoryMain_ContentList>(transform, "BottomsList");
            if (bottomsList != null)
                contentLists.Add(bottomsList);

            glovesList = UtilHelper.Find<InventoryMain_ContentList>(transform, "GlovesList");
            if (glovesList != null)
                contentLists.Add(glovesList);

            weaponList = UtilHelper.Find<InventoryMain_ContentList>(transform, "WeaponList");
            if (weaponList != null)
                contentLists.Add(weaponList);

            weaponList = UtilHelper.Find<InventoryMain_ContentList>(transform, "WeaponList");
            if (weaponList != null)
                contentLists.Add(weaponList);

            weaponList = UtilHelper.Find<InventoryMain_ContentList>(transform, "WeaponList");
            if (weaponList != null)
                contentLists.Add(weaponList);

            weaponList = UtilHelper.Find<InventoryMain_ContentList>(transform, "WeaponList");
            if (weaponList != null)
                contentLists.Add(weaponList);

            /*            contentLists = GetComponentsInChildren<InventoryMain_ContentList>(true);
                        if(contentLists != null)
                        {
                            for (int i = 0; i < contentLists.Length; i++)
                            {
                                contentLists[i].Init();
                            }
                        }*/

            playerInventory = FindObjectOfType<PlayerInventory>();
        }
    }
}