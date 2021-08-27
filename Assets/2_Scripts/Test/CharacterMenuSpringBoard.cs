using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class CharacterMenuSpringBoard : MonoBehaviour
    {
        [SerializeField] private Button statusBtn;
        [SerializeField] private Button weaponBtn;
        [SerializeField] private Button equipBtn;
        [SerializeField] private Button skillBtn;
        [SerializeField] private Button dataBtn;

        private CharacterWindowUI characterWindow;

        public void Init()
        {
            characterWindow = GetComponentInParent<CharacterWindowUI>();

            statusBtn = UtilHelper.Find<Button>(transform, "Status");
            if (statusBtn != null)
                statusBtn.onClick.AddListener(characterWindow.OpenStatusPanel);

            weaponBtn = UtilHelper.Find<Button>(transform, "Weapon");
            if (weaponBtn != null)
                weaponBtn.onClick.AddListener(characterWindow.OpenWeaponPanel);

            equipBtn = UtilHelper.Find<Button>(transform, "Equipment");
            if (equipBtn != null)
                equipBtn.onClick.AddListener(characterWindow.OpenEquipmentPanel);

            skillBtn = UtilHelper.Find<Button>(transform, "Skill");
            if (skillBtn != null)
                skillBtn.onClick.AddListener(characterWindow.OpenSkillPanel);

            dataBtn = UtilHelper.Find<Button>(transform, "Data");
            if (dataBtn != null)
                dataBtn.onClick.AddListener(characterWindow.OpenDataPanel);

            dataBtn = UtilHelper.Find<Button>(transform, "Data");
        }
    }
}

