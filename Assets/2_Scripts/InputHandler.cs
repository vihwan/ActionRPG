﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        private float horizontal;
        private float vertical;
        private float moveAmount;
        private float mouseX;
        private float mouseY;

        public bool a_Input; //상호작용 : 키보드 F
        public bool b_Input;
        public bool rb_Input; //약공격 : 키보드 E / 마우스 왼클릭
        public bool rt_Input; //강공격 : 키보드 R
        public bool jump_Input;
        public bool menu_Input; //메뉴 버튼창 열기 : ESC
        public bool lockOn_Input; //타겟 조준 : 마우스 휠 클릭
        public bool right_Stick_Right_Input; // 타겟 오른쪽으로 변경 : E
        public bool right_Stick_Left_Input; // 타겟 왼쪽으로 변경 : Q


        // 스킬 공격 : 키보드 버튼 1,2,3,4
        public bool sk_One_Input; 
        public bool sk_Two_Input;
        public bool sk_Three_Input;
        public bool sk_Ult_Input;

        public float rollInputTimer;
        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool menuFlag;
        public bool lockOnFlag;

        private PlayerControls inputActions;
        private PlayerAttackAnimation playerAttacker;
        private PlayerInventory playerInventory;
        private PlayerManager playerManager;
        private PlayerSkillManager playerSkillManager;
        private GUIManager guiManager;
        private CameraHandler cameraHandler;


        Vector2 movementInput;
        Vector2 cameraInput;


        #region Property
        public float MoveAmount { get => moveAmount; private set => moveAmount = value; }
        public float Horizontal { get => horizontal; private set => horizontal = value; }
        public float Vertical { get => vertical; private set => vertical = value; }
        public float MouseX { get => mouseX; private set => mouseX = value; }
        public float MouseY { get => mouseY; private set => mouseY = value; }
        #endregion

        private void Start()
        {
            playerAttacker = GetComponent<PlayerAttackAnimation>();
            if (playerAttacker == null)
                Debug.LogWarning("playerAttacker Component is Null");

            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerSkillManager = GetComponent<PlayerSkillManager>();
            guiManager = FindObjectOfType<GUIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }


        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.Skill_One.performed += i => sk_One_Input = true;
                inputActions.PlayerActions.Skill_Two.performed += i => sk_Two_Input = true;
                inputActions.PlayerActions.Skill_Three.performed += i => sk_Three_Input = true;
                inputActions.PlayerActions.Skill_Ult.performed += i => sk_Ult_Input = true;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Menu.performed += i => menu_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleSkillAttackInput(delta);
            HandleInteractingInput();
            HandleJumpingInput();
            HandleMenuInput();
            HandleLockOnInput();
        }

        private void HandleMoveInput(float delta)
        {
            Horizontal = movementInput.x;
            Vertical = movementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Horizontal) + Mathf.Abs(Vertical));
            MouseX = cameraInput.x;
            MouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            sprintFlag = b_Input;

            if (b_Input)
            {
                //rollFlag = true;
                rollInputTimer += delta;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            
            // inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }

            /*            if(rt_Input)
                        {
                            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                        }*/
        }

        private void HandleQuickSlotInput()
        {
            if (rt_Input)
            {
                playerInventory.ChangeRightWeapon();
            }
        }

        private void HandleSkillAttackInput(float delta)
        {
            if (sk_One_Input)
            {

                //콤보 도중에도 스킬을 쓸 수 있게 하는 것이 좋겠지?
                //if (playerManager.isInteracting)
                //    return;
                playerSkillManager.UseSkill(1);       
            }
            else if (sk_Two_Input)
            {
                playerSkillManager.UseSkill(2);
            }
            else if (sk_Three_Input)
            {
                playerSkillManager.UseSkill(3);
            }
            else if (sk_Ult_Input)
            {
                playerSkillManager.UseSkill(4);
            }
        }

        private void HandleInteractingInput()
        {
            inputActions.PlayerActions.A.performed += i => a_Input = true;

            //Set the UI to the Interactable Object's Text
            //Set the Text Popup to true
            if (a_Input)
            {
                if(playerManager.InteractableObject != null)
                    playerManager.ExecuteInteract();
                //interactableObject.Interact(this);
            }
        }

        private void HandleJumpingInput()
        {
        }

        private void HandleMenuInput()
        {
            if (menu_Input)
            {
                menuFlag = !menuFlag;
                if (menuFlag)
                {
                    guiManager.OpenSelectMenuWindow();
                    guiManager.UpdateUI();
                    guiManager.SetActiveHudWindows(false);
                }
                else
                {
                    guiManager.CloseSelectMenuWindow();
                    guiManager.CloseEquipmentWindowPanel();
                    guiManager.SetActiveHudWindows(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            //마우스 휠클릭 - 락온 , 오프
            if (lockOn_Input && !lockOnFlag)
            {
                lockOn_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if(lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget();
            }

            //대상 바꾸기 Q,E
            if(lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }
    }
}