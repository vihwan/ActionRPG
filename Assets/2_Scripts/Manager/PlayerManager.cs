using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    public class PlayerManager : MonoBehaviourSingleton<PlayerManager>
    {
        [Header("Basics")]
        public readonly string playerName = "Diluc Ragnvindr";

        private InputHandler inputHandler;
        private Animator anim;
        private CameraHandler cameraHandler;
        private PlayerLocomotion playerLocomotion;
        private PlayerStats playerStats;
        private PlayerInventory playerInventory;
        private PlayerSkillManager playerSkillManager;
        private PlayerQuestInventory playerQuestInventory;
        private InteractableUI interactableUI;
        [SerializeField] private Interactable interactableObject;
        private AnimationLayerHandler animationLayerHandler;
        private PlayerAnimatorHandler playerAnimatorHandler;
        private ActiveWeaponObject activeWeaponObject;
        private WeaponPivoting weaponPivoting;

        public bool isInteracting;

        [Header("Player Flags")]
        public bool isUnEquip;
        public bool isSprinting;
        public bool isRolling; //문제..
        public bool isInAir;

        public bool canDoCombo;
        public bool isInvulnerable;
        public bool isDamaged;
        public bool isBlocking;
        public bool canCounter;
        public bool isFalldown;

        public float changeWeaponOutWaitTime = 0f;
        public float counterAttackTime = 0f;
        [SerializeField] private const float changeWeaponOutLimitTime = 5f;
        [SerializeField] private const float ableCounterAttackLimitTime = 1f;
        [SerializeField] internal Transform lockOnTransform;

        public InteractableUI InteractableUI { get => interactableUI; }
        public Interactable InteractableObject { get => interactableObject; }
        public AnimationLayerHandler AnimationLayerHandler { get => animationLayerHandler; }
        public WeaponPivoting WeaponPivoting { get => weaponPivoting; set => weaponPivoting = value; }

        public void Start()
        {
            if (CameraHandler.Instance != null)
                cameraHandler = CameraHandler.Instance;
        }

        public void Init()
        {
            inputHandler = GetComponent<InputHandler>();
            if (inputHandler != null)
                inputHandler.Init();

            playerLocomotion = GetComponent<PlayerLocomotion>();
            if (playerLocomotion != null)
                playerLocomotion.Init();

            playerInventory = GetComponent<PlayerInventory>();
            if (playerInventory != null)
                playerInventory.Init();

            playerStats = GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.Init();

            playerSkillManager = GetComponent<PlayerSkillManager>();
            if (playerSkillManager != null)
                playerSkillManager.Init();

            playerQuestInventory = GetComponent<PlayerQuestInventory>();
            if (playerQuestInventory != null)
                playerQuestInventory.Init();

            anim = GetComponentInChildren<Animator>();
            playerAnimatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();

            interactableUI = FindObjectOfType<InteractableUI>();

            animationLayerHandler = GetComponent<AnimationLayerHandler>();
            if (animationLayerHandler != null)
                animationLayerHandler.Init();

            activeWeaponObject = GetComponentInChildren<ActiveWeaponObject>();
            WeaponPivoting = GetComponentInChildren<WeaponPivoting>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;      
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            canCounter = anim.GetBool("canCounter");
            anim.SetBool("isInAir", isInAir);
            isUnEquip = anim.GetBool("isUnEquip");
            isInvulnerable = anim.GetBool("isInvulnerable");
            isDamaged = anim.GetBool("isDamaged");
            isBlocking = anim.GetBool("isBlocking");
            isFalldown = anim.GetBool("isFalldown");
            isRolling = anim.GetBool("isRolling");  //문제..

            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerStats.RegenerationStamina();
        }

        private void CheckCounterTimer(float delta)
        {
            if (inputHandler.counterFlag.Equals(true))
            {
                counterAttackTime += delta;

                if (counterAttackTime >= ableCounterAttackLimitTime)
                {
                    counterAttackTime = 0f;
                    inputHandler.counterFlag = false;
                    Debug.Log("카운터 공격 가능 시간 초과");
                }
            }
            else
            {
                counterAttackTime = 0f;
                return;
            }
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerAnimatorHandler.canRotate = anim.GetBool("canRotate");
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);
            CheckCounterTimer(delta);;
            // playerLocomotion.HandleSprintEnd();
        }


        private void LateUpdate()
        {
            float delta = Time.deltaTime;
            if (!isUnEquip && GUIManager.it.windowPanel.characterWindowUI.gameObject.activeSelf.Equals(false))
                ChangePlayerToUnEquip(delta);

            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            //inputHandler.lt_Input = false;
            inputHandler.sk_One_Input = false;
            inputHandler.sk_Two_Input = false;
            inputHandler.sk_Three_Input = false;
            inputHandler.sk_Ult_Input = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.menu_Input = false;
            inputHandler.consume_Input = false;
            inputHandler.rollFlag = false;
            isSprinting = inputHandler.b_Input;
            
            if (GUIManager.it.windowPanel.characterWindowUI.gameObject.activeSelf.Equals(true))
            {

                cameraHandler.HandleCharacterWindowCameraPosition(delta);
                cameraHandler.Zoom_OnActiveCharacterWindowUI();
                cameraHandler.DragCharacter_OnActiveCharacterWindowUI();
                return;
            }
            else
            {
                cameraHandler.isUpdate = false;
                cameraHandler.zoomLevel = 0;
            }


            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }

            anim.SetBool("isDamaged", false);
            playerLocomotion.isJumping = anim.GetBool("isJumping");
            CheckWeaponPivoting();
        }

        private void CheckWeaponPivoting()
        {
            if(isBlocking) WeaponPivoting.GuardPivoting();
            else WeaponPivoting.NormalPivoting();
        }

        public void EnableInputHandler()
        {
            inputHandler.enabled = false;
        }

        private void ChangePlayerToUnEquip(float delta)
        {
            changeWeaponOutWaitTime += delta;

            if(isFalldown || isDamaged)
            {   //데미지를 입은 상태 혹은 다운되어있는 상태라면 전투 대기시간을 계속 초기화
                changeWeaponOutWaitTime = 0f;
            }

            //구르기, 점프, 공격, 스킬 입력키, 막기가 입력되면(isInteracting == true 대기경과시간을 초기화)
            if (this.isInteracting ||
                inputHandler.rb_Input || inputHandler.jump_Input ||
                inputHandler.sk_One_Input || inputHandler.sk_Two_Input || inputHandler.sk_Three_Input || inputHandler.sk_Ult_Input ||
                inputHandler.lt_Input)
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

        internal void OpenInteractionToVelocityZero()
        {
            //플레이어가 미끄러지는 것을 막는다.
            playerLocomotion.rigidBody.velocity = Vector3.zero;
        }

        public void SetLevelSystem()
        {
            LevelManager.it.OnLevelChanged += LevelManager_OnLevelChanged;
        }

        private void LevelManager_OnLevelChanged(object sender, EventArgs e)
        {
            PlayLevelUpParticleEffect();
            playerStats.SetStatusByLevelUp();
            playerStats.SetMaxStatusBar();
        }

        private void PlayLevelUpParticleEffect()
        {
            GameObject go = Instantiate(Database.it.PrefabDataBase.levelUpParticlePrefab,
                        this.gameObject.transform.position,
                        Quaternion.identity,
                        this.transform) as GameObject;
            go.transform.localScale = new Vector3(.7f, .7f, .7f);
            go.GetComponent<ParticleSystem>().Play();
            Destroy(go, 5f);
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

                if (interactableObject != null)
                {
                    Interactable interactable = other.GetComponent<Interactable>();
                    if (interactable == interactableObject)
                        interactableObject = null;
                }
            }
        }

        public void SetAllMonsterCurrentTargetToNull()
        {
            EnemyManager[] enemyManagers = GameObject.FindObjectsOfType<EnemyManager>();
            for (int i = 0; i < enemyManagers.Length; i++)
            {
                enemyManagers[i].currentTarget = null;
            }
        }

        public void ChangePlayerMotionToEquip()
        {
            AnimationLayerHandler.HandlePlayerEquip();
            activeWeaponObject.SetActiveHandWeapon(true);
        }
    }
}

