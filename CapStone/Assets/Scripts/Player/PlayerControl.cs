using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    /*private bool isPickUp = false;*/
    private bool isDead = false;
    private bool canSprint = true;

    private bool isBodyRotating = false;

    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        move = Vector3.zero;
        gravity = 10f;
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        Control();
        Stamina();
        UpdateAnimations();
    }

    /*private void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

        head.transform.localEulerAngles = new Vector3(mouseX, -mouseY, 0);

        if (!isBodyRotating && mouseX > 60 || mouseX < -60)
        {
            isBodyRotating = true;
            StartCoroutine(RotateBody());
        }

        if (isBodyRotating)
        {
            transform.rotation = Quaternion.Euler(0, -mouseX, 0);
        }
    }

    private IEnumerator RotateBody()
    {
        yield return new WaitForSeconds(0.5f);
        isBodyRotating = false;
    }*/

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
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = controller.transform.TransformDirection(move);
            isMoving = move.magnitude > 0;
            isRunning = canSprint && Input.GetKey(KeyCode.LeftShift);
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
}
