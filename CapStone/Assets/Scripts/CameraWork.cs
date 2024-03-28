using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    #region Private Fields

    [SerializeField] 
    private float distance = 7.0f;
    
    [SerializeField] 
    private float height = 3.0f;
    
    [SerializeField] 
    private Vector3 centerOffset = Vector3.zero;
    
    [SerializeField] 
    private bool followOnStart = false;
    
    [SerializeField] 
    private float smothSpeed = 0.0125f;

    private Transform cameraTransform;

    bool isFollowing;

    private Vector3 cameraOffset = Vector3.zero;

    #endregion

    #region  MonoBehaviour Callbacks

    void Start()
    {
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }

        if (isFollowing)
        {
            Follow();
        }
    }
    #endregion

    #region Public Methods

    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        Cut();
    }
    #endregion

    #region Private Methods

    void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
            this.transform.position +this.transform.TransformVector(cameraOffset), smothSpeed*Time.deltaTime);
        cameraTransform.LookAt(this.transform.position + centerOffset);
    }

    void Cut()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);
        
        cameraTransform.LookAt(this.transform.position + centerOffset);    
    }

    #endregion
}
