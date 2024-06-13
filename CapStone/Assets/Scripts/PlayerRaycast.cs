using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerRaycast : MonoBehaviour
{
    //검출할 태그를 enum으로 열거
    public enum HitObject
    {
        NotValid,
        Door,
        Item,
        
    }
    //최적화를 위해 const로 문자열 선언
    private const string str_Door = "Door";
    private const string str_DoubleDoor = "DoubleDoor";
    private const string str_Item = "Item";

    //캐비넷 검출을 위한 예비 함수
    public void OnEnter_Space()
    {
        
    }
    // E를 누를시 레이를 이용하여 콜라이더 검출하여 해당하는 조건 실행함수
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
            if (hit.collider.CompareTag(str_DoubleDoor))
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
