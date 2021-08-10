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
        private Interactable interactableObject;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        public InteractableUI InteractableUI { get => interactableUI; }
        public Interactable InteractableObject { get => interactableObject; }

        private void Awake()
        {

        }

        private void Start()
        {
            if (CameraHandler.Instance != null)
                cameraHandler = CameraHandler.Instance;

            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isInAir", isInAir);

            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();

            //CheckForInteractable();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
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

            isSprinting = inputHandler.b_Input;

            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
                //playerLocomotion.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }

            /*            if (isGrounded)
                        {
                            playerLocomotion.Rigidbody.constraints = 
                                RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                        }*/
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

        public void ExecuteInteract()
        {
            this.interactableObject.Interact(this);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Interactable")
            {
                interactableObject = other.GetComponent<Interactable>();
                if (InteractableObject.canInteract)
                {
                    string interactText = InteractableObject.interactableText;
                    interactableUI.InteractText.text = interactText;
                    interactableUI.InteractionBG.SetActive(true);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Interactable")
            {
                if (interactableUI.InteractionBG != null)
                    interactableUI.InteractionBG.SetActive(false);
            }
        }
    }
}

