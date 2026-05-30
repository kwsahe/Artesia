# Artesia

> 우주를 배경으로 한 로그라이크 턴제 던전 RPG
> Unity 2D 팀 프로젝트 (2024)

---

## 소개

Artesia는 매번 다르게 생성되는 던전을 탐험하며 미지의 비밀을 파헤치는 2D 로그라이크 RPG입니다.
이상한 던전 시리즈의 턴제 던전 탐험 방식에 서브컬처 감성을 녹여 독창적인 플레이 경험을 제공합니다.

---

## 스크린샷

<!-- 스크린샷 또는 GIF 삽입 -->
| 던전 탐험 | 전투 화면 |
|:---------:|:---------:|
| | |

---

## 핵심 기능

- **절차적 던전 생성** — BSP 알고리즘으로 매 플레이마다 새로운 맵 구조 생성
- **턴제 전투 시스템** — 이동, 공격, 스킬, 아이템 사용을 조합한 전략적 전투
- **속성 시스템** — 캐릭터·적·아이템의 속성 조합에 따른 데미지 계산
- **AI 적 행동** — A* Pathfinding 기반의 적 추적 및 행동 패턴
- **8방향 이동** — 그리드 맵 위에서 자유로운 이동 및 스킬을 통한 이동 범위 확장

---

## 기술 스택

| 분야 | 기술 |
|------|------|
| 엔진 | Unity 2D |
| 언어 | C# |
| 맵 생성 | BSP (Binary Space Partitioning) Algorithm |
| 경로 탐색 | A* Pathfinding |

---

## 프로젝트 구조

```
Assets/
├── 00.Scenes/                  # 씬 파일
│   ├── MainScene.unity
│   ├── DungeonSample.unity
│   └── BaseCamp.unity
│
├── 01.Scripts/
│   ├── Core/                   # 인터페이스 및 상태 머신
│   │   ├── IAbility.cs
│   │   ├── IDamageable.cs
│   │   ├── IState.cs
│   │   ├── ITurn.cs
│   │   └── StateMachine.cs
│   ├── Player/                 # 플레이어 상태 및 행동
│   │   ├── PlayerController.cs
│   │   ├── PlayerMove.cs
│   │   ├── PlayerIdle.cs
│   │   ├── PlayerAtk.cs
│   │   ├── PlayerStat.cs
│   │   ├── PlayerSkill.cs
│   │   └── PlayerCollider.cs
│   ├── Monster/                # 몬스터 AI 및 행동
│   │   ├── MobController.cs
│   │   ├── MobMove.cs
│   │   ├── MobIdle.cs
│   │   ├── MobAtk.cs
│   │   ├── MobStat.cs
│   │   └── EnemySpawner.cs
│   ├── Dungeon/                # 맵 생성 및 경로 탐색
│   │   ├── MapGenerator.cs
│   │   ├── DrawTile.cs
│   │   ├── AStarPathfinder.cs
│   │   └── Node.cs
│   ├── Item/                   # 아이템 및 스포너
│   │   ├── PotionStat.cs
│   │   └── ItemSpawner.cs
│   ├── Interaction/            # 맵 오브젝트 상호작용
│   │   ├── PortalCollider.cs
│   │   └── StairCollider.cs
│   ├── UI/                     # HUD 및 UI 컴포넌트
│   │   ├── StatusUI.cs
│   │   ├── DmgText.cs
│   │   ├── MinimapOnPlayer.cs
│   │   ├── CanvasStateListener.cs
│   │   ├── KeySettingSwitch.cs
│   │   ├── CameraController.cs
│   │   └── Btn/                # 씬 전환·메뉴 버튼
│   │       ├── BackBtn.cs
│   │       ├── BaseCampBtn.cs
│   │       ├── MainMenuButton.cs
│   │       ├── NextStageButton.cs
│   │       ├── OptionBtn.cs
│   │       ├── SaveDataBtn.cs
│   │       └── SelectDunBtn.cs
│   ├── Data/                   # 데이터 모델
│   │   ├── Data.cs
│   │   └── CharacterInformation.cs
│   ├── Manager/                # 싱글톤 매니저
│   │   ├── GameManager.cs
│   │   ├── BattleManager.cs
│   │   ├── TurnManager.cs
│   │   ├── UIManager.cs
│   │   ├── DataManager.cs
│   │   └── SceneLoader.cs
│   └── Shader/
│       └── SpriteOutline.cs
│
├── 03.Tilemaps/                # 타일맵 및 룰 타일
├── Animation/                  # 애니메이터 컨트롤러 및 애니메이션 클립
├── Resources/                  # 런타임 로드 리소스
│   ├── Prefabs/
│   ├── Player/
│   ├── Monster/
│   └── Image/
└── InputSystem/                # Unity Input System 액션
```

---

## 팀 구성

| 이름 | 역할 |
|------|------|
| 권상헌 | Game Director |
| 송수민 | Game Programmer |
| 고가영 | Game Designer |
| 김상진 | Game Designer |
| 이서연 | Game Designer |

---

## 개발 기간

2024년 3월 — 2024년 6월

---

## 레퍼런스

- 이상한 던전 시리즈 (不思議のダンジョン) — 포켓몬스터 불가사의 던전
- 원신, 붕괴: 스타레일, 블루 아카이브 — 서브컬처 비주얼 방향성
