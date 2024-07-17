using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [SerializeField] Sound[] sounds;
    [SerializeField] AudioSource[] soundPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StopSound()
    {
        for (int j = 0; j < soundPlayer.Length; j++)
        {
            if (soundPlayer[j].isPlaying)
            {
                soundPlayer[j].Stop();
                return;
            }
        }
    }

    public void PlaySound(string _soundName)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if (_soundName == sounds[i].soundName)
            {
                for (int j = 0; j < soundPlayer.Length; j++)
                {
                    if (!soundPlayer[j].isPlaying)
                    {
                        soundPlayer[j].clip = sounds[i].clip;
                        soundPlayer[j].Play();
                        return;
                    }
                    Debug.Log("모든 효과음 플레이어 사용 중");
                }
            }
        }
    }
}
