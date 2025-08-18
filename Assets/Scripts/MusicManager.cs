using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static MusicManager instance;

    // Inspector 창에서 연결할 1일차 낮 배경음악
    public AudioClip day1Music;
    // (추후 확장 가능) 밤 배경음악, 다른 날짜 배경음악 등을 여기에 추가
    
    // AudioSource 컴포넌트를 담을 변수
    private AudioSource audioSource;

    void Awake()
    {
        
        // 씬에 MusicManager 인스턴스가 아직 없다면
        if (instance == null)
        {
            // 이 인스턴스를 사용
            instance = this;
            // 씬이 바뀌어도 이 오브젝트는 파괴되지 않도록 설정
            DontDestroyOnLoad(gameObject);
        }
        // 이미 인스턴스가 존재한다면 (다른 씬에서 넘어왔다면)
        else
        {
            // 새로 생긴건 파괴하여 중복 및 음악 중첩 재생 방지
            Destroy(gameObject);
            return; // 중복된 경우, 아래 코드를 실행하지 않도록 함수 종료
        }

        // 자신의 AudioSource 컴포넌트를 가져와 변수에 할당
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // GameManager의 상태가 변경될 때마다 음악을 체크하는 함수를 호출하도록 구독
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.AddListener(CheckAndPlayMusic);
        }

        // 게임 시작 시점의 상태에 맞는 음악을 즉시 재생
        CheckAndPlayMusic();
    }
    
    // 현재 게임 상태를 확인하고 그에 맞는 음악을 재생하는 함수
    void CheckAndPlayMusic()
    {
        if (GameManager.instance == null) return;

        // "1일차 낮" 상태일 경우
        if (GameManager.instance.currentDay == 1)
        {
            // 현재 재생 중인 음악이 1일차 음악이 아니라면
            if (audioSource.clip != day1Music)
            {
                audioSource.clip = day1Music; // 오디오 클립을 1일차 음악으로 변경
                audioSource.Play();           // 재생 시작
                Debug.Log("1일차 낮 BGM을 재생합니다.");
            }
        }
        // 다른 조건이 있다면 여기에 추가 (예: 2일차 또는 밤)
        // else if (GameManager.instance.currentDay == 2) { ... }
        else
        {
            // 그 외의 모든 상황에서는 음악을 멈춤
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("조건에 맞지 않아 BGM을 중지합니다.");
            }
        }
    }

    // (선택사항) 스크립트에서 음악 볼륨을 조절하고 싶을 때 사용할 함수
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // 오브젝트가 파괴될 때 이벤트 구독 해제 
    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onStateChanged.RemoveListener(CheckAndPlayMusic);
        }
    }
}