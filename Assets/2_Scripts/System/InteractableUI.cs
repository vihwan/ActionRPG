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
        [SerializeField] private GameObject interactionPopup;
        [SerializeField] private Image interactObjectImage;
        public TMP_Text InteractText { get => interactText; }
        public GameObject InteractionPopup { get => interactionPopup; }
        public Image InteractObjectImage { get => interactObjectImage; set => interactObjectImage = value; }

        public void Init()
        {
            interactionPopup = transform.Find("Interaction Popup").gameObject;
            if (InteractionPopup != null)
            {
                InteractionPopup.SetActive(false);
            }
            interactText = GetComponentInChildren<TMP_Text>(true);

            InteractObjectImage = UtilHelper.Find<Image>(interactionPopup.transform, "Image");
            if (interactObjectImage != null)
                interactObjectImage.preserveAspect = true;
        }

        public void SetActiveInteractUI(bool status)
        {
            interactionPopup.SetActive(status);
        }
    }
}
