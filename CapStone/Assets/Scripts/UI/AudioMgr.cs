using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMgr : MonoBehaviour
{
    public AudioClip bgm1;
    private AudioSource bgmSource;

    // 슬라이더
    public Slider volumeSlider;

    // 목표 음량
    private float targetVolume = 0.5f;
    // 현재 음량
    private float currentVolume = 0.5f;
    // 초기 소리 크기
    private float initialVolume = 0.5f;
    // 오브젝트 배열
    public GameObject[] targetObjects;
    // 오브젝트가 열려있는지 여부
    private bool isObjectOpen = false;
    // 음량 조절 속도
    public float volumeChangeSpeed = 0.1f; // 이 부분에서 음량 조절 속도를 설정합니다.

    void Start()
    {
        // AudioSource 컴포넌트 추가
        bgmSource = gameObject.AddComponent<AudioSource>();

        // BGM 설정
        bgmSource.clip = bgm1;

        // 루프 설정
        bgmSource.loop = true;

        // BGM1 재생
        bgmSource.Play();

        // 초기 소리 크기 설정
        initialVolume = bgmSource.volume;
    }

    void Update()
    {
        // 슬라이더 값으로 목표 음량 설정
        targetVolume = volumeSlider.value;

        // 슬라이더로 조절할 때는 소리 즉시 변경
        if (!isObjectOpen)
        {
            bgmSource.volume = targetVolume;
        }
        else
        {
            // 오브젝트가 열릴 때는 현재 볼륨에서 목표 볼륨으로 서서히 변경
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

    // 오브젝트가 열리거나 닫힐 때 호출되는 메서드
    public void SetObjectState(bool isOpen)
    {
        isObjectOpen = isOpen;
        // 오브젝트가 열릴 때 목표 음량을 설정값으로 줄입니다.
        if (isOpen)
        {
            targetVolume = initialVolume * 0.5f; // 현재 볼륨의 50%로 설정
        }
    }
}
