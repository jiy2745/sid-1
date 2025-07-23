using UnityEngine;
using UnityEngine.UIElements;

public struct DialogueData
{
    public string Name;
    public string[] Lines;

    public DialogueData(string name, string[] lines)
    {
        Name = name;
        Lines = lines;
    }
}

public class Dialogue : MonoBehaviour
{
    private Label _nameLabel;
    private Label _dialogueLabel;
    private int _currentDialogueIndex;

    private DialogueData _testDialogueData;
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _nameLabel = root.Q<Label>("NameLabel");
        _dialogueLabel = root.Q<Label>("DialogueLabel");
        _currentDialogueIndex = 0;
        _testDialogueData = new DialogueData
        (
            "홍길동",
            new string[]
            {
                "대화 테스트 11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                "대화 테스트 222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222",
                "대화 테스트 33333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333"
            }
        );

        _nameLabel.text = _testDialogueData.Name;
        _dialogueLabel.text = _testDialogueData.Lines[_currentDialogueIndex];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _currentDialogueIndex += 1;
            _dialogueLabel.text = _testDialogueData.Lines[_currentDialogueIndex];
        }

    }
}
