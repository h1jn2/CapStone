using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveCloud : MonoBehaviour
{
    public float speed = 15f; // 이미지의 이동 속도

    private RectTransform rectTransform; // 이미지의 RectTransform 컴포넌트

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // 이미지의 RectTransform 컴포넌트 가져오기
    }

    void Update()
    {
        // 이미지를 오른쪽으로 이동
        rectTransform.Translate(Vector3.right * speed * Time.deltaTime);

        // 이미지가 캔버스를 벗어나면
        if (rectTransform.anchoredPosition.x > Screen.width / 2 + rectTransform.rect.width / 2)
        {
            // 이미지를 캔버스 왼쪽 끝으로 이동
            rectTransform.anchoredPosition = new Vector2(-Screen.width / 2 - rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
        }
    }
}
