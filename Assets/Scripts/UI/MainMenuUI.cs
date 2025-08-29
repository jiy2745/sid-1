using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEditor.EditorTools;

[System.Serializable]
public class SaveSlot
{
    public string slotName;
    public Button slotButton;
}

public class MainMenuUI : MonoBehaviour
{
    [Header("Debugging")]
    public bool debuggingMode = false;
    [Tooltip("The scene to load when starting a new game, change this for debugging purposes")]
    public string debugNewGameSceneName = "NightScene"; // The scene to load when starting a new game in debugging mode

    [Header("UI References")]
    public Animator animator;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitButton;

    [Header("Main Menu Settings")]
    public int saveSlotCount = 3;
    public List<SaveSlot> saveSlots = new List<SaveSlot>();
    [SerializeField] private string newGameSceneName = "Day_classroom"; // The scene to load when starting a new game

    void Start()
    {
        newGameButton.onClick.AddListener(StartNewGame);
        quitButton.onClick.AddListener(QuitGame);

        string latestSlotId = DataPersistenceManager.instance.GetMostRecentProfileId();
        if (latestSlotId == null)
        {
            continueButton.interactable = false; // Disable continue button if no save files exist
        }
        else continueButton.onClick.AddListener(()=>{ContinueGame(latestSlotId);});
    }

    private void InitSaveSlots()
    {
        // Get save data from DataPersistenceManager and update slot names accordingly
        List<string> profileIds = DataPersistenceManager.instance.GetAllProfileIds();
        // Update save slot buttons
        for (int i = 1; i <= saveSlotCount; i++)
        {
            SaveSlot slot = saveSlots[i - 1];
            string slotId = "slot" + i;
            bool hasSave = profileIds.Contains(slotId); // Whether this slot has a save file

            slot.slotButton.GetComponentInChildren<TMP_Text>().text = SaveSlotInfo(slotId, hasSave);
            slot.slotButton.onClick.AddListener(() => OnSlotSelected(slotId, hasSave));
        }
    }

    // Returns the display string for a save slot
    private string SaveSlotInfo(string slotId, bool hasSave)
    {
        string info = "";
        if (!hasSave)
        {
            return info + "빈 슬롯";
        }

        info += "세이브" + slotId.Substring(slotId.Length - 1);

        DataPersistenceManager.instance.ChangeSelectedProfileId(slotId);
        GameData data = DataPersistenceManager.instance.GetGameData();
        // TODO: Add timestamp and game info
        DateTime saveTime = DateTime.FromFileTime(data.timestamp);
        info += $"\n마지막 플레이: {saveTime:yyyy-MM-dd HH:mm}";
        return info;
    }

    private void OnSlotSelected(string slotId, bool hasSave)
    {
        DataPersistenceManager.instance.ChangeSelectedProfileId(slotId);

        string sceneToLoad;

        if (hasSave)
        {
            DataPersistenceManager.instance.LoadGame();
            sceneToLoad = DataPersistenceManager.instance.GetGameData().lastSceneName;
        }
        else
        {
            DataPersistenceManager.instance.NewGame();
            sceneToLoad = newGameSceneName; // New game starts at the classroom scene
        }

        if (debuggingMode)
        {
            sceneToLoad = debugNewGameSceneName; // Override for debugging
        }
        //SceneManager.LoadScene(sceneToLoad);
        SceneFade.LoadScene(sceneToLoad);
    }

    // ------ Methods for button actions ------
    public void StartNewGame()
    {
        SaveSlotPopup();
        newGameButton.interactable = false; // Prevent multiple clicks
        continueButton.interactable = false;
    }

    public void ContinueGame(string slotId)
    {
        newGameButton.interactable = false; // Prevent multiple clicks
        continueButton.interactable = false;
        //TODO: Load the last saved game directly without showing save slots (use timestamps to determine the most recent save)
        OnSlotSelected(slotId, true);
    }

    public void SaveSlotPopup()
    {
        InitSaveSlots();    // lazy initialization
        animator.Play("SaveSlotPopup");
    }

    public void SaveSlotPopdown()
    {
        animator.Play("SaveSlotPopdown");
        newGameButton.interactable = true;
        continueButton.interactable = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
