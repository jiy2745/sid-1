변경사항* (추가된 스크립트는 모두 기존 main 브랜치 스크립트를 기반으로 참고 후 작성하였으며 주석을 달아놓았습니다.)

1. build profiles에서 Scene 4개: SampleScene, DayScene_classroom, DayScene_hallway, NightScene 추가. (DayScene 두 종류 제작)

2. TriggerArea 스크립트에 기반한 "Scene Loader" 스크립트 추가: 장소 이동에 사용됨(ex. 교실->복도)

3. 씬 이동시 플레이어가 사라지지 않도록 "DontDestroy" 스크립트 추가

4. 임시 플레이어 생성 (무료 에셋 사용)

5. 임시 교실과 복도 생성, 벽 생성(영역 안에 고정되도록)

6. 카메라 비율 수정

7. 교실 <-> 복도 이동 인터랙션 추가

8. 복도로 이동시에 플레이어가 사라지지 않고 특정 위치에서 시작하도록 "PlayerSpawner" 스크립트와 PlayerStartPoint 지점 추가
*PlayerStartPoint 는 알파값 최대라서 안보입니다. 

9. "PlayerMovement" 스크립트 수정 (중요!!) 기존 코드의 playerMovement.OnMinigameStart() 함수는 플레이어의 움직임을 멈추는데,
이건 미니게임용으로 의도된 코드일 뿐더러 플레이어가 씬이 바뀌는(교실->복도) 인터랙션을 추가한 후엔, 플레이어가 해당 함수로 인해 
멈춤 상태에 머물러있어서 새로운 씬에서 이동이 불가능함. 이에 PlayerMovement 스크립트를 수정하여 멈춤 상태를 해제하는, 
씬 이동시 스크립트를 켜고 끄는 역할을 명확하게 보여주는 함수 두개를 추가함.

10. 또한 기존 "TriggerArea" 스크립트에 두 줄을 추가하여, TriggerArea 스크립트를 사용하는 모든 오브젝트(문, 미니게임 트리거 등)의 인스펙터 
창에 Stop Player On Enter라는 체크박스를 만들었고, 이를 미니게임 트리거에선 체크박스를 키고, 문과 같은 컴포넌트에서는 체크박스를 끄는
방식으로 작업.