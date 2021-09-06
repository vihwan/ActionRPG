using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerManager : CharacterManager
    {
        private InputHandler inputHandler;
        private Animator anim;
        private CameraHandler cameraHandler;
        private PlayerLocomotion playerLocomotion;
        private InteractableUI interactableUI;
        [SerializeField] private Interactable interactableObject;
        private AnimationLayerHandler animationLayerHandler;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isUnEquip;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        public float changeWeaponOutWaitTime = 0f;
        [SerializeField] private const float changeWeaponOutLimitTime = 5f;
        public InteractableUI InteractableUI { get => interactableUI; }
        public Interactable InteractableObject { get => interactableObject; }
        public AnimationLayerHandler AnimationLayerHandler { get => animationLayerHandler; }

        private void Start()
        {
            if (CameraHandler.Instance != null)
                cameraHandler = CameraHandler.Instance;

            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
            animationLayerHandler = GetComponent<AnimationLayerHandler>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);
            isUnEquip = anim.GetBool("isUnEquip");

            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();

            //CheckForInteractable();
            CheckOpenUI();
        }

        private void CheckOpenUI()
        {
/*            if (GUIManager.instance.IsActiveUIWindows())
            {
                inputHandler.enabled = false;
            }
            else
            {
                inputHandler.enabled = true;
            }*/
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
           // playerLocomotion.HandleSprintEnd();
        }

        private void LateUpdate()
        {
            float delta = Time.deltaTime;
            if (!isUnEquip)
                ChangePlayerToUnEquip(delta);

            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.sk_One_Input = false;
            inputHandler.sk_Two_Input = false;
            inputHandler.sk_Three_Input = false;
            inputHandler.sk_Ult_Input = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.menu_Input = false;
            inputHandler.consume_Input = false;

            isSprinting = inputHandler.b_Input;

           
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        public void EnableInputHandler()
        {
            inputHandler.enabled = false;
        }


        //public void CheckForInteractable()
        //{
        //    RaycastHit hit;
        //    if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
        //    {
        //        if (hit.collider.tag == "Interactable")
        //        {
        //            Interactable interactableObject = hit.collider.GetComponent<Interactable>();
        //            if (interactableObject != null)
        //            {
        //                string interactText = interactableObject.interactableText;
        //                InteractableUI.InteractText.text = interactText;
        //                InteractableUI.SetActiveInteractUI(true);

        //                //Set the UI to the Interactable Object's Text
        //                //Set the Text Popup to true
        //                if (inputHandler.a_Input)
        //                {
        //                    hit.collider.GetComponent<Interactable>().Interact(this);
        //                    //interactableObject.Interact(this);
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        if (InteractableUI.InteractionBG != null)
        //            InteractableUI.SetActiveInteractUI(false);
        //    }
        //}

        private void ChangePlayerToUnEquip(float delta)
        {
            changeWeaponOutWaitTime += delta;

            //구르기, 점프, 공격, 스킬 입력키가 입력되면(isInteracting == true 대기경과시간을 초기화)
            if (this.isInteracting ||
                inputHandler.rb_Input || inputHandler.jump_Input ||
                inputHandler.sk_One_Input || inputHandler.sk_Two_Input || inputHandler.sk_Three_Input || inputHandler.sk_Ult_Input)
            {
                changeWeaponOutWaitTime = 0f;
            }

            if (changeWeaponOutWaitTime >= changeWeaponOutLimitTime)
            {
                AnimationLayerHandler.HandlePlayerUnEquip();
                changeWeaponOutWaitTime = 0f;
            }
        }

        public void ExecuteInteract()
        {
            this.interactableObject.Interact(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Interactable"))
            {
                interactableObject = other.GetComponent<Interactable>();
                if (InteractableObject.canInteract)
                {
                    string interactText = InteractableObject.interactableText;
                    interactableUI.InteractText.text = interactText;
                    interactableUI.InteractObjectImage.sprite = InteractableObject.interactIcon;
                    interactableUI.InteractionPopup.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Interactable"))
            {
                if (interactableUI.InteractionPopup != null)
                    interactableUI.InteractionPopup.SetActive(false);
        
                if(interactableObject != null)
                {
                    Interactable interactable = other.GetComponent<Interactable>();
                    if (interactable == interactableObject)
                        interactableObject = null;
                }
            }
        }
    }
}

