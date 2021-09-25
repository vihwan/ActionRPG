using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SG
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] public Transform targetTransform;
        [SerializeField] public Transform cameraTransform;
        [SerializeField] public Transform cameraPivotTransform;
        private Transform myTransform;
        private Transform tempTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler Instance;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35f;
        public float maximumPivot = 35f;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 1.6f;
        public float unlockedPivotPosition = 1f;



        [Header("Camera LockOn")]
        [SerializeField]
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        public Transform nearestLockOnTarget;
        public Transform currentLockOnTarget;
        public Transform leftLockTarget;
        public Transform rightLockTarget;
        public float maximumLockOnDistance = 30;

        [Header("Camera Zoom")]
        [SerializeField] internal int zoomLevel;

        private InputHandler inputHandler;
        private PlayerManager playerManager;
        internal bool isTurningCamera;
        internal bool isUpdate = false;

        public Transform TempTransform { get => tempTransform; private set => tempTransform = value; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            environmentLayer = LayerMask.NameToLayer("Environment");
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();

        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position,
                ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.lockOnFlag == false)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                Vector3 dir = currentLockOnTarget.position - transform.position;
                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }

        }

        //캐릭터가 지형 등에 의해 보이지 않을 경우, 카메라가 충돌 처리를 검사하여 카메라를 이동하게 한다.
        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position,
                cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }



        //마우스 휠 키를 클릭하여 록온한 대상을 카메라가 계속 고정시켜 보여준다.
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            availableTargets.Clear();

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (character.transform.root != targetTransform.transform.root
                        && viewableAngle > -50
                        && viewableAngle < 50
                        && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position, Color.cyan);
                            if (hit.transform.gameObject.layer == environmentLayer)
                            {
                                //cannot lockon to target, object in way
                            }
                            else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k].lockOnTransform;
                }

                //플레이어가 조준 상태일때는 왼쪽, 오른쪽 상대를 찾아 리스트에 저장합니다.
                if (inputHandler.lockOnFlag)
                {
                    Vector3 relativeEnemyPosition = currentLockOnTarget.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

                    if (relativeEnemyPosition.x > 0f && distanceFromLeftTarget < shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k].lockOnTransform;
                    }

                    if (relativeEnemyPosition.x < 0f && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k].lockOnTransform;
                    }
                }
            }
        }

        //록온한 대상을 해제하여 원래대로 돌아온다.
        public void ClearLockOnTarget()
        {
            availableTargets.Clear();
            currentLockOnTarget = null;
            nearestLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition
                    = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition
                    = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }

        public void HandleCharacterWindowCameraPosition(float delta)
        {
            if (isUpdate)
                return;

            Vector3 rotationDirection = cameraTransform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 10 * delta);
            playerManager.transform.rotation = targetRotation;

            Quaternion characterWindowUICameraRotation = Quaternion.Euler(0f, 180f, 0f);
            myTransform.transform.rotation =
                   Quaternion.Slerp(myTransform.rotation, characterWindowUICameraRotation, 10 * delta);

            Quaternion characterWindowUICameraPivotRotation = Quaternion.Euler(0f, 0f, 0f);
            cameraPivotTransform.rotation =
                   Quaternion.Slerp(cameraPivotTransform.rotation, characterWindowUICameraPivotRotation, 10 * delta);

            Invoke(nameof(IsUpdateTrue), 1f);
        }

        private void IsUpdateTrue()
        {
            isUpdate = true;
        }

        public void DragCharacter_OnActiveCharacterWindowUI()
        {
            if (GUIManager.instance.windowPanel.characterWindowUI.skillPanel.gameObject.activeSelf.Equals(true))
                return;

            if (Mouse.current.leftButton.isPressed)
            {
                if (Mouse.current.position.ReadValue().x > Mouse.current.position.ReadValueFromPreviousFrame().x + 3f)
                {
                    myTransform.Rotate(0, 2f, 0);
                }
                else if (Mouse.current.position.ReadValue().x < Mouse.current.position.ReadValueFromPreviousFrame().x - 3f)
                {
                    myTransform.Rotate(0, -2f, 0);
                }
            }
        }

        public void Zoom_OnActiveCharacterWindowUI()
        {
            if (GUIManager.instance.windowPanel.characterWindowUI.skillPanel.gameObject.activeSelf.Equals(true))
                return;

            //위로 마우스 휠업을 하면, F버튼 아이콘이 상위 아이템 패널을 가리키게 된다.
            if (Mouse.current.scroll.y.ReadValue() > 0)
            {
                zoomLevel++;
                if (zoomLevel > 5)
                {
                    zoomLevel = 5;
                    return;
                }
                cameraTransform.localPosition += new Vector3(0, 0, 0.3f);
            }
            else if (Mouse.current.scroll.y.ReadValue() < 0)
            {
                zoomLevel--;
                if (zoomLevel < -5)
                {
                    zoomLevel = -5;
                    return;
                }
                cameraTransform.localPosition -= new Vector3(0, 0, 0.3f);
            }
        }
    }
}

