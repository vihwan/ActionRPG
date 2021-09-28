using System;
using System.Collections;
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
        public bool rb_Input; //약공격 : 마우스 왼클릭
        public bool rt_Input; //강공격 : 키보드 R
        public bool lt_Input; //막기 : 마우스 우클릭
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

        //소비 아이템 사용 : C
        public bool consume_Input;

        public float rollInputTimer;
        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool menuFlag;
        public bool lockOnFlag;
        public bool counterFlag;

        private PlayerControls inputActions;
        private PlayerAttackAnimation playerAttacker;
        private PlayerManager playerManager;
        private PlayerSkillManager playerSkillManager;
        private PlayerStats playerStats;
        private CameraHandler cameraHandler;
        private ActiveWeaponObject activeWeaponObject;


        internal Vector2 movementInput;
        Vector2 cameraInput;


        #region Property
        public float MoveAmount { get => moveAmount; private set => moveAmount = value; }
        public float Horizontal { get => horizontal; private set => horizontal = value; }
        public float Vertical { get => vertical; private set => vertical = value; }
        public float MouseX { get => mouseX; private set => mouseX = value; }
        public float MouseY { get => mouseY; private set => mouseY = value; }
        #endregion

        public void Init()
        {
            playerAttacker = GetComponentInChildren<PlayerAttackAnimation>();
            if (playerAttacker == null)
                Debug.LogWarning("playerAttacker Component is Null");

            playerManager = GetComponent<PlayerManager>();
            playerSkillManager = GetComponent<PlayerSkillManager>();
            playerStats = GetComponent<PlayerStats>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            activeWeaponObject = GetComponentInChildren<ActiveWeaponObject>();
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
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerActions.Skill_One.performed += i => sk_One_Input = true;
                inputActions.PlayerActions.Skill_Two.performed += i => sk_Two_Input = true;
                inputActions.PlayerActions.Skill_Three.performed += i => sk_Three_Input = true;
                inputActions.PlayerActions.Skill_Ult.performed += i => sk_Ult_Input = true;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Menu.performed += i => menu_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerActions.ConsumeItem.performed += i => consume_Input = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void StopMovement()
        {
            Horizontal = 0f;
            Vertical = 0f;
            MoveAmount = 0f;
            MouseX = 0f;
            MouseY = 0f;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        public void TickInput(float delta)
        {

            if (GUIManager.instance.dialogObject.activeSelf.Equals(true))
                return;

            HandleMenuInput();

            if (GUIManager.instance.IsActiveUIWindows())
            {
                StopMovement();
                return;
            }

            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotInput();
            HandleSkillAttackInput(delta);
            HandleInteractingInput();
            HandleJumpingInput();
            HandleLockOnInput();
            HandleConsumeItemInput();
        }

        private void HandleConsumeItemInput()
        {
            if (consume_Input)
            {
                //아이템 소비
                playerSkillManager.UseConsumeItem();
            }
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
                    if (playerStats.CurrentStamina >= 15 && !playerManager.isInteracting)
                    {
                        sprintFlag = false;
                        rollFlag = true;
                        playerStats.UseStamina(15);
                    }
                }
                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            if (rb_Input)
            {
                ChangePlayerMotionToEquip();

                if(counterFlag.Equals(true))
                {
                    Debug.Log("counterFlag is true");
                    playerAttacker.HandleCounterAttack(PlayerInventory.Instance.currentWeapon);
                    counterFlag = false;
                }

                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(PlayerInventory.Instance.currentWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    playerAttacker.HandleLightAttack(PlayerInventory.Instance.currentWeapon);
                }
            }

            //강공격
            //if (rt_Input)
            //{
            //    playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            //}

            //막기
            if(lt_Input)
            {
                //막기 자세를 취함
                ChangePlayerMotionToEquip();
                if(playerManager.isInteracting)
                    return;

                playerAttacker.HandleGuard(PlayerInventory.Instance.currentWeapon);
                
            }
        }

        private void HandleQuickSlotInput()
        {
            /* if (rt_Input)
             {
                 playerInventory.ChangeRightWeapon();
             }*/
        }

        private void HandleSkillAttackInput(float delta)
        {
            if (sk_One_Input)
            {
                ChangePlayerMotionToEquip();
                playerSkillManager.UseSkill(1);
            }
            else if (sk_Two_Input)
            {
                ChangePlayerMotionToEquip();
                playerSkillManager.UseSkill(2);
            }
            else if (sk_Three_Input)
            {
                ChangePlayerMotionToEquip();
                playerSkillManager.UseSkill(3);
            }
            else if (sk_Ult_Input)
            {
                ChangePlayerMotionToEquip();
                playerSkillManager.UseSkill(4);
            }
        }

        private void HandleInteractingInput()
        {
            inputActions.PlayerActions.A.canceled += i => a_Input = true;

            //Set the UI to the Interactable Object's Text
            //Set the Text Popup to true
            if (a_Input)
            {
                if (playerManager.InteractableObject != null)
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
                if (GUIManager.instance.windowPanel.characterWindowUI.gameObject.activeSelf.Equals(true))
                    GetComponent<AnimationLayerHandler>().SetAnimaionLayerWeightCloseCharacterPanel();

                HandleMenuFlag();
            }
        }

        public void HandleMenuFlag()
        {
            menuFlag = !menuFlag;
            GUIManager.instance.SetActiveGUIMenu(menuFlag);
        }

        private void HandleLockOnInput()
        {
            //마우스 휠클릭 - 락온 , 오프
            if (lockOn_Input && !lockOnFlag)
            {
                lockOn_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTarget();
            }

            //대상 바꾸기 Q,E
            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void ChangePlayerMotionToEquip()
        {
            playerManager.isUnEquip = false;
            playerManager.AnimationLayerHandler.HandlePlayerEquip();
            activeWeaponObject.SetActiveHandWeapon(true);
        }
    }
}
