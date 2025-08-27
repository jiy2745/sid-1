using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI")]
    public Slider volumeSlider;          // Slider_Volume (0~100)
    public TMP_Text volumeCurrentText;     // Text_Current (Handle 자식)
    public TMP_Dropdown screenModeDropdown;    // Dropdown_ScreenMode (0=창,1=전체)
    public GameObject panelRoot;             // Panel_Settings

    [Header("Audio (선택)")]
    public AudioMixer masterMixer;           // Master.mixer 
    public string exposedParam = "MasterVolume";

    const string KEY_VOLUME = "VOLUME";
    const string KEY_MODE = "SCREENMODE";

    void Start()
    {
        // 볼륨 슬라이더 설정 + 이벤트
        if (volumeSlider)
        {
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 100;
            volumeSlider.wholeNumbers = true;

            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

            int savedVol = Mathf.Clamp(PlayerPrefs.GetInt(KEY_VOLUME, 80), 0, 100);
            volumeSlider.SetValueWithoutNotify(savedVol);
            OnVolumeChanged(savedVol); // 숫자/믹서 동기화
        }

        // 드롭다운 설정 + 이벤트
        if (screenModeDropdown)
        {
            EnsureScreenOptions();

            screenModeDropdown.onValueChanged.RemoveListener(OnScreenModeChanged);
            screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);

            int savedMode = Mathf.Clamp(PlayerPrefs.GetInt(KEY_MODE, 0), 0, 1);
            screenModeDropdown.SetValueWithoutNotify(savedMode);
            screenModeDropdown.RefreshShownValue();
            OnScreenModeChanged(savedMode); // 현재 모드 적용
        }
    }

    // 슬라이더 값 변경 시: 표시/오디오/저장
    public void OnVolumeChanged(float value)
    {
        int iv = Mathf.RoundToInt(value);
        if (volumeCurrentText) volumeCurrentText.text = iv.ToString();

        float linear = iv / 100f;

        if (masterMixer) // 믹서 사용
        {
            float dB = (iv == 0) ? -80f : Mathf.Log10(Mathf.Max(0.0001f, linear)) * 20f;
            masterMixer.SetFloat(exposedParam, dB);
        }
        else             // 간단 모드: 전체 볼륨
        {
            AudioListener.volume = linear; // 0~1
        }

        PlayerPrefs.SetInt(KEY_VOLUME, iv);
        PlayerPrefs.Save();
    }

    // 드롭다운 값 변경 시: 모드 적용/저장
    public void OnScreenModeChanged(int idx)
    {
        idx = Mathf.Clamp(idx, 0, 1);

        if (idx == 0) // 창 모드
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else          // 전체 화면(경계 없음)
        {
            var r = Screen.currentResolution;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(r.width, r.height, FullScreenMode.FullScreenWindow);
        }

#if UNITY_EDITOR
        Debug.Log($"[Settings] Requested: {(idx == 0 ? "Windowed" : "Fullscreen")}. " +
                  "Editor에서는 다르게 보일 수 있음(빌드로 확인).");
#endif
        PlayerPrefs.SetInt(KEY_MODE, idx);
        PlayerPrefs.Save();
    }

    // 닫기 버튼
    public void OnClickClose()
    {
        if (panelRoot) panelRoot.SetActive(false);
        // 필요하면 일시정지 해제:
        // Time.timeScale = 1f;
    }

    // 게임 종료 버튼
    public void OnClickQuitGame()
    {
        Debug.Log("[Settings] Quit requested");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void EnsureScreenOptions()
    {
        if (screenModeDropdown.options.Count < 2)
        {
            screenModeDropdown.ClearOptions();
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("창 모드"));
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("전체 화면"));
        }
    }
}
