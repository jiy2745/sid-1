using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI")]
    public Slider volumeSlider;          // Slider_Volume (0~100)
    public TMP_Text volumeCurrentText;     // Text_Current (Handle �ڽ�)
    public TMP_Dropdown screenModeDropdown;    // Dropdown_ScreenMode (0=â,1=��ü)
    public GameObject panelRoot;             // Panel_Settings

    [Header("Audio (����)")]
    public AudioMixer masterMixer;           // Master.mixer 
    public string exposedParam = "MasterVolume";

    const string KEY_VOLUME = "VOLUME";
    const string KEY_MODE = "SCREENMODE";

    void Start()
    {
        // ���� �����̴� ���� + �̺�Ʈ
        if (volumeSlider)
        {
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 100;
            volumeSlider.wholeNumbers = true;

            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

            int savedVol = Mathf.Clamp(PlayerPrefs.GetInt(KEY_VOLUME, 80), 0, 100);
            volumeSlider.SetValueWithoutNotify(savedVol);
            OnVolumeChanged(savedVol); // ����/�ͼ� ����ȭ
        }

        // ��Ӵٿ� ���� + �̺�Ʈ
        if (screenModeDropdown)
        {
            EnsureScreenOptions();

            screenModeDropdown.onValueChanged.RemoveListener(OnScreenModeChanged);
            screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);

            int savedMode = Mathf.Clamp(PlayerPrefs.GetInt(KEY_MODE, 0), 0, 1);
            screenModeDropdown.SetValueWithoutNotify(savedMode);
            screenModeDropdown.RefreshShownValue();
            OnScreenModeChanged(savedMode); // ���� ��� ����
        }
    }

    // �����̴� �� ���� ��: ǥ��/�����/����
    public void OnVolumeChanged(float value)
    {
        int iv = Mathf.RoundToInt(value);
        if (volumeCurrentText) volumeCurrentText.text = iv.ToString();

        float linear = iv / 100f;

        if (masterMixer) // �ͼ� ���
        {
            float dB = (iv == 0) ? -80f : Mathf.Log10(Mathf.Max(0.0001f, linear)) * 20f;
            masterMixer.SetFloat(exposedParam, dB);
        }
        else             // ���� ���: ��ü ����
        {
            AudioListener.volume = linear; // 0~1
        }

        PlayerPrefs.SetInt(KEY_VOLUME, iv);
        PlayerPrefs.Save();
    }

    // ��Ӵٿ� �� ���� ��: ��� ����/����
    public void OnScreenModeChanged(int idx)
    {
        idx = Mathf.Clamp(idx, 0, 1);

        if (idx == 0) // â ���
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else          // ��ü ȭ��(��� ����)
        {
            var r = Screen.currentResolution;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(r.width, r.height, FullScreenMode.FullScreenWindow);
        }

#if UNITY_EDITOR
        Debug.Log($"[Settings] Requested: {(idx == 0 ? "Windowed" : "Fullscreen")}. " +
                  "Editor������ �ٸ��� ���� �� ����(����� Ȯ��).");
#endif
        PlayerPrefs.SetInt(KEY_MODE, idx);
        PlayerPrefs.Save();
    }

    // �ݱ� ��ư
    public void OnClickClose()
    {
        if (panelRoot) panelRoot.SetActive(false);
        // �ʿ��ϸ� �Ͻ����� ����:
        // Time.timeScale = 1f;
    }

    // ���� ���� ��ư
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
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("â ���"));
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("��ü ȭ��"));
        }
    }
}
