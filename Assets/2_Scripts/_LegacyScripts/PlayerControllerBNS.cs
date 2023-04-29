using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3인칭 시점 캐릭터 이동을 제어하는 컴포넌트입니다.

/* 왜인지 모르겠으나, characterController를 받아와서 사용하면, 애니메이션이 부들부들떨림 
 * -> 그냥 리지드바디와 컬라이더를 설정하니 이 현상이 사라짐
 * -> 리지드바디로 쓰니 제약이 너무 많아짐
 * -> 캐릭터 컨트롤러로 다시 바꾸고 하위 계층으로 옮기니 잘됨.
 * 
 * 2D Freeform으로 하니까 움직인 이후 IK가 적용이 풀린다. 
 * -> 1D로 바꾸니까 이러한 현상이 없어짐. 왜지?
 * **/

/*
 * 1. 걷기
 * 2. 달리기
 * 3. 점프
 * **/

/*BNS 방식의 컨트롤
 * PC게임에 적합합니다.
 * 
 * **/

public enum PlayerMove
{
    IDLE,
    WALK,
    JOG,
    RUN,
    Attack
}

public class PlayerControllerBNS : MonoBehaviour
{
    [SerializeField] private bool isWeaponOut = false;
    [SerializeField] private bool isWalk = false;
    [SerializeField] private PlayerMove playerMove = PlayerMove.IDLE;
    public LayerMask layerMask;

    public bool enableRM;
    private bool isWeaponChanging = false;
    //private bool isJumping = false;
    private bool isGround = true;

    [SerializeField] private int attackCount = 0;
    private float gravity = -9.81f;
    private Vector3 gravityForce;
    private float moveSpeed = 0f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float jogSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    //[SerializeField] private float jumpSpeed = 10f;
    private float trunSmoothVelocity;
    [Range(0f, 1f)]
    [SerializeField] private float DistanceToGround;



    private Dictionary<KeyCode, Action> keyDics = new Dictionary<KeyCode, Action>();

    //Component
    public Transform cam;
    private Animator ani;
    private DoublePressKeyDetection[] keys;
    private CharacterController characterController;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    private AnimatorStateInfo aniStateInfo;


    private void Start()
    {
        ani = GetComponentInChildren<Animator>();
        characterController = GetComponentInChildren<CharacterController>();
        /*        myRigid = GetComponentInChildren<Rigidbody>();
                capsuleCollider = GetComponentInChildren<CapsuleCollider>();*/



        keys = new[]
        {
            //Consturctor //Param : KeyCode, threshold
            new DoublePressKeyDetection(KeyCode.W, 0.4f),
            new DoublePressKeyDetection(KeyCode.A, 0.4f),
            new DoublePressKeyDetection(KeyCode.S, 0.4f),
            new DoublePressKeyDetection(KeyCode.D, 0.4f),
        };
    }

    void FixedUpdate()
    {
        enableRM = !ani.GetBool("canMove");
        ani.applyRootMotion = enableRM;
        aniStateInfo = ani.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(aniStateInfo.normalizedTime);


        if (enableRM && playerMove != PlayerMove.Attack)
            return;

        if (isWeaponChanging)
            return;

        if (!characterController.isGrounded)
        {
            gravityForce = new Vector3(0f, gravity * Time.deltaTime, 0f);
            characterController.Move(gravityForce);
            //Debug.Log("Not Grounded. Add Gravity Force");
        }


        ExecuteWeaponOut();
        DoublePressKeyCheck();
        CheckMove();
        ChangeMoveStatus();
        SwitchWalk();
        // TryJump();
        Movement(moveSpeed);
        TryDodge();
        TryAttack();
    }

    private void TryAttack()
    {
        /*
         * 무기가 꺼내져있는 상태에서
         * Idle,Walk,Jog 상태에서 공격시 - 공격 콤보
         * Run 상태에서 공격시, 대시 공격
         * **/
        if (isWeaponOut == true)
        {
            if (!aniStateInfo.IsName("Locomotion_WeaponOut") && playerMove == PlayerMove.Attack)
            {
                if (aniStateInfo.normalizedTime >= 1.0f)
                {
                    if (ani.IsInTransition(0))
                        return;

                    attackCount = 0;
                    ani.SetInteger("Combo", attackCount);
                    ani.SetTrigger("Idle_WeaponOut");
                }
            }

            //공격키
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //만약 다른 애니메이션으로 거쳐가는 중이라면 취소
                if (ani.IsInTransition(0))
                {
                    Debug.Log("애니메이션 변경중");
                    return;
                }

                if (attackCount == 0)
                {
                    attackCount = 1;
                    ani.SetInteger("Combo", attackCount);
                    playerMove = PlayerMove.Attack;
                    Debug.Log("공격하기");
                }
                else
                {
                    //애니메이션 진행도가 애니메이션 길이의 절반을 넘어섰다면
                    if (aniStateInfo.normalizedTime >= 0.5f)
                    {
                        attackCount++;
                        ani.SetInteger("Combo", attackCount);
                        playerMove = PlayerMove.Attack;
                        Debug.Log("공격하기");
                    }
                }
            }

            if (aniStateInfo.IsName("Attack6") == true)
            {
                attackCount = 0;
                ani.SetInteger("Combo", attackCount);
            }
        }
    }

    private void TryDodge()
    {
        //왼쪽 시프트키를 누르면 회피를 시전합니다. (기본방향 : 뒤)
        //추가로 방향키를 입력하면 그 방향으로 이동합니다.
        //param DodgeDir 0: 앞 1: 왼  2: 오 3: 뒤
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ani.SetInteger("DodgeDir", 3);
            if (Input.GetKey(KeyCode.W))
            {
                ani.SetInteger("DodgeDir", 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                ani.SetInteger("DodgeDir", 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                ani.SetInteger("DodgeDir", 2);
            }

            ani.SetTrigger("Dodge");
        }
    }

    private void TryJump()
    {
        IsGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Debug.Log("점프한다!");
            ani.SetTrigger("Jump");
        }
    }

    private void ExecuteWeaponOut()
    {
        //무기 꺼내기
        if (Input.GetKeyUp(KeyCode.X) && playerMove == PlayerMove.IDLE)
        {
            isWeaponOut = !isWeaponOut;
            ChangeWeaponCoroutine();
        }
    }

    private void ChangeWeaponCoroutine()
    {
        isWeaponChanging = true;
        ani.SetBool("WeaponOut", isWeaponOut);
    }

    private void EndChangeWeapon()
    {
        isWeaponChanging = false;
    }

    private void SwitchWalk()
    {
        //왼쪽 컨트롤키를 누르면 걷기, 조깅 모드가 바뀝니다.
        if (Input.GetKeyDown(KeyCode.LeftControl) && playerMove == PlayerMove.IDLE)
        {
            isWalk = !isWalk;
        }
    }

    private void CheckMove()
    {
        /*
         * - 아무 키도 입력중이지 않다면 정지 상태
         * - 방향키 한번 누르고 있는 상태라면 걷기
         *      걷는 도중에 다른 키를 눌러도 걷는 상태 유지
         * - 방향키 두번 누르는 상태라면 달리기
         *      달리는 도중에 다른 키를 눌러도 달리는 상태 유지
         *     
         * 위의 사항을 각각 키를 개별적으로 검사 
         * **/
        if (playerMove == PlayerMove.RUN)
        {
            if (keys[0].SinglePressed || keys[1].SinglePressed || keys[2].SinglePressed || keys[3].SinglePressed)
            {
                playerMove = PlayerMove.RUN;
                return;
            }
        }

        if (keys[0].DoublePressed || keys[1].DoublePressed || keys[2].DoublePressed || keys[3].DoublePressed)
        {
            if (isWalk)
                isWalk = false;

            playerMove = PlayerMove.RUN;
            return;
        }

        if (keys[0].SinglePressed || keys[1].SinglePressed || keys[2].SinglePressed || keys[3].SinglePressed)
        {
            if (!isWalk)
                playerMove = PlayerMove.JOG;
            else
                playerMove = PlayerMove.WALK;
        }

        if (keys[0].NonePressed && keys[1].NonePressed && keys[2].NonePressed && keys[3].NonePressed)
        {
            playerMove = PlayerMove.IDLE;
        }
    }

    //키가 두번 눌렸는지 체크하는 함수입니다.
    private void DoublePressKeyCheck()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].UpdateCheck();
        }

        //keys[0].UpdateAction(() => Debug.Log("W"), () => Debug.Log("WW"));
        //keys[1].UpdateAction(() => Debug.Log("A"), () => Debug.Log("AA"));
        //keys[2].UpdateAction(() => Debug.Log("S"), () => Debug.Log("SS"));
        //keys[3].UpdateAction(() => Debug.Log("D"), () => Debug.Log("DD"));
    }

    //플레이어를 이동시킵니다.
    private void Movement(float moveSpeed = 0f)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical);
        Vector3 direction = dir.normalized;
        float clampDir = Mathf.Clamp(dir.magnitude, 0f, direction.magnitude);
        ani.SetFloat("Vertical", clampDir * moveSpeed);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref trunSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //myRigid.MovePosition(transform.position + moveDir.normalized * moveSpeed * Time.deltaTime);
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        }
    }

    //플레이어의 이동 상태에 따라 속도를 다르게 하는 함수입니다.
    private void ChangeMoveStatus()
    {
        if (playerMove == PlayerMove.WALK)
            moveSpeed = walkSpeed;
        else if (playerMove == PlayerMove.JOG)
            moveSpeed = jogSpeed;
        else if (playerMove == PlayerMove.RUN)
            moveSpeed = runSpeed;
    }

    private void IsGround()
    {
        //항상 레이저는 캡슐 기준이 아닌 절대적으로 아래로 쏴야하기 때문에 Vector.down를 쓴다.
        //bounds : Collider의 영역 / entents : bounds의 절반
        //정확히 반사이즈만 주게되면 계단을 오를때 부자연스러울 수 있기에 +0.1f 오차를 넣는다.

        //빛을 쏠때.. 고려해야할점
        //어디에서, 어느 방향으로, 얼만큼, (누굴 맞췄는지) 등등

        isGround = Physics.Raycast(transform.position, Vector3.down, characterController.bounds.extents.y + 0.1f);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ani)
        {
            ani.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            ani.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            //Left Foot
            RaycastHit hit;
            Ray ray = new Ray(ani.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += DistanceToGround;
                    ani.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                }
            }
        }
    }
}
