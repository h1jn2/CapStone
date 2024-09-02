using UnityEngine;

public class AimController : MonoBehaviour
{
    public Camera mainCamera; // ī�޶� GameObject
    public GameObject aimImage; // �̹��� GameObject
    public GameObject aimFilledImage;
    public float raycastDistance = 100f; // ����ĳ��Ʈ �Ÿ�
    public float activationDistance = 7f; // �̹��� Ȱ��ȭ �Ÿ�
    private PlayerManager pm;

    void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // ī�޶��� �߾ӿ��� ���� ����

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.GetComponent<PlayerManager>() != null)
            {
                pm = hit.collider.GetComponent<PlayerManager>();
            }
            if (hit.collider.CompareTag("Cabinet") || hit.collider.CompareTag("Item") || hit.collider.CompareTag("Door") || hit.collider.CompareTag("DoubleDoor")) // ī�޶� �ʿ� �ִ� 3D ������Ʈ�� �Ĵٺ��� �ִ��� Ȯ�� //tag : door, item, cabinet�߰� 04.23
            {
                // ������Ʈ�� �÷��̾� ���� �Ÿ� ���
                float distance = Vector3.Distance(hit.collider.transform.position, mainCamera.transform.position);

                if (distance <= activationDistance) // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance �̳��� ��
                {
                    aimImage.SetActive(true); // �̹��� Ȱ��ȭ
                    aimFilledImage.SetActive(false);
                }
                else
                {
                    aimImage.SetActive(false); // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance���� Ŭ �� �̹��� ��Ȱ��ȭ
                    aimFilledImage.SetActive(false);
                }
            }
            else
            {
                aimImage.SetActive(false); // �ٸ� ������Ʈ�� �Ĵٺ��� �ִٸ� �̹��� ��Ȱ��ȭ
            }
            if (hit.collider.CompareTag("Player") && pm._isDie)
            {
                float distance = Vector3.Distance(hit.collider.transform.position, mainCamera.transform.position);

                if (distance <= activationDistance) // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance �̳��� ��
                {
                    aimImage.SetActive(true); // �̹��� Ȱ��ȭ
                    aimFilledImage.SetActive(true);
                }
                else
                {
                    aimImage.SetActive(false); // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance���� Ŭ �� �̹��� ��Ȱ��ȭ
                    aimFilledImage.SetActive(false);

                }
            }
            else if (hit.collider.CompareTag("Player") && !pm._isDie)
            {
                aimImage.SetActive(false); // �÷��̾�� ������Ʈ ���� �Ÿ��� activationDistance���� Ŭ �� �̹��� ��Ȱ��ȭ
                aimFilledImage.SetActive(false);

            }
        }
        else
        {
            aimImage.SetActive(false); // �ƹ��͵� �Ĵٺ��� ���� �� �̹��� ��Ȱ��ȭ
            aimFilledImage.SetActive(false);
        }
    }
}
