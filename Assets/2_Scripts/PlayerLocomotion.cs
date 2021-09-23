using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObject;
        InputHandler inputHandler;
        CameraHandler cameraHandler;
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterBlockerCollider;

        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public Rigidbody rigidBody;
        // public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;


        [Header("Player Stats")]
        [SerializeField]
        float walkingSpeed = 1;
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 80;

        // Start is called before the first frame update
        public void Init()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidBody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();

            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initalize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            Physics.IgnoreCollision(characterCollider, characterBlockerCollider, true);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputHandler.Vertical;
            moveDirection += cameraObject.right * inputHandler.Horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            /*
             * 08월 05일 수정 
             * 1. 아무런 이동방향 없이 구르기 버튼을 눌렀을 경우, 실행되지 않고 오류가 발생.
             * **/
            if (inputHandler.sprintFlag && inputHandler.MoveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {
                if (inputHandler.MoveAmount < 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidBody.velocity = projectedVelocity;



            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.Vertical, inputHandler.Horizontal, playerManager.isSprinting);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.MoveAmount, 0, playerManager.isSprinting);
            }

            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        private void HandleRotation(float delta)
        {
            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                if (inputHandler.sprintFlag || inputHandler.rollFlag)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = cameraHandler.cameraTransform.forward * inputHandler.Vertical;
                    targetDirection += cameraHandler.cameraTransform.right * inputHandler.Horizontal;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    Vector3 rotationDirection = moveDirection;
                    rotationDirection = cameraHandler.currentLockOnTarget.position - transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {

                Vector3 targetDir = Vector3.zero;
                float moveOverride = inputHandler.MoveAmount;

                targetDir = cameraObject.forward * inputHandler.Vertical;
                targetDir += cameraObject.right * inputHandler.Horizontal;

                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                    targetDir = myTransform.forward;

                float rs = rotationSpeed;

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                myTransform.rotation = targetRotation;
            }

        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.Vertical;
                moveDirection += cameraObject.right * inputHandler.Horizontal;

                if (inputHandler.MoveAmount > 0)
                {
                    if (playerManager.isUnEquip == false)
                        animatorHandler.PlayTargetAnimation("Rolling", true);
                    else
                        animatorHandler.PlayTargetAnimation("Rolling_UnEquip", true);

                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                }
                else
                {
                    if (playerManager.isUnEquip == false)
                        animatorHandler.PlayTargetAnimation("Step_Back", true);
                    else
                        animatorHandler.PlayTargetAnimation("Step_Back_UnEquip", true);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidBody.AddForce(-Vector3.up * fallingSpeed);
                rigidBody.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            //레이캐스트를 이용해서 땅에 닿아있는지 상태를 체크합니다.
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in the air for : " + inAirTimer);
                        if (playerManager.isUnEquip == false)
                            animatorHandler.PlayTargetAnimation("Land", true);
                        else
                            animatorHandler.PlayTargetAnimation("Land_UnEquip", true);

                        inAirTimer = 0;
                    }
                    else
                    {
                        if (playerManager.isUnEquip == false)
                            animatorHandler.PlayTargetAnimation("Empty", false);
                        else
                            animatorHandler.PlayTargetAnimation("Empty UnEquip", false);

                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        if (playerManager.isUnEquip == false)
                            animatorHandler.PlayTargetAnimation("Falling", true);
                        else
                            animatorHandler.PlayTargetAnimation("Falling_UnEquip", true);
                    }

                    Vector3 vel = rigidBody.velocity;
                    vel.Normalize();
                    rigidBody.velocity = vel * (movementSpeed / 2f);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.MoveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, delta);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }

            if (playerManager.isInteracting || inputHandler.MoveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                myTransform.position = targetPosition;
            }


        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting)
                return;

            if (inputHandler.jump_Input)
            {

                moveDirection = cameraObject.forward * inputHandler.Vertical;
                moveDirection += cameraObject.right * inputHandler.Horizontal;

                if (playerManager.isUnEquip == false)
                    animatorHandler.PlayTargetAnimation("Jump", true);
                else
                    animatorHandler.PlayTargetAnimation("Jump_UnEquip", true);

                moveDirection.y = 0f; //Root Motion
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = jumpRotation;

                if (inputHandler.MoveAmount > 0)
                {

                }
            }
        }

        //장비 미장착에서만 동작. 전투중에 달리기 끝마치는 경우 같은 동작은 필요가 없다.
        public void HandleSprintEnd()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isUnEquip == true)
            {
                if (playerManager.isSprinting)
                {
                    if (rigidBody.velocity == Vector3.zero)
                    {
                        Debug.Log("Play SprintEnd");
                        animatorHandler.PlayTargetAnimation("SprintEnd_UnEquip", true);
                        playerManager.isSprinting = false;
                    }
                }
            }
        }
        #endregion
    }
}

