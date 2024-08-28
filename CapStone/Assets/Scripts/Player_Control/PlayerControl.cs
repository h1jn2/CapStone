using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviourPun
{
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
    private Animator animator;
    private PhotonView punview;

    public PlayerRaycast raycaster;
    private CabinetManager cabinetManager;

    [SerializeField]
    private AudioSource[] soundPlayer;

    private float revivalTime;
    private GameObject filled_F;

    public enum State
    {
        Moving,
        Running,
        Dead,
        Idle
    }

    [SerializeField]
    private State _curState;

    private void Awake()
    {
        ChangeState(State.Idle);
        controller = GetComponent<CharacterController>();
        move = Vector3.zero;
        gravity = 10f;
        animator = GetComponent<Animator>();
        punview = GetComponent<PhotonView>();
        cabinetManager = FindObjectOfType<CabinetManager>();

    }

    public void Update()
    {
        UpdateAnimations();
        if (this.GetComponent<PlayerManager>()._isDie)
        {
            ChangeState(State.Dead);
        }
        if (punview.IsMine && !this.GetComponent<PlayerManager>()._isDie)
        {
            Control();
            Stamina();

            switch (_curState)
            {
                case State.Moving:
                    if (isMoving && isRunning)
                    {
                        ChangeState(State.Running);
                    }
                    if (!isMoving && !isRunning)
                    {
                        ChangeState(State.Idle);
                    }
                    break;

                case State.Running:
                    if (!isRunning)
                    {
                        if (isMoving)
                        {
                            ChangeState(State.Moving);
                        }
                        else
                        {
                            ChangeState(State.Idle);
                        }
                    }
                    break;

                case State.Dead:
                    if (!GetComponent<PlayerManager>()._isDie)
                    {
                        ChangeState(State.Idle);
                    }
                    break;

                case State.Idle:
                    if (isMoving && !isRunning)
                    {
                        if (isRunning)
                        {
                            ChangeState(State.Running);
                        }
                        else
                        {
                            ChangeState(State.Moving);
                        }
                    }
                    break;
            }

            // ssg - 문 충돌이 간헐적으로 감지되지 않는 현상을 해결하기 위한 코드
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("enter F");
                Collider collider;
                PlayerRaycast.HitObject hitObject = raycaster.OnEnter_F(out collider);

                Debug.Log(hitObject);

                if (hitObject != PlayerRaycast.HitObject.NotValid && collider != null && photonView.IsMine)
                {
                    switch (hitObject)
                    {
                        case PlayerRaycast.HitObject.Door:
                            collider.GetComponent<DoorManager>().ChangeState();
                            break;
                        case PlayerRaycast.HitObject.DoubleDoor:
                            collider.GetComponent<DoubleDoorManager>().ChangeState();
                            break;
                        case PlayerRaycast.HitObject.Cabinet:
                            collider.GetComponent<CabinetManager>().ToggleHide();
                            break;
                        case PlayerRaycast.HitObject.Item:
                            PhotonView cpv = collider.GetComponent<PhotonView>();
                            SoundManager.instance.PlaySound("Item", false, soundPlayer);
                            cpv.RPC("DestroyItem_RPC", RpcTarget.All);
                            break;
                    }
                }
            }
            if (Input.GetKey(KeyCode.F))
            {
                Collider collider;
                PlayerRaycast.HitObject hitObject = raycaster.OnEnter_F(out collider);
                filled_F = GameObject.Find("IngameUi").transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
                Debug.Log(filled_F);

                if (hitObject != PlayerRaycast.HitObject.NotValid && collider != null && photonView.IsMine)
                {
                    switch (hitObject)
                    {
                        case PlayerRaycast.HitObject.Player:
                            if (collider.GetComponent<PlayerManager>()._isDie)
                                filled_F.GetComponent<Image>().fillAmount = revivalTime / 8;
                            if (revivalTime > 8f)
                            {
                                collider.GetComponent<PlayerNetwork>().RevivalUpdate();
                            }
                            else
                            {
                                revivalTime += Time.deltaTime;
                            }
                            break;
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                revivalTime = 0;
            }
        }
    }
    private void LateUpdate()
    {
        if (punview.IsMine)
        {
            CameraControl();
        }
    }

    private void ChangeState(State newState)
    {
        _curState = newState;

        switch (_curState)
        {
            case State.Moving:
                SoundManager.instance.StopSound(soundPlayer);
                SoundManager.instance.PlaySound("FootStepUser", true, soundPlayer);
                break;
            case State.Running:
                SoundManager.instance.StopSound(soundPlayer);
                SoundManager.instance.PlaySound("RunningFootStepUser", true, soundPlayer);
                break;
            case State.Dead:
                SoundManager.instance.StopSound(soundPlayer);
                break;
            case State.Idle:
                SoundManager.instance.StopSound(soundPlayer);
                break;
        }
    }

    private void CameraControl()
    {
        if (punview.IsMine)
        {
            PlayerCamera.transform.position = head.transform.position;

            mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
            mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

            mouseY = Mathf.Clamp(mouseY, -35f, 30f);

            PlayerCamera.transform.localRotation = Quaternion.Euler(-mouseY, 0, 0);
            head.transform.localRotation = Quaternion.Euler(0, 0, mouseY);
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
                moveDirection.y = 0f;
                moveDirection.Normalize();

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
    public void ControlCameraInCabinet()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;

        mouseX = Mathf.Clamp(mouseX, -35f, 35f);
        mouseY = Mathf.Clamp(mouseY, -35f, 35f);

        PlayerCamera.transform.localRotation = Quaternion.Euler(-mouseY, mouseX, 0);
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