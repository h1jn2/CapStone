using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion
    #region Private Fields

    [SerializeField]
    private GameObject beams;

    private bool IsFiring;

    #endregion

    #region Public Fields

    public float Health = 1f;
    public GameObject palyerPrefab;

    #endregion
    
    #region  MonoBehaviour CallBacks

    void Awake()
    {
        if (beams == null)
        {
            Debug.Log("무기 오브젝트 불러오기 오류");
        }
        else
        {
            beams.SetActive(false);
        }
    }

    void Start()
    {
        if (palyerPrefab == null)
        {
            
        }
        else
        {
            PhotonNetwork.Instantiate(this.palyerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
            
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.Log("카메라제어 컴포넌트 연결불가");
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
            if (Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
        if (beams != null && IsFiring != beams.activeInHierarchy)
        {
            beams.SetActive(IsFiring);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }

        Health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }
        Health -= 0.1f * Time.deltaTime;
    }

    #endregion

    #region Custom

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsFiring)
            {
                IsFiring = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (IsFiring)
            {
                IsFiring = false;
            }
        }
    }

    #endregion
}
