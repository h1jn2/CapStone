using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerControl : MonoBehaviourPun
{
    public GameObject cabinet;

    [SerializeField]
    private float mouseSpeed = 8f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 10f;
    [SerializeField]
    private float stamina = 100f;
    [SerializeField]
    private float maxStamina = 100f;
    [SerializeField]
    private float deStamina = 20f;
    [SerializeField]
    private float reStamina = 10f;
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private GameObject PlayerCamera;

    private float mouseX;
    private float mouseY;

    private float gravity;
    private CharacterController controller;
    private Vector3 move;

    private bool isMoving = false;
    private bool isRunning = false;
    private bool canSprint = true;
    private bool isHiding = false;

    private Animator animator;
    private PhotonView punview;

    private bool isInsideCabinet = false;

    public PlayerRaycast raycaster;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        move = Vector3.zero;
        gravity = 10f;
        animator = GetComponent<Animator>();
        punview = GetComponent<PhotonView>();
    }

    public void Update()
    {
        UpdateAnimations();
        if (punview.IsMine && !this.GetComponent<PlayerManager>()._isDie)
        {
            
            Control();
            Stamina();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsPlayerNearCabinet())
                {
                    ToggleHide();
                }
                else if (isInsideCabinet)
                {
                    ToggleHide();
                }
            }
            
            // ssg - 문 충돌이 간헐적으로 감지되지 않는 현상을 해결하기 위한 코드
            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("enter E");
                Collider collider;
                PlayerRaycast.HitObject hitObject = raycaster.OnEnter_E(out collider);

                Debug.Log(hitObject);
                
                if (hitObject != PlayerRaycast.HitObject.NotValid && collider != null && photonView.IsMine )
                {
                    switch (hitObject)
                    {
                        case PlayerRaycast.HitObject.Door:
                            collider.GetComponent<DoorManager>().ChangeState();
                            break;
                        case PlayerRaycast.HitObject.Item:
                            PhotonView cpv = collider.GetComponent<PhotonView>();
                            cpv.RPC("DestroyItem_RPC",RpcTarget.All);
                            
                            break;
                    }
                }

            }
        }
        
    }

    private void LateUpdate()
    {
        if (punview.IsMine && !this.GetComponent<PlayerManager>()._isDie)
        {
            PlayerCamera.transform.position = head.transform.position;

            mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
            mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

            mouseY = Mathf.Clamp(mouseY, -60f, 60f);
            head.transform.localRotation = Quaternion.Euler((float)(-mouseX/6), 0, mouseY);
            this.transform.localRotation = Quaternion.Euler(0, mouseX, 0);    
        }
    }
    
    private void Control()
    {
        float currentSpeed = canSprint && Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        if (canSprint && currentSpeed == sprintSpeed)
        {
            stamina -= deStamina * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        }

        if (controller.isGrounded)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            Vector3 moveDirection = (moveX * PlayerCamera.transform.right + moveZ * PlayerCamera.transform.forward).normalized;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                move = moveDirection * currentSpeed;
                isMoving = true;
                isRunning = canSprint && Input.GetKey(KeyCode.LeftShift);
            }
            else
            {
                move = Vector3.zero;
                isMoving = false;
                isRunning = false;
            }
        }
        else
        {
            move.y -= gravity * Time.deltaTime;
        }

        controller.Move(move * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isDead", this.GetComponent<PlayerManager>()._isDie);
    }

    private void Stamina()
    {
        if (stamina <= 0)
        {
            canSprint = false;
        }

        stamina += reStamina * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        if (stamina >= maxStamina)
        {
            canSprint = true;
        }
    }

    private bool IsPlayerNearCabinet()
    {
        float distance = Vector3.Distance(transform.position, cabinet.transform.position);
        return distance < 3f;
    }

    private void ToggleHide()
    {
        if (isHiding)
        {
            transform.position = cabinet.transform.position + new Vector3(0f, 1f, 0f);
            isInsideCabinet = false;
        }
        else
        {
            transform.position = cabinet.transform.position;
            isInsideCabinet = true;
        }

        isHiding = !isHiding;
    }

    private void ChangeDoorState(Collider other)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (other.CompareTag("Door"))
            {
                if (photonView.IsMine)
                {
                    other.GetComponent<DoorManager>().ChangeState();
                }
            }
        }
    }
  
}
