using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    private Button _mainScreenStartButton;
    private Button _mainScreenSettingButton;
    private Button _mainScreenExitButton;

    private void OnStartButtonClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnSettingButtonClicked(ClickEvent evt)
    {
        
    }

    private void OnExitButtonClicked(ClickEvent evt)
    {
        Application.Quit();
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _mainScreenStartButton = root.Q<Button>("StartButton");
        _mainScreenSettingButton = root.Q<Button>("SettingButton");
        _mainScreenExitButton = root.Q<Button>("ExitButton");

        _mainScreenStartButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        _mainScreenSettingButton.RegisterCallback<ClickEvent>(OnSettingButtonClicked);
        _mainScreenExitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
    }

    void OnDisable()
    {
        _mainScreenStartButton.UnregisterCallback<ClickEvent>(OnStartButtonClicked);
        _mainScreenSettingButton.UnregisterCallback<ClickEvent>(OnSettingButtonClicked);
        _mainScreenExitButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
    }
}
