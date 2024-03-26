using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class RaycastAttack : NetworkBehaviour
{
    public float Damage = 10;

    public PlayerMovement PlayerMovement;

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }
        Ray ray = PlayerMovement.Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += PlayerMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log(ray);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
            if (Runner.GetPhysicsScene().Raycast(ray.origin,ray.direction, out var hit))
            {
                if (hit.transform.TryGetComponent<Health>(out var health))
                {
                    health.DealDamageRpc(Damage);
                }
            }
        }
    }
}
