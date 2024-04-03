using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMgr : MonoBehaviour
{
    public AudioClip bgm1;
    private AudioSource bgmSource;

    // �����̴�
    public Slider volumeSlider;

    // ��ǥ ����
    private float targetVolume = 0.5f;
    // ���� ����
    private float currentVolume = 0.5f;
    // �ʱ� �Ҹ� ũ��
    private float initialVolume = 0.5f;
    // ������Ʈ �迭
    public GameObject[] targetObjects;
    // ������Ʈ�� �����ִ��� ����
    private bool isObjectOpen = false;
    // ���� ���� �ӵ�
    public float volumeChangeSpeed = 0.1f; // �� �κп��� ���� ���� �ӵ��� �����մϴ�.

    void Start()
    {
        // AudioSource ������Ʈ �߰�
        bgmSource = gameObject.AddComponent<AudioSource>();

        // BGM ����
        bgmSource.clip = bgm1;

        // ���� ����
        bgmSource.loop = true;

        // BGM1 ���
        bgmSource.Play();

        // �ʱ� �Ҹ� ũ�� ����
        initialVolume = bgmSource.volume;
    }

    void Update()
    {
        // �����̴� ������ ��ǥ ���� ����
        targetVolume = volumeSlider.value;

        // �����̴��� ������ ���� �Ҹ� ��� ����
        if (!isObjectOpen)
        {
            bgmSource.volume = targetVolume;
        }
        else
        {
            // ������Ʈ�� ���� ���� ���� �������� ��ǥ �������� ������ ����
            if (currentVolume > targetVolume)
            {
                currentVolume -= volumeChangeSpeed * Time.deltaTime;
                currentVolume = Mathf.Max(currentVolume, targetVolume);
            }
            else if (currentVolume < targetVolume)
            {
                currentVolume += volumeChangeSpeed * Time.deltaTime;
                currentVolume = Mathf.Min(currentVolume, targetVolume);
            }

            bgmSource.volume = currentVolume;
        }
    }

    // ������Ʈ�� �����ų� ���� �� ȣ��Ǵ� �޼���
    public void SetObjectState(bool isOpen)
    {
        isObjectOpen = isOpen;
        // ������Ʈ�� ���� �� ��ǥ ������ ���������� ���Դϴ�.
        if (isOpen)
        {
            targetVolume = initialVolume * 0.5f; // ���� ������ 50%�� ����
        }
    }
}
