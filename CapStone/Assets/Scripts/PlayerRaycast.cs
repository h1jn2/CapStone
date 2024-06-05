using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerRaycast : MonoBehaviour
{
    public enum HitObject
    {
        NotValid,
        Door,
        Item,
        
    }

    private const string str_Door = "Door";
    private const string str_Item = "Item";

    public void OnEnter_Space()
    {
        
    }

    public HitObject OnEnter_E(out Collider hitCollider)
    {
        RaycastHit hit;
        hitCollider = null;
                
        // 문 인식을 위해 현 위치에서 ray 쏨
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {

            if (hit.collider == null)
                return HitObject.NotValid;

            hitCollider = hit.collider;
            if (hit.collider.CompareTag(str_Door))
            {
                return HitObject.Door; 
            } 
            else if (hit.collider.CompareTag(str_Item))
            {
                return HitObject.Item;
            }
        }

        return HitObject.NotValid;
    }
    
    
}
