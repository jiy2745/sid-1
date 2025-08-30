using UnityEngine;

public class MusicManager : MonoBehaviour
{
   
    public static MusicManager instance;
    public AudioClip day1Music;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.AddListener(CheckAndPlayMusic);
        }
        CheckAndPlayMusic();
    }
    
    
    public void PlayMusicDirectly(AudioClip musicClip)
    {
        if (musicClip == null)
        {
            Debug.LogWarning("재생할 AudioClip이 전달되지 않았습니다. 음악을 재생하지 않습니다.");
            return;
        }

      
        if (audioSource.clip == musicClip && audioSource.isPlaying)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = musicClip;
        audioSource.loop = true; 
        audioSource.Play();
        Debug.Log($"[{musicClip.name}] BGM을 직접 재생합니다.");
    }

 
    void CheckAndPlayMusic()
    {
        if (GameManager.instance == null) return;

        if (GameManager.instance.currentDay == 1)
        {
            if (audioSource.clip != day1Music)
            {
                audioSource.clip = day1Music;
                audioSource.Play();
                Debug.Log("1일차 낮 BGM을 재생합니다.");
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("조건에 맞지 않아 BGM을 중지합니다.");
            }
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.RemoveListener(CheckAndPlayMusic);
        }
    }
}