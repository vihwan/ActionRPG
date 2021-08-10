using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class InteractableUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text interactText;
        [SerializeField] private GameObject interactionBG;
        [SerializeField] private RawImage interactObjectImage;


        public TMP_Text InteractText { get => interactText; }
        public GameObject InteractionBG { get => interactionBG; }

        public void Init()
        {
            interactionBG = transform.Find("Interaction Popup").gameObject;
            if (InteractionBG != null)
            {
                InteractionBG.SetActive(false);
            }
            interactText = GetComponentInChildren<TMP_Text>(true);
            interactObjectImage = GetComponentInChildren<RawImage>(true);
        }

        public void SetActiveInteractUI(bool status)
        {
            interactionBG.SetActive(status);
        }
    }
}
