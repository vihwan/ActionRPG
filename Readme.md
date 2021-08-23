# < 개발 일지 >

다(이루)크 소울


## 명심하자!
항상 코드는 DRY하고 KISS하고 YAGNI하게..

DRY = Don't Repeat Yourself     - 특정 코드의 로직이 다양한 곳에서 반복되어 사용하지 마라.\
KISS = Keep In Simple, Stupid   - 코드는 항상 심플하고 멍청하게. 그리고 누구나 알아볼수 있게.\
YAGNI = You Ain't Gonna Need It - 지금 당장 필요없는 것을 미리 만들지 말자.


## 강사님 소스

https://drive.google.com/drive/u/0/folders/1RcwYCNc75_wKahdHDvJu-_-m22C2K0Ym


## 확인된 버그 & 개선이 필요한 것

 <버그>

- 점프 시 콜라이더가 모델을 따라오지 않는 버그. 이 경우는 애니메이션 문제일 가능성이 더 크다. 현재로써는 해결할 수 없는 부분.

- 일부 동작 종료 이후 애니메이션 동작이 끊키는 것 처럼 보임. 마치 CrossFade하지 않음. 가끔씩 이런 일이 발생하는데, 정확한 원인을 찾을 수가 없다.


 <개선이 필요한 점>

- 플레이어 모델이 스프린트를 끝마쳤을때, 브레이크를 거는 애니메이션을 넣고 싶었으나, 조건을 신경쓸게 많아보였고 그것을 정확히 어떻게 해야할 지를 몰라 실패하였다. 비록 안만들어도 게임 구성에는 전혀 문제가 없으나, 나중에 다시 재도전하여 꼭 넣고 싶다.


## 다음 목표는..

다른 무기 장착 및 애니메이션
인벤토리 제작
몬스터 AI

------------------------------------------------------
필드를 넓게 만들지 말것, 되도록 작게 만들자.

포트폴리오는 시선을 끌 수 있어야한다!


하지만, 가장 중요한건 **컨텐츠를 만들 수 있는 개발 역량이다.**

컨텐츠적인 요소 vs 연출적인 요소
둘 중 하나를 골라 집중적으로 해보는 것이 좋다.

연출적인 요소를 잘 만들고 싶으면, 영상을 많이 보고 따라해보는 것이 중요
1. 애니메이션
2. 카메라


전반적인 RPG 게임 시스템은 다 만들어서 적용시키기
ex) 레벨업, 강화, 이펙트, 상점(구매, 판매, 강화, 수리, 제작 등), 퀘스트, 창고, 인터페이스 등 


-------------------------------

**공격 애니메이션 이벤트 함수**

OpenRightHandDamageCollider\
CloseRightHandDamageCollider\
EnableCombo\
DisableCombo\

-------------------------------

**Input Handler Keys**

1. 이동 - WASD
2. 스킬 - 1,2,3,4
3. 포션 퀵슬롯 - Z
4. 약공격 - 마우스 왼클릭
5. 강공격 - 마우스 왼클릭 꾹 누르기
6. 달리기(스프린트) - Shift 꾹 누르기
7. 회피 - Shift 누르고 떼기
8. 점프 - Space
9. 막기(패링) - 마우스 우클릭
10. 백스텝 - 아무런 입력 없이 시프트
11. 무기교체 - X
12. 타겟 고정 - 마우스 휠버튼
13. 카메라이동 - 마우스 이동

------------------------------

**UI Fonts**

Resources 폴더에 Fonts 폴더 생성

한글 - 배민주아체
영어 - 노트산스체
숫자 - ...

--------------------------------

**Memo**

 <CharacterMenu 제작중>

 - StatusPanel => PlayerStats와 PlayerInventory와 연동
 - WeaponPanel => 
 			현재 장착중인 무기 - PlayerInventory의 rightWeapon과 연동
 			미장착 무기들 - _A의 WeaponInventory와 연동_

 - EquipmentPanel =>
 		장비 종류 - 상의, 하의, 장갑, 신발, 악세사리, 특수장비

 			현재 장착중인 장비 - PlayerInventory의 EquipmentSlot과 연동
 			미장착 장비들 - _A의 EquipmentInventory와 연동_

 - SkillPanel =>  ScriptableObject.PlayerSkill, SkillDatabase
 - DataPanel => 미정

> **Weapon과 나머지 Equipment들을 LeftPanel에서 보여주는 스크립트 작성 요망.**
    ItemType Struct를 생성하고 ScriptableObject에 설정할 수 있도록 추가
    PlayerInventory에 각 ItemType에 대한 List를 생성


-------------------------------

## Last Update

## 2021.08.23 (월)

< UI/UX 명세 확인 및 제작 >

1. CharacterMenu - WeaponPanel

	- (완료) 무기 장착 버튼으로 무기 교체하기

	weaponPanel 클래스의 changeWeaponBtn을 통해 무기 교체.

	**무기 교체 함수를 어떤 클래스가 담당해야하는가?**
	무기를 교체하면 많은 일들이 발생한다.
	1. playerInventory의 currentWeapon 변경 V
	2. playerStats를 통한 플레이어 스테이터스 변경 V
	3. right WeaponPanel에서 장착 무기로 변경 V
	4. left weaponInventoryList에서 장착 무기 표시 변경 V

	위의 경우들을 전부 참조하고 있는 클래스가 제일 적합할듯 싶다.


	- (완료) 장비 비교 버튼을 누르면 현재 장착한 무기와 장비 비교가 가능

		장착중이 아닌 무기를 선택할 경우, 장비비교 가능 버튼이 활성화. 누르면 새로운 페이지가 나타나면서, 장비비교가 가능하게 만든다.


	- (완료) 목록 정렬 기능을 Dropdown과 연동하여 추가할 것

		PlayerInventory에 가나다순, 레어도순으로 정렬하는 함수를 추가 생성

	System.Linq를 이용하여 문제를 해결하였다.
	WeaponInventoryList에 Dropdown을 추가. 가나다순, 레어도순으로 정렬하는 함수를 AddListener로 추가
	이후 PlayerInventory 클래스의 Sort메소드를 추가하여 weaponInventory를 Linq의 OrderBy함수를 이용하여 정렬
	정렬된 weaponInventory를 토대로 다시 WeaponInventoryList.UpdateUI 메소드를 실행하여 재정렬한다.

2. CharacterMenu - EquipmentPanel

	- 각 필요요소 패러미터 연결하기 (오른쪽 패널, 왼쪽 패널, 개별장비UI, 비교장비UI)

	- SetItem 클래스 재조정 

		세트아이템의 종류는 임시로 3가지 : 이아손, 종려, 향기
		각 세트아이템의 타입에 따라 세트 이름과 효과를 프로퍼티로 설정

	- SetTemplate 클래스 생성

		playerInventory의 currentEquipmentSlots의 SetType을 서로 비교하여, 일정 갯수 이상을 넘으면, SetTemplate를 생성하고, UI로 표시하도록 생성

	**CharacterUI_EquipmentPanel.CreateSetItemTemplate 부터 작성**


-------------------------------

### 개발 플로우 차트

개 X발 FlowChart

1. 플레이어 캐릭터 생성
	- 적절한 캐릭터 모델을 가져와 사용
	- 적절한 캐릭터 애니메이션을 BlendTree를 이용해 적용

2. 플레이어 컨트롤
	- PlayerController 스크립트를 작성
	- InputSystem을 이용하여 크로스 플랫폼 확장성까지 고려.
	- 이동, 스킬, 구르기, 달리기, 백스텝 등 여러 모션을 설정 및 구현

3. 플레이어 공격
	- 무기 모델에 콜라이더를 추가하고 OnTriggerEnter를 이용하여 접촉시 데미지를 주도록
	- 콤보 시스템 구현
	- 스킬 공격 구현

4. 플레이어 상호작용
	- 땅에 떨어진 물건, NPC와 대화하기 등 여러 상호작용을 고려


5. 플레이어 인벤토리
	- 장비, 소비아이템 임시 구현. 퀵슬롯 구현

6. 플레이어 화면 고정

7. 몬스터 생성
	- 적절한 몬스터 모델을 가져와 사용
	- 적절한 몬스터 애니메이션을 가져와 적용
	- FSM을 활용한 몬스터 AI 작성

-------------------------------
## 이전 개발 일지

### 2021.08.19 (목)

1. UI/UX 명세 확인 및 제작

	1) PlayerStatus

	- (완료) 현재 플레이어의 스테이터스를 전부 보여줄 수 있도록
	- (완료) 플레이어가 장착한 장비들에 따라 스테이터스가 갱신되도록

	2) WeaponInventory

	- (완료) 무기를 주으면, 플레이어 인벤토리에 무기가 추가되도록 
	- (완료) 무기 탭 창을 열면, 현재 착용하고 있는 무기의 스탯이 출력되도록
	  	ex)
		공격력      + 10
		방어력      + 20
		치명타      + 3%


	- (완료) 교체 버튼을 클릭하면 왼쪽에 weaponInventory를 받아와서 출력하는 Panel을 추가 => WeaponInventoryList :
	- (완료) 왼쪽 패널이 열려있는 상태에서, 뒤로가기 버튼을 누르면 패널이 닫히도록 추가 
	- (완료) 현재 장착중인 무기가 제일 왼쪽 위에 먼저 생성되도록, 나머지는 순서대로
	- (완료) WeaponItemSlotPrefab을 가져와 생성. 현재 장착중인 아이템, 선택중인 아이템 등을 구별가능하도록 다르게 표시
	- 무기 장착 버튼으로 무기 교체하기

	weaponPanel 클래스의 changeWeaponBtn을 통해 무기 교체.

	**무기 교체 함수를 어떤 클래스가 담당해야하는가?**
	무기를 교체하면 많은 일들이 발생한다.
	1. playerInventory의 currentWeapon 변경
	2. playerStats를 통한 플레이어 스테이터스 변경
	3. right WeaponPanel에서 장착 무기로 변경
	4. left weaponInventoryList에서 장착 무기 표시 변경

	위의 경우들을 전부 참조하고 있는 클래스가 제일 적합할듯 싶다.


	- 장비 비교 버튼을 누르면 현재 장착한 무기와 장비 비교가 가능

		장착중이 아닌 무기를 선택할 경우, 장비비교 가능 버튼이 활성화. 누르면 새로운 페이지가 나타나면서, 장비비교가 가능하게 만든다.

	- 목록 정렬 기능을 Dropdown과 연동하여 추가할 것

		weaponInventoryList에 가나다순, 레어도순으로 정렬하는 함수를 추가 생성



### 2021.08.18 (수)

1. UI/UX 명세 확인 및 제작

- Item이 가질 수 있는 Stats

	HP
	ATK
	DEF
	CRI
	CRI.DMG
	STA
	세트 효과

한 가지 크게 착각한 게 있는데 플레이어가 가져야할 총 집합 수치들은 PlayerStats에 적용되어야한다는 사실이다.
따라서 나중에 그러한 스탯들을 적용하고 업데이트 해주는 함수를 추가하는 것이 필요하다.

플레이어가 현재 장착하고 있는 무기,장비 슬롯도 만들어야할 필요가 있다. 그리고 장비한 아이템에 따라 플레이어의 스탯을 업데이트하는 것 또한 필요하다.

또한, 아이템마다 다양한 속성을 지정할 필요가 있다. 예를 들어, 무기가 꼭 방어력이 오르면 안된다는 법칙은 없다.
따라서 무기마다 아이템 속성을 개발자가 에디터에서 조절할 수 있도록 설정하는 것이 필요했다. => ItemAttribute

그리고, PlayerInventory에서 플레이어가 여러 아이템을 가지고 관리하도록 할 수 있게, 저장할 수 있는 공간이 필요하다
Weapon은 List로 된다고 하지만, Equipment나 나머지들은 어떻게 해야할지 아직 고민이 된다. Dictionary로 쓰면 좋긴 할텐데
좀 더 고려해봐야겠다

### 2021.08.17 (화)

1. UI/UX 명세 확인 및 제작

 <CharacterMenu 제작중>

 - StatusPanel => PlayerStats와 PlayerInventory와 연동
 - WeaponPanel => 
 			현재 장착중인 무기 - PlayerInventory의 rightWeapon과 연동
 			미장착 무기들 - _A의 WeaponInventory와 연동_

 - EquipmentPanel =>
 		장비 종류 - 상의, 하의, 장갑, 신발, 악세사리, 특수장비

 			현재 장착중인 장비 - PlayerInventory의 EquipmentSlot과 연동
 			미장착 장비들 - _A의 EquipmentInventory와 연동_

 - SkillPanel =>  ScriptableObject.PlayerSkill, SkillDatabase
 - DataPanel => 미정

> **Weapon과 나머지 Equipment들을 LeftPanel에서 보여주는 스크립트 작성 요망.**
    ItemType Struct를 생성하고 ScriptableObject에 설정할 수 있도록 추가
    PlayerInventory에 각 ItemType에 대한 List를 생성


2. TMP 한글 폰트 생성

	Resources 폴더에 Fonts 폴더 생성

	한글 - 배민주아체
	영어 - 노트산스체
	숫자 - ...


### 2021.08.16 (월)

1. UI/UX 명세 확인 및 제작

UI 프로토타입을 설계하고 스크립트 작성
캐릭터부터 순서대로

 - 캐릭터 UI 요소들 배치

 속성, 무기, 장비, 장비비교까지. 스킬과 자료는 추후 추가.

 SpringBoard 메뉴에 버튼들을 추가하고, 버튼을 클릭하면, 해당 버튼에 맞는 패널이 우측에서 생성되도록 설정
 SpringBoardButton은 Weapon까지밖에 설정하지 않음.
 이후 WeaponPanel과 EquipmentPanel에 적절한 GameObject Parameter를 추가하여 연동되도록 설정하기.


### 2021.08.14 (토)

<버그 수정 및 개선> 

1. 캐릭터 행동 - Equip Change

**<개선점>**

**캐릭터 모델 하위에 붙어 있는, 미장착 상태( 등에 붙어있는 ) 무기를 적절한 게임 오브젝트 계층에 
신규 스크립트를 작성하거나, 기존 스크립트에 새로운 파라미터를 추가해서 무기 모델을 Instantiate해주는 코드를 
작성한다.**

WeaponUnholderSlot 스크립트를 새로 작성하고, Spine에 추가.
Spine에 무기 오브젝트를 생성하고, 임의로 위치, 회전값을 정해주어 자연스럽게 만들었다.
하지만, 캐릭터가 미장착 상태에서, 구를 때, 무기도 같이 굴러진다는 것이 문제. 이것이 자연스러운걸까?
나중에 무기는 따로 안구르게 할거면, 무기의 생성 위치를 모델의 영향을 받지 않는 다른 상위 계층으로 옮겨야한다.


2. 캐릭터 이동 - 스프린팅 종료

캐릭터가 빠르게 달리고 InputHandler의 MoveAmount가 = 0 이 되면, 캐릭터가 서서히 멈춰서는 동작이 나오도록
설정.

- 무기를 착용안한 상태에서, 스프린트 도중에, 쉬프트키를 누르고 있지만, 방향키를 뗀 순간 실행한다.
	
		Shift를 누른다 : b.input = true / sprintFlag = true / isSprinting = true
		방향키를 뗀다 : input.MoveAmount != 1;
Override UnEquip만 고려하여 작성. 전투 중에 서서히 멈추는 요소는 오히려 게임플레이에 방해가 될 수 있기에 제외.
하지만 이러면 아무런 방향키 입력 없이 Shift를 누를 경우, SprintEnd가 실행된다. 추가적인 조건문이 필요하다.


**백스텝과 조건식이 서로 같아서 벌어지는 현상인듯. 추가적인 조건문이 필요하다.
이 부분은 꽤나 어렵다. 그래서 일단 신경쓰지 않기로 결정했다. 나중에 다시 재도전하기로.**


### 2021.08.12 (목)

1. 캐릭터 행동 - Equip Change

* 상호작용하지 않으며, 비전투 상태일 경우에, 자동으로 무기를 집어넣는 상태로 전환하게 만든다.

무기를 들고 있는 상태에서 공격 플래그가 켜지지 않으면, 무기를 집어넣는 상태가 된다.
무기가 집어넣어져 있는 상태에서 공격 플래그가 켜지면 즉시 공격 애니메이션이 나가도록 설정한다.
무기를 집어넣고 꺼내는 동작은 Interacting이 아니도록 설정 (임시).

달리는 도중에도 무기를 집어넣는 경우가 있기 때문에, 새로운 Both Hand 애니메이터 Layer를 생성.
InputHandler의 MoveAmount가 0을 넘어가면 Both Hand Layer의 애니메이션 스테이트를 실행

Base Layer UnEquip을 새로 생성. UnEquip상태일 경우에 동작하는 신규 Locomotion을 등록.
Locomotion에 1D와 2D freeform 애니메이션을 추가.

UnEquip Override Layer를 새로 생성. player Flag의 isUnEquip이 true일 경우에 따로 실행되는
애니메이션 클립들을 새로 등록하고, 조건문도 따로 작성.


**<개선점>**

**캐릭터 모델 하위에 붙어 있는, 미장착 상태( 등에 붙어있는 ) 무기를 적절한 게임 오브젝트 계층에 
신규 스크립트를 작성하거나, 기존 스크립트에 새로운 파라미터를 추가해서 무기 모델을 Instantiate해주는 코드를 작성한다.**


2. 캐릭터 이동 - 스프린팅 종료

캐릭터가 빠르게 달리고 InputHandler의 MoveAmount가 = 0 이 되면, 캐릭터가 서서히 멈춰서는 동작이 나오도록
설정.

- 무기를 착용안한 상태에서, 스프린트 도중에, 쉬프트키를 누르고 있지만, 방향키를 뗀 순간 실행한다.
	
		Shift를 누른다 : b.input = true / sprintFlag = true / isSprinting = true
		방향키를 뗀다 : input.MoveAmount != 1;
Override UnEquip만 고려하여 작성. 전투 중에 서서히 멈추는 요소는 오히려 게임플레이에 방해가 될 수 있기에 제외.

**하지만 이러면 아무런 방향키 입력 없이 Shift를 누를 경우, SprintEnd가 실행된다. 추가적인 조건문이 필요하다.**


### 2021.08.11 (수)

1. < UI/UX 개발 계획 및 명세 작성 >

ESC를 눌러 메인메뉴 창을 연다.

(메인 메뉴 창 목록)

	1. 캐릭터
	2. 가방
	3. 월드맵
	4. 퀘스트
	5. 업적
	6. 설정
	7. 스크린샷 찍기
	8. 게임종료



2. 2Handed Flag

한손잡이를 양손잡으로 바꾸는 부분

InputHandler에 bool y_Input, bool twoHandFlag를 추가
HandleTwoHandInput() 메소드를 추가
이후 WeaponSlotManager에서 Animator.Crossfade 함수로 애니메이션 호출
Both Arms라는 새로운 Animation Layer를 생성. 이에 해당하는 Avatar Mask를 만들고 적용
(ItemAsset)무기 패러미터로 Two Handed Idle 애니메이션 String을 추가


내가 만들고싶은 게임에는 굳이 이 기능은 필요없을 듯 싶다. 대신에 이를 응용해서
무기를 장착안하고 있는 Unarmed 상태를 만들 수 있을 듯 하다.


### 2021.08.10 (화)

1. UI - 인벤토리 시스템 

메뉴 불러오기 단축키 - ESC => Control에 키 등록 및 InputHandler에 추가

ESC를 누르면, 선택할 수 있는 메뉴 UI가 나오고, 다시 누르면 사라지는 방식
메뉴 UI안에는 우선, 장비창, 가방, 설정을 넣어둠
기본 베이스 스크립트만 작성하고, 나중에 UI/UX 설계를 끝마친 뒤에 추가적으로 작업할 예정.


2. 배경 스테이지 구매.

원신 모델링들과 어울리는 적절한 던전 스테이지를 찾아서 구매.
FANTASTIC - Dungeon Pack

3. Camera Jitter Fix

캐릭터가 움직일 때 마다 카메라가 캐릭터를 따라다니면서, 미세하게 흔들리는 부분을 좀 더 부드럽게 만들도록 수정하였다.

4. 캐릭터 조작 - 락 온

카메라 락온 - 마우스 휠버튼
락온 대상 변경 - Q, E

플레이어 범위 내에 락온이 가능한 객체를 전부 검색하고, 가장 가까운 객체를 찾은 뒤에 그 객체를 락온하는 방식
객체를 락온 할 때, 카메라의 Y축 회전은 0으로 고정시키도록 설정.

LockOn 할 경우, 카메라의 Pivot의 높낮이가 SmoothDamp를 이용하여 변경되도록 설정.

5. 캐릭터 이동 - 8방향 Locomotion 추가

걷기, 달리기 8방향 Locomotion Animation을 추가.
BlendTree를 2D Freeform Directional로 변경하고, LockOn 상태일 경우에만 2D Freeform으로 적용되어 움직일 수 있도록 조건문을 설정


현재 락온 상태에서 타겟을 변경할 때, 제대로 바뀌지 않는 현상이 있음.
<확인해 볼 스크립트>
	1. PlayerControls V
	2. InputHandler V
	3. CameraHandler
CameraHandler에서 Available Targets List를 맨 처음에 초기화 되지 않아 List에 계속 객체들이 추가되고 있었기 때문에 계산에 수가 틀린 듯 하다. 


### 2021.08.08 (일)


1. 플레이어 상호작용

어제 만든 것을 보고 느낀 점이 있다면, 상호작용 할 수 있는 대상의 콜라이더 영역을 지정을 잘 해야한다는 점

2. 점프 시 따라오지 않는 콜라이더

이것은 계속 고민해봤는데, 기본적으로 애니메이션은 모델의 mesh를 움직이게 하는 것이지, 콜라이더를 움직이게 하는 것이 아니다.
따라서 콜라이더를 움직이게 하려면 단순히 리지드바디에 힘을 가하면 된다고 생각했다.
그러나 힘을 가해도 무언가에 의해 y방향의 속도를 0으로 초기화시킨다는 것을 인지했다.
그것이 무엇인지는 아직 찾을 수 없었다. 그러나 역동적인 게임을 만들기 위해서는 반드시 해결해야하는 문제.
누군가에게는 루트모션의 애니메이션이 문제라고 설명했다. 정말인가?


### 2021.08.07 (토)

1. Input Handler 설정 변경

약공격 - 마우스 왼클릭
스프린트 - Shift 꾹 누르기
백스텝 - 방향키 입력 없이 Shift
구르기 - Shift 눌렀다 떼기

2. 스킬 아이콘 업데이트

QuickSlotUI에 UpdateSkillSlotUI를 추가 - 스킬 아이콘 이미지 갱신

3. 캐릭터 공격 - 스킬 공격 추가

2,3,4 번키를 눌러 각각 스킬 및 궁극기가 나가도록 추가

4. 플레이어 스킬 쿨타임 연동

PlayerSkill 클래스에 쿨타임 패러미터를 새로 생성. 
이후, 각 스킬 버튼에 맞게 쿨타임을 세팅

5. 캐릭터 상호작용

Interactable 클래스를 생성. 나중에 인물, 사물 등과 상호작용 할 수 있게.
Interactable을 상속받는 WeaponPickUp 클래스를 생성
GUIManager의 하위 오브젝트로 InteractableUI를 생성

PlayerManager를 통해 상호작용할 수 있는 대상이 있는지를 검사.

여기서...매우 고민이 되는 부분이 있다. 검사 방법을 어떻게 할 것인가?

	1. Physics.SphereCast 같은 것을 이용해서 범위 내의 대상을 탐색하고 조건문 조사
	2. OnTriggerEnter,Exit을 이용하여 조사.

현재 내 생각에는 OnTriggerEnter 방식으로 가는것이 좋다고 생각한다. 
그 이유는 현재 2가지

	1. Update문으로 SphereCast를 계속 불러오면, 함수 호출 횟수가 증가하여 성능에 영향을 미칠 것 같음.
	2. 만약 아이템이 서로 붙어있는 경우, SphereCast 같은 것으로 아이템을 구별하여 줍기 힘들 것이다.

밑의 글은 인터넷 서핑으로 찾은 글.

>	전자는 필드 오브젝트 수와 함수호출 횟수가 성능에 영향을 미칩니다. 후자는 유니티의 Physics 루틴에 의해 프레임마다 실행되며 전체적인 부분중 극소량의 시간을 할애하여 체크를 합니다. 이 경우 콜라이더수가 성능에 영향을 미칩니다. 오브젝트 수와 함수 호출 횟수가 극히 낮다면 전자가 더 깔끔한 코딩 스타일이라고 생각되며 이가 아닐 시 후자의 방법을 택하는 것이 좋다고 생각됩니다.
	(병목현상을 발생 시키는 것보다 전체적인 프레임이 살짝 떨어지는 것이 더 바람직하다고 사료됨.)


6. 캐릭터 이동 - 점프

현재 점프하면 콜라이더가 같이 움직이지 않음 + 점프모션이 끝나면 콜라이더가 기울어짐

힘이 제대로 가해지지 않는 것 같다. Velocity의 Y가 순식간에 0으로 고정이 되어서 콜라이더가 움직이지 않는 듯 하다.


Bug Fix - 아무런 이동방향 없이 구르기 버튼을 눌렀을 경우, 실행되지 않고 오류가 발생하는 부분을 수정.


### 2021.08.06 (금)

1. 캐릭터 공격 - 콤보 공격

WeaponItem 에셋 메뉴에서 콤보 공격에 해당하는 애니메이션 클립 이름을 등록
PlayerAttacker 클래스에서, 약공격 혹은 강공격을 시작할 때, 해당 클립 이름을 저장하고, 그 클립 이름에 따라
다음 콤보 공격이 이루어 질 수 있게끔 만들었다.

나중에 콤보공격이 많아지면 이 부분을 한번 리팩토링할 필요성이 있다.


2. 캐릭터 애니메이션 교체

쌍검 애니메이션 => 양손검 애니메이션

Locomotion, 구르기, 회피, 공격 등


3. 퀵 슬롯 - 스킬 아이콘

임시 스킬 아이콘 UI를 생성하고, 스킬을 업데이트 할 수 있도록 설정
PlayerSkill 이라는 클래스를 생성
스킬 아이콘을 생성하고 SkillBtn 스크립트를 추가. 
클릭 시, 스킬 사용 및 쿨타임 표시 등 전반적인 스킬 기능을 관리한다.
SkillTimer 스크립트를 추가하여 스킬 쿨타임 동안 텍스트 애니메이션이 실행되게끔 만들었다.

4. 캐릭터 공격 - 스킬 공격

<꽤나 복잡한 과정 요함>

	1. 스킬 애니메이션을 선정 및 추가
	2. InputHandler에 스킬 버튼을 추가
	3. 스킬 버튼을 입력 시, 스킬 애니메이션이 재생
	4. 스킬 애니메이션이 재생되는 순간, 스킬 아이콘의 이벤트가 액션
	5. 스킬 공격 타이밍마다 타격 애니메이션 이벤트(콜라이더)를 추가


### 2021.08.05 (목)

08.01 ~ 08.05

게임 컨셉을 다(이루)크 소울로 방향을 명확하게 정했다.


1. 다이루크 캐릭터 모델링 업데이트

2. 아이템 클래스 생성

아이템 클래스를 상속받는 Weapon Item 클래스를 생성
이를 CreateAssetMenu로 무기 아이템을 쉽게 생성할 수 있도록 만들었다.

3. 무기를 쥐고 있는 손 슬롯 - Weapon Holder Slot
모델의 왼손 및 오른손 Transform에 직접 붙어있는 스크립트
이 스크립트를 통해 원하는 무기를 생성하거나 파괴한다.

4. 무기 슬롯 매니저 - Weapon Slot Manager
모델에 붙어있는 Weapon Holder Slot 스크립트들을 전부 찾은 뒤,
무기 아이템 모델을 생성하게 해주는 관리 스크립트

5. 캐릭터 공격 - 공격 애니메이션

E키로 약공격, R키로 강공격이 되도록 설정

6. 캐릭터 행동 - 데미지 입음

플레이어가 데미지를 입을 경우, 체력이 깎이는 임시 UI를 만들었음.
데미지를 입으면 데미지를 받는 애니메이션이 실행. 체력이 0이 되면 죽는 애니메이션 실행.

7. 캐릭터 공격 - 데미지 주기

플레이어가 데미지를 주는 스크립트를 작성
플레이어가 들고 다닐 무기에 컬라이더를 넣는다. 이때 isTrigger는 키는 방식을 택했다.
DamageCollider 스크립트를 만들어 넣어, 공격 애니메이션 동작 중 컬라이더가 켜지고 끄게 하여
타격 대상을 OnTriggerEnter로 확인하여 데미지를 주는 스크립트로 만들었다.


Bug Fix. 일반적인 고도에서 떨어질 경우, 땅 위에 있어도 계속 떨어지는 애니메이션을 수정.



### 2021.08.01 (일)

방향성을 명확하게 잡을 필요가 있다. 소울 라이크 게임으로 명확하게 게임 방향성을 잡고 다시 시작하자.

1. 캐릭터 이동 - 상,하,좌,우

PackageManager의 InputSystem을 활용해서 WASD키로 캐릭터가 이동하게끔 만들었다.
Animator의 BlendTree를 활용하여 Locomotion을 제작.

2. 카메라 이동

플레이어 캐릭터를 따라 추적하는 CameraHandler 스크립트를 제작.

3. 캐릭터 이동 - 구르기

InputSystem에서 ActionMaps에 Player Actions를 추가.
PlayerLocomotion 스크립트에 RollingAndSprinting 메소드를 추가하여 회피할 때, 애니메이션 실행 및
카메라 위치와 RootMotion이 실행될 수 있도록 설정

다만 아무런 이동방향 없이 구르기 버튼을 눌렀을 경우, 실행되지 않고 오류가 발생.

4. 캐릭터 이동 - 달리기

5. 캐릭터 이동 - 백스텝

6. 캐릭터 이동 - 중력 보정 떨어짐.

Raycast를 사용해서 캐릭터 아래방향으로 쏴서 검사하는 방식
공중에 있는 상태에서는 떨어지는 애니메이션을 계속 유지하며 떨어지며 그 동안에, 아무런 동작을 실행할 수 없다.


### 2021.07.26 (월)

1. 공격키가 연속으로 입력되지 않는 현상

-> 공격 시 EnableRM이 켜져있기 때문에 Update문이 돌아가지 않았음.

이를 PlayerMove Status로 제어하려니, 키 입력이 아무것도 없을 경우, IDLE로 설정하는 함수 때문에
먹히지를 않는다.

키 입력부터 다시 설정을 조절할 필요가 있음.



### 2021.07.14 (수) ~ 2021.07.25(일)

C# 프로그래밍 개별 공부 및 게임 설계


### 2021.07.13 (화)

1. 캐릭터 모션 - 공격처리

무기가 꺼내져있는 상태에서
* Idle,Walk,Jog 상태에서 공격시 - 공격 콤보
* Run 상태에서 공격시, 대시 공격
* 스킬 버튼을 누르면, 스킬 공격

### 2021.07.09 (금)

1. Root Motion이 적용되니 캡슐콜라이더가 캐릭터랑 같이 이동되지를 않아 문제가 생김 -> Character Controller를 사용

### 2021.07.08 (목)

1. 캐릭터 이동 - 다방면 회피

Animator에 Parameter를 새로 만들고 조건식을 하나 더 작성해 해결
아무런 추가 방향입력 없이 회피할 경우 뒤로 회피. 그렇지 않을 경우 그 방향으로 회피

2. 캐릭터 이동 - 중력 적용

CharacterController의 IsGrounded가 False라면 중력 계수를 받아 Move함수를 사용해 아래 방향으로 중력을 받게끔 적용.

3. 캐릭터 모션 - IK Foot

진행중..

<문제 발생>

1. 캐릭터 달릴 때 부들부들 떨림- RigidBody, CapsuleCollider 로 다시 교체 (왜 character 컨트롤러로 바꿨더라?)
2. 캐릭터가 회피할 때, 캐릭터 포지션의 이동이 부자연스러움.

### 2021.07.07 (수)

Soul-Like Character Controller에 대해 공부중...


1. 캐릭터 이동

캐릭터의 기본 이동은 RigidBody를 이동시켜서 캐릭터가 움직이도록 했고
회피 모션, 그리고 앞으로 추가할 공격, 경직, 쓰러짐 등은 Root Motion으로 처리하도록 할 예정이다.
그런데, 이런 식으로 하려니 CapsuleCollider와 RigidBody가 말썽이고 캐릭터 이동 자체도 제대로 이루어지지 않아 CharacterController로 바꿔보니 매우 잘 동작한다. 왜일까?

아무튼 그래서 문제를 해결하기는 했다.

회피 모션은 가만히 생각해보니 PC에서 8방면으로 회피를 하는 것은 굉장히 조작이 어렵다.
회피를 쉽게 하기 위해서 회피키를 마우스 우클릭으로 할 지, 아니면 그냥 왼쪽 Shift로 할지는
좀 더 고민해봐야겠다.



### 2021.07.02 (금)

1. 캐릭터 이동 애니메이션
	
	* 점프, 회피 등을 생각하면 역시 Root Transform을 사용해야하는지,
	나중에 크로스 플랫폼을 고려한다면 역시 캐릭터 동작 방식을 다르게 해야하는지 아이디어 고려중.



### 2021.06.30 (수)

1. 캐릭터 이동 애니메이션
	
	* 키를 연속 두번 누르면 달리기를 하도록 설정



### 2021.06.29 (화)

1. 캐릭터 이동 애니메이션

	* 이동 시, 캐릭터가 부들부들거림. CharacterController를 끄니까 이러한 문제가 없어짐
	* 실행 시, 처음에 IK가 잘 적용되나, 움직인 이후 IK가 적용이 안됨.

	> 1D로 바꾸니까 이러한 현상이 없어졌는데, 왜 그런지 도통 이해를 하지 못하겠음.

	* Foot IK를 이용하여 자연스러운 애니메이션 움직임을 사용

	* 기본 이동은 걷는 모션으로, 방향키 두번을 연속으로 누르면 달리기로 바꾸는방식?

	> 더블키 입력을 인식하는 시간이 대략 0.3초라고 한다. 그래서 안전하게 0.45 ~ 0.75f 정도의 간격을 주고 입력을 받아오도록 하겠다.

	DoublePressKeyDetection 클래스 작성


### 2021.06.28 (월)

1. 캐릭터 이동 - 3인칭 캐릭터 이동을 구현

	* Third Person Movement를 구현
	* 기본적인 동작 과정은 Cinemachine FreeCamera와 흡사하게 만들었다.
	


### 2021.06.27 (일)

1. 캐릭터 스크립트 설계

	* Information (status)
	* Controller
	* Model 
	NavMeshAgent 의 값을 전달하기 위한 용도로 사용
	하위 계층에 있는 애니메이터의 값을 전달하기 위한 용도로 사용
	* FSM (monster)
	몬스터의 유한 상태 머신을 관리하기 위함으로 사용
	* Animator 

### 2021.06.07 (월)

1. 캐릭터 모델 - 원신 (여행자 남)
2. 사용할 무기 - 카타나
3. 사용할 애니메이션 - 
Gruzzam powerful Sword Animation - Katana , Frank Slash


### 2021.06.02 (수)


1. 프로토타이핑 기획 설계


주어진 애니메이션의 파일에 따라 캐릭터 컨셉이 달라질듯

- ExplosiveLLC
	검, 도끼, 활, 석궁, 창
- Grruzam Powerful Sword
	대검, 카타나
- Frank Slash Pack
	양손검, 어쌔신, 쌍검, 대검, 카타나, 창




### 2021.05.31 (월)


1. 개발 세팅 

<필요한 에셋 및 패키지들>

	1. Frank Slash Pack
	2. ExplosiveLLC
	3. 500skillIcon
	4. Grruzam Powerful Sword Animation(Great Sword, Katana)
	5. MMD4Mecanim_Beta
	6. GUI PRO Kit => 용량이 너무 커서 이미지만 따로 가져올 예정
	7. Dynamic Bones
	8. FantasySpellsEffectsPack ▶
	9. PoiyomiToon Shader
	10. Gamemaster Audio - Pro Sound Collection ▶
	11. PolygonFantasyRivals ▶
	12. Polygonal Creatures Pack ▶
	13. Mini Legion Warband HP ▶
	14. BitGem ▶

	(고려할 것)
	1. Bullet Physics
	2. Quick Outline 
	3. UI Particles
	4. Tiny Dungeon

	추후 추가 예정


<사용할 3D 모델>

	1. 원신 > 여행자 남
	2. 소울워커 > 하루 에스티아
	3. 킹스레이드 > Rigging 상태가 다른 모델들과 달라서 보류
	4. 유니티쨩
	5. Vocaloid (하츠네미쿠)
	여유가 되면 추가 예정