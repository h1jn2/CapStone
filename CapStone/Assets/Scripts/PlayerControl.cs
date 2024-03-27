using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 10f;
    [SerializeField]
    private float mouseSpeed = 8f;
    [SerializeField]
    private float stamina = 100f;
    [SerializeField]
    private float maxStamina = 100f;
    [SerializeField]
    private float deStamina = 20f;
    [SerializeField]
    private float reStamina = 10f;
    private float gravity;
    private CharacterController controller;
    private Vector3 move;


    private float mouseX;
    private float mouseY;

    private bool canSprint = true;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        move = Vector3.zero;
        gravity = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Control();
        Stamina();
    }

    private void Control()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        this.transform.localEulerAngles = new Vector3(-mouseY, mouseX, 0);

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
        }
        else
        {
            move.y -= gravity * Time.deltaTime;
        }

        controller.Move(move * Time.deltaTime * currentSpeed);
    }

    public void Stamina()
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