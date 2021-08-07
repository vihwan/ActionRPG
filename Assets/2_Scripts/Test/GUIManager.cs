using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private QuickSlotUI quickSlotUI;
        [SerializeField] private InteractableUI interactableUI;


        // Start is called before the first frame update
        void Awake()
        {
            healthBar = GetComponentInChildren<HealthBar>(true);
            if (healthBar != null)
                healthBar.Init();

            quickSlotUI = GetComponentInChildren<QuickSlotUI>(true);
            if(quickSlotUI != null)
                quickSlotUI.Init();

            interactableUI = GetComponentInChildren<InteractableUI>(true);
            if (interactableUI != null)
                interactableUI.Init();
        }
    }
}

