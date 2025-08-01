using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // (임시 변수) 나중에 다른 씬이랑 합쳤을 때 GameManager.cs에 있는 currentDay를 쓸 예정입니다.
    private int currentDay = 3;
    // (임시 변수) 나중에 다른 씬이랑 합쳤을 때 GameManager.cs에 있는 currentDay를 쓸 예정입니다.
    private int enlightenmentMeter = 70;

    // DAY - (숫자)가 써있는 텍스트
    public TextMeshProUGUI dayCounterText;
    // 계몽 수치 게이지
    public Image enlightenmentMeterImage;
    // 계몽 수치 퍼센트 텍스트
    public TextMeshProUGUI enlightenmentMeterText;

    public TextMeshProUGUI ItemDescriptionText;

    void OnEnable()
    {
        UpdateDayCounterText();
        UpdateEnlightenmentMeterImage();
        UpdateenlightenmentMeterText();
    }

    // UI에 일수 업데이트
    private void UpdateDayCounterText()
    {
        dayCounterText.text = "DAY - " + currentDay;
    }

    // 계몽 수치 바 업데이트
    private void UpdateEnlightenmentMeterImage()
    {
        enlightenmentMeterImage.fillAmount = enlightenmentMeter / 100f;
    }

    // 계몽 수치 퍼센트 업데이트
    private void UpdateenlightenmentMeterText()
    {
        enlightenmentMeterText.text = enlightenmentMeter + "%";
    }

    // 버튼에 따라 다른 텍스트를 보여주는 함수
    public void OnItemButtonClick(int buttonID)
    {
        string itemDescription = "";

        switch (buttonID)
        {
            case 0:
                itemDescription = "\'소환 마법 스크롤\' 이라고 적혀 있는 낡은 종이다. 딱 한 번, 이 종이를 찢으면 깡총거리는 짐승들에게 도움을 받을 수 있다고 쓰여있다.";
                break;
            case 1:
                itemDescription = "에리카에게서 받은 성냥개비다. 괴물에게 붙잡힐 것 같을 때 불을 피워 괴물을 쫓아낼 수 있을 것 같다.";
                break;
            case 2:
                itemDescription = "청소하다가 교실 바닥에서 주운 라이터다. 괴물에게 붙잡힐 것 같을 때 불을 피워 괴물을 쫓아낼 수 있을 것 같다.";
                break;
            case 3:
                itemDescription = "소방도구함에서 꺼낸 소방도끼다. 도끼날이 날카로워서 무언가를 부술 때 쓸 수 있을 것 같다.";
                break;
            case 4:
                itemDescription = "어떤 책에 적혀 있던 내용을 기반으로 알아낸 것을 기록한 메모이다. 이 정보를 활용하면 첨탑을 부술 수 있을 것 같다.";
                break;
            case 5:
                itemDescription = "에리카가 화학실에서 구해온 휘발유다. 불을 붙일 때 사용할 수 있을 것 같다.";
                break;
        }

        ItemDescriptionText.text = itemDescription;
    }
}
