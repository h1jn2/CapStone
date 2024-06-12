using UnityEngine;
using Cinemachine;

public class AimController : MonoBehaviour
{
    //public Cinemachine mainCamera;
    public Camera mainCamera; // ī�޶� GameObject
    public GameObject aimImage; // �̹��� GameObject
    public float raycastDistance = 100f; // ����ĳ��Ʈ �Ÿ�
    public float activationDistance = 7f; // �̹��� Ȱ��ȭ �Ÿ�

    void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // ī�޶��� �߾ӿ��� ���� ����

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("cabinet") || hit.collider.CompareTag("item") || hit.collider.CompareTag("door")) // ī�޶� �ʿ� �ִ� 3D ������Ʈ�� �Ĵٺ��� �ִ��� Ȯ�� //tag : door, item, cabinet�߰� 04.23
            {
                // ������Ʈ�� �÷��̾� ���� �Ÿ� ���
                float distance = Vector3.Distance(hit.collider.transform.position, mainCamera.transform.position);

                if (distance <= activationDistance) // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance �̳��� ��
                {
                    aimImage.SetActive(true); // �̹��� Ȱ��ȭ
                }
                else
                {
                    aimImage.SetActive(false); // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance���� Ŭ �� �̹��� ��Ȱ��ȭ
                }
            }
            else
            {
                aimImage.SetActive(false); // �ٸ� ������Ʈ�� �Ĵٺ��� �ִٸ� �̹��� ��Ȱ��ȭ
            }
        }
        else
        {
            aimImage.SetActive(false); // �ƹ��͵� �Ĵٺ��� ���� �� �̹��� ��Ȱ��ȭ
        }
    }
}
