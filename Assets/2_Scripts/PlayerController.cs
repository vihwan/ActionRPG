using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //무기 교체
        if (Input.GetKeyUp(KeyCode.X))
        {
            isWeaponOut = !isWeaponOut;
            ani.SetBool("WeaponOut", isWeaponOut);
        }

        if (isWalk)
            moveSpeed = walkSpeed;
        else if (isRun)
            moveSpeed = runSpeed;

        Movement(moveSpeed);
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

    private void ChangeMoveStatus()
    {
        //왼쪽 컨트롤키를 눌러 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            
        }
    }
}
