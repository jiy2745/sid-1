## How To Use
---
### Quickstart
1. 대화창을 만들고 싶은 오브젝트에 DialogueTrigger Component를 넣는다.

2. 각 대사를 어떤 경우에 출력하는 지 조건을 판단하는 DialogueSelector Component를 넣는다.

3. DialogueSelector의 Dialogues에 원하는 대화의 개수만큼 리스트에 추가한다.

4. 각 대화의 Dialogue Id에는 해당 대화의 대사 데이터 파일의 이름을 넣는다. (json 확장자 제외하고 이름만)

5. 각 대화의 대사 중에 추가적인 이벤트를 넣고 싶다면, Dialogue Lines를 우선 대사 데이터 파일 속 대사 갯수만큼 크기를 지정한 후, 해당하는 순서의 대사의 On Line Start() 칸에 이벤트를 추가해준다.


### 주의 사항
- 대사 파일은 Assets/Resources/Dialogues 내에 저장
- 반드시 하나의 오브젝트에 DialogueSelector 컴포넌트가 DialogueTrigger 컴포넌트와 같이 존재해야 함
- 만약 강제 이벤트 방식으로 대화창을 부르고 싶은 경우 (UnityEvent 등을 이용하여) DialogueManager의 StartDialogue 함수를 호출하면 됨

### DialogueSelector 클래스

NPC 또는 상호작용할 물체가 어떤 경우에 어떤 대화를 출력할 지 결정하는 스크립트. 추상 클래스로 구현해놨으므로, 별도의 로직이 필요하다면 상속 후 구현하여 컴포넌트로 추가 가능

**현재 구현해둔 클래스**
- NPCDialogueSelector  
기본적인 NPC 행동을 위한 클래스, Game Manager 클래스에 저장된 현재 날짜를 바탕으로 대사를 지정한다.