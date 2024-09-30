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
    private float steminaTime = 0f;
    public bool isPlayerRun = false;
    private AudioSource breathAudioSource;
    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (breathAudioSource != null && breathAudioSource.isPlaying)
        {
            if (isPlayerRun)
            {
                steminaTime += 0.1f * Time.deltaTime;
                steminaTime = Mathf.Clamp(steminaTime, 0f, 1f);
            }
            else
            {
                steminaTime -= 0.1f * Time.deltaTime;
                steminaTime = Mathf.Clamp(steminaTime, 0f, 1f);
            }
            breathAudioSource.volume = steminaTime;
            Debug.Log(breathAudioSource.volume);
        }
        
    }

    public void StopSound(AudioSource[] soundPlayer)
    {
        for (int j = 0; j < soundPlayer.Length; j++)
        {
            if (soundPlayer[j].isPlaying)
            {
                if (soundPlayer[j].clip.name.Equals("Hunting"))
                    return;
                if (soundPlayer[j].clip.name.Equals("Breath"))
                {
                    Debug.Log("stop");
                    continue;
                }
                Debug.Log("stoop");
                soundPlayer[j].Stop();
                return;
            }
        }
    }

    public void PlaySound(string _soundName, bool isLoop, AudioSource[] soundPlayer)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if (_soundName == sounds[i].soundName)
            {
                for (int j = 0; j < soundPlayer.Length; j++)
                {
                    if (!soundPlayer[j].isPlaying)
                    {
                        if (_soundName.Equals("Breath"))
                            breathAudioSource = soundPlayer[j];
                        
                        soundPlayer[j].clip = sounds[i].clip;
                        soundPlayer[j].loop = isLoop;
                        soundPlayer[j].Play();
                        return;
                    }
                    Debug.Log("모든 효과음 플레이어 사용 중");
                }
            }
        }
    }
}
