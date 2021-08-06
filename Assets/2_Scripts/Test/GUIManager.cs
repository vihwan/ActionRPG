using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private QuickSlotUI quickSlotUI;


        // Start is called before the first frame update
        void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
                healthBar.Init();

            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            if (quickSlotUI != null)
                quickSlotUI.Init();
        }
    }
}

