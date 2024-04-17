using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviourPun
{
    public GameObject cabinet;

    [SerializeField]
    private float mouseSpeed = 8f;
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
    public PhotonView pv;

    private bool isInsideCabinet = false;

    private void Awake()
    {
        controller = this.GetComponent<CharacterController>();
        move = Vector3.zero;
        gravity = 10f;
        animator = this.gameObject.GetComponent<Animator>();
        pv = this.gameObject.GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (pv.IsMine)
        {
            Control();
            Stamina();
            UpdateAnimations();
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
        }
    }

    private void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

        this.transform.localEulerAngles = new Vector3(-mouseY, mouseX, 0);
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

            Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

            if (moveDirection != Vector3.zero)
            {
                move = transform.TransformDirection(moveDirection);
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

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isRunning", isRunning);
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
}
