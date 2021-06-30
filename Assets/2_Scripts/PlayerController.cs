using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3인칭 시점 캐릭터 이동을 제어하는 컴포넌트입니다.

/* 왜인지 모르겠으나, characterController를 받아와서 사용하면, 애니메이션이 부들부들떨림 
 * -> 그냥 리지드바디와 컬라이더를 설정하니 이 현상이 사라짐
 * 
 * 2D Freeform으로 하니까 움직인 이후 IK가 적용이 풀린다. 
 * -> 1D로 바꾸니까 이러한 현상이 없어짐. 왜지?
 * **/

/*
 * 1. 걷기
 * 2. 달리기
 * 3. 점프
 * **/


public enum MoveType
{
    WALK = 1,
    RUN = 2,
    LEFT = 4,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isWeaponOut = false;
    [SerializeField] private bool isMove = false;
    [SerializeField] private bool isWalk = false;
    [SerializeField] private bool isRun = false;

    private float moveSpeed = 0f;
    [Range(0, 2f)] [SerializeField] private float walkSpeed = 1f;
    [Range(0, 6f)] [SerializeField] private float runSpeed = 3f;
    [Range(0, .5f)] [SerializeField] private float turnSmoothTime = 0.1f;
    private float trunSmoothVelocity;

    //Component
    private Rigidbody myRigid;
    public Transform cam;
    private Animator ani;
    private DoublePressKeyDetection[] keys;

    private void Start()
    {
        ani = GetComponentInChildren<Animator>();
        myRigid = GetComponent<Rigidbody>();

        keys = new[]
        {
            //Consturctor //Param : KeyCode, threshold
            new DoublePressKeyDetection(KeyCode.W, 0.4f),
            new DoublePressKeyDetection(KeyCode.A, 0.4f),
            new DoublePressKeyDetection(KeyCode.S, 0.4f),
            new DoublePressKeyDetection(KeyCode.D, 0.4f),
        };
    }

    void Update()
    {
        //무기 교체
        if (Input.GetKeyUp(KeyCode.X) && !isMove)
        {
            isWeaponOut = !isWeaponOut;
            ani.SetBool("WeaponOut", isWeaponOut);
        }

        DoublePressKeyCheck();
        CheckMove();

        Movement(moveSpeed);

        if (isWalk)
            moveSpeed = walkSpeed;
        else if (isRun)
            moveSpeed = runSpeed;
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
        if (isRun)
        {
            if (keys[0].SinglePressed || keys[1].SinglePressed || keys[2].SinglePressed || keys[3].SinglePressed)
            {
                isWalk = false;
                isRun = true;
                return;
            }
        }

        if (keys[0].DoublePressed || keys[1].DoublePressed || keys[2].DoublePressed || keys[3].DoublePressed)
        {
            isWalk = false;
            isRun = true;
            return;
        }

        if (keys[0].SinglePressed || keys[1].SinglePressed || keys[2].SinglePressed || keys[3].SinglePressed)
        {
            isWalk = true;
            isRun = false;
        }

        if (keys[0].NonePressed && keys[1].NonePressed && keys[2].NonePressed && keys[3].NonePressed)
        {
            isWalk = false;
            isRun = false;
        }
    }

    private void DoublePressKeyCheck()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].UpdateCheck();
        }

        //if (keys[0].NonePressed && keys[1].NonePressed && keys[2].NonePressed && keys[3].NonePressed)
        //{
        //    isWalk = false;
        //    isRun = false;
        //}

        //keys[0].UpdateAction(() => isWalk = true, () => isRun = true);
        //keys[1].UpdateAction(() => isWalk = true, () => isRun = true);
        //keys[2].UpdateAction(() => isWalk = true, () => isRun = true);
        //keys[3].UpdateAction(() => isWalk = true, () => isRun = true);


        keys[0].UpdateAction(() => Debug.Log("W"), () => Debug.Log("WW"));
        keys[1].UpdateAction(() => Debug.Log("A"), () => Debug.Log("AA"));
        keys[2].UpdateAction(() => Debug.Log("S"), () => Debug.Log("SS"));
        keys[3].UpdateAction(() => Debug.Log("D"), () => Debug.Log("DD"));
    }


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

            myRigid.MovePosition(transform.position + moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void ChangeMoveStatus(bool status)
    {
        if (status == true)
        {
            isWalk = false;
            isRun = true;
        }
        else
        {
            isWalk = true;
            isRun = false;
        }
    }
}
