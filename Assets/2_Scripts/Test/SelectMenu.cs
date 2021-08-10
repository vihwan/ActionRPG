using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG { 
    public class SelectMenu : MonoBehaviour
    {
        [SerializeField] private Button equipButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button optionButton;

        private GUIManager guiManager;

        // Start is called before the first frame update
        public void Init()
        {
            guiManager = GetComponentInParent<GUIManager>();
            equipButton = UtilHelper.Find<Button>(transform, "Select Equipment");
            if (equipButton != null)
            {
                equipButton.onClick.AddListener(guiManager.OpenEquipmentWindowPanel);
                equipButton.onClick.AddListener(guiManager.CloseSelectMenuWindow);
            }
  

            inventoryButton = UtilHelper.Find<Button>(transform, "Select Inventory");
            optionButton = UtilHelper.Find<Button>(transform, "Select GameOptions");
        }
    }
}

