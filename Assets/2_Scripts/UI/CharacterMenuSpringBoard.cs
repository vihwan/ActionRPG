using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class CharacterMenuSpringBoard : MonoBehaviour
    {
        [SerializeField] internal Button statusBtn;
        [SerializeField] private Button weaponBtn;
        [SerializeField] private Button equipBtn;
        [SerializeField] private Button skillBtn;
        [SerializeField] private Button dataBtn;

        private Color alpha0 = new Color(1f, 1f, 1f, 0f);
        private Color selectColor = new Color(0f,.48f,.68f,1f);

        private CharacterWindowUI characterWindowUI;

        public void Init()
        {
            characterWindowUI = GetComponentInParent<CharacterWindowUI>();

            statusBtn = UtilHelper.Find<Button>(transform, "Status");
            if (statusBtn != null)
            {
                statusBtn.onClick.AddListener(characterWindowUI.OpenStatusPanel);
                statusBtn.onClick.AddListener(() => OnClickBtnChangeColor(statusBtn));
            }

            weaponBtn = UtilHelper.Find<Button>(transform, "Weapon");
            if (weaponBtn != null)
            {
                weaponBtn.onClick.AddListener(characterWindowUI.OpenWeaponPanel);
                weaponBtn.onClick.AddListener(() => OnClickBtnChangeColor(weaponBtn));
            }

            equipBtn = UtilHelper.Find<Button>(transform, "Equipment");
            if (equipBtn != null)
            {
                equipBtn.onClick.AddListener(characterWindowUI.OpenEquipmentPanel);
                equipBtn.onClick.AddListener(() => OnClickBtnChangeColor(equipBtn));
            }

            skillBtn = UtilHelper.Find<Button>(transform, "Skill");
            if (skillBtn != null)
            {
                skillBtn.onClick.AddListener(characterWindowUI.OpenSkillPanel);
                skillBtn.onClick.AddListener(() => OnClickBtnChangeColor(skillBtn));
            }

            dataBtn = UtilHelper.Find<Button>(transform, "Data");
            if (dataBtn != null)
            {
                dataBtn.onClick.AddListener(characterWindowUI.OpenDataPanel);
                dataBtn.onClick.AddListener(() => OnClickBtnChangeColor(dataBtn));
            }
        }

        public void OnClickBtnChangeColor(Button button)
        {
            DeSelectAllButton();
            button.GetComponent<Image>().color = selectColor;
            button.interactable = false;
        }

        private void DeSelectAllButton()
        {
            statusBtn.GetComponent<Image>().color = alpha0;
            weaponBtn.GetComponent<Image>().color = alpha0;
            equipBtn.GetComponent<Image>().color = alpha0;
            skillBtn.GetComponent<Image>().color = alpha0;
            dataBtn.GetComponent<Image>().color = alpha0;

            statusBtn.interactable = true;
            weaponBtn.interactable = true;
            equipBtn.interactable = true;
            skillBtn.interactable = true;
            dataBtn.interactable = true;
        }
    }
}

