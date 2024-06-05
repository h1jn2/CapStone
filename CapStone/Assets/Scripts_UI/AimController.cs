using UnityEngine;

public class AimController : MonoBehaviour
{
    public Camera mainCamera; // 카메라 GameObject
    public GameObject aimImage; // 이미지 GameObject
    public float raycastDistance = 100f; // 레이캐스트 거리
    public float activationDistance = 7f; // 이미지 활성화 거리

    void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 카메라의 중앙에서 레이 생성

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("cabinet") || hit.collider.CompareTag("item") || hit.collider.CompareTag("door")) // 카메라가 맵에 있는 3D 오브젝트를 쳐다보고 있는지 확인 //tag : door, item, cabinet추가 04.23
            {
                // 오브젝트와 플레이어 간의 거리 계산
                float distance = Vector3.Distance(hit.collider.transform.position, mainCamera.transform.position);

                if (distance <= activationDistance) // 플레이어와 오브젝트 간의 거리가 activationDistance 이내일 때
                {
                    aimImage.SetActive(true); // 이미지 활성화
                }
                else
                {
                    aimImage.SetActive(false); // 플레이어와 오브젝트 간의 거리가 activationDistance보다 클 때 이미지 비활성화
                }
            }
            else
            {
                aimImage.SetActive(false); // 다른 오브젝트를 쳐다보고 있다면 이미지 비활성화
            }
        }
        else
        {
            aimImage.SetActive(false); // 아무것도 쳐다보지 않을 때 이미지 비활성화
        }
    }
}
