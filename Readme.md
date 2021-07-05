# < 개발 일지 >


## 명심하자!
항상 코드는 DRY하고 KISS하고 YAGNI하게..

DRY = Don't Repeat Yourself     - 특정 코드의 로직이 다양한 곳에서 반복되어 사용하지 마라.\
KISS = Keep In Simple, Stupid   - 코드는 항상 심플하고 멍청하게. 그리고 누구나 알아볼수 있게.\
YAGNI = You Ain't Gonna Need It - 지금 당장 필요없는 것을 미리 만들지 말자.


## 확인된 버그 & 개선이 필요한 것

## 다음 목표는..


필드를 넓게 만들지 말것, 되도록 작게 만들자.

포트폴리오는 시선을 끌 수 있어야한다!


하지만, 가장 중요한건 **컨텐츠를 만들 수 있는 개발 역량이다.**

컨텐츠적인 요소 vs 연출적인 요소
둘 중 하나를 골라 집중적으로 해보는 것이 좋다.

연출적인 요소를 잘 만들고 싶으면, 영상을 많이 보고 따라해보는 것이 중요
1. 애니메이션
2. 카메라


전반적인 RPG 게임 시스템은 다 만들어서 적용시키기
ex) 레벨업, 강화, 이펙트, 상점(구매,판매,강화,수리,제작 등), 퀘스트, 창고, 인터페이스 등 


--------------------------------

## Last Update




-------------------------------

### 개발 플로우 차트

개 X발 FlowChart


1. 플레이어 캐릭터 생성
	- 적절한 캐릭터 모델을 가져와 사용
	- 적절한 캐릭터 애니메이션을 BlendTree를 이용해 적용
	- PlayerController 스크립트를 작성 
	-> WASD로 이동, 마우스로 카메라 시야를 회전, 마우스 좌클릭으로 공격, 키보드 숫자키로 스킬

2. 몬스터 생성
	- 적절한 몬스터 모델을 가져와 사용
	- 적절한 몬스터 애니메이션을 가져와 적용
	- FSM을 활용한 몬스터 AI 작성

-------------------------------
## 이전 개발 일지


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