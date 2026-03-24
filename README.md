# ✨lumos-library-core
유니티 기반 & 에셋에서 자주 사용되는 요소, <br>
또는 확장하는 기능의 스크립트를 모아놓아 개발의 생산성 상승을 위한 유니티 패키지

<br>

## ℹ️의존성

* **URP**
* **Newtonsoft.Json (자동 설치)**
* [ UniTask ](https://github.com/Cysharp/UniTask?tab=readme-ov-file#upm-package)
* [ Tri-Inspector ](https://github.com/codewriter-packages/Tri-Inspector?tab=readme-ov-file)

<br>

## ℹ️확장
* [ lumos-DOTween ](https://github.com/lumos5934/lumos-DOTween)
* [ lumos-firebase ](https://github.com/lumos5934/lumos-firebase)
* [ lumos-BGDatabase ](https://github.com/lumos5934/lumos-BGDatabase)

<br>

## ℹ️기능

* [ Audio ](#Audio)
* [ EventBus ](#EventBus)
* [ FSM ](#FSM)
* [ Services ](#Services)
* [ Input ](#Input)
* [ PreInitialize ](#PreInitialize)
* [ Pool ](https://www.notion.so/Pool-2df3966a742c8066b614c44f459abde8?source=copy_link)
* [ Resource ](https://www.notion.so/Resource-2df3966a742c80bbbad3d8fbf0bd24d2?source=copy_link)
* [ Save ](https://www.notion.so/Save-2df3966a742c80898b8ad7cd3d16f9ec?source=copy_link)
* [ Tutorial ](https://www.notion.so/Tutorial-2df3966a742c808b860be13cf2e99a08?source=copy_link)
* [ TestWindow ](https://www.notion.so/Test-Editor-2df3966a742c80c3af6ac96904d157da?source=copy_link)
* [ Popup ](https://www.notion.so/UI-2df3966a742c80f38990cbc42a6d1b49?source=copy_link)

<br>
<br>

---

### Audio

**AudioManager** <br>
`Create / [LumosLib] / Prefabs / Manager / Audio`

오디오 통합 관리자 <br>

<table>
  <tr>
    <td><b>Mixer</b></td>
    <td>사용할 오디오 믹서</td>
  </tr>
  <tr>
    <td><b>AudioPlayer</b></td>
    <td>재생에 사용되는 프리팹</td>
  </tr>
   <tr>
    <td><b>SetVolume()</b></td>
    <td>사용되는 믹서 볼륨 조절</td>
  </tr>
   <tr>
    <td><b>BGM</b></td>
    <td>`bgmType`별 독립 채널 관리 (전투, 환경음 등)</td>
  </tr>
   <tr>
    <td><b>SFX</b></td>
    <td>추적이 필요 없는 단발성 효과음</td>
  </tr>
</table>



<br>

**AudioPlayer** <br>
`Create / [LumosLib] / Prefabs / Audio Player`

매니저를 통해 관리되는 오디오 플레이어


<br>

**Sound Asset** <br>
`Create / [LumosLib] / Scriptable Objects / SoundAsset`

사용되는 사운드 보관 SO

<table>
  <tr>
    <td><b>MixerGroup</b></td>
    <td>사용될 믹서그룹</td>
  </tr>
  <tr>
    <td><b>Clip</b></td>
    <td>사용될 오디오 클립</td>
  </tr>
   <tr>
    <td><b>VolumeMult</b></td>
    <td>볼륨 가중치</td>
  </tr>
   <tr>
    <td><b>IsLoop</b></td>
    <td>반복 여부</td>
</table>

<br>

[🎬튜토리얼](https://youtu.be/h66xEmaztBA?si=_H5PhyZfN-9ZT5Gh)


<br>
<br>

---

### EventBus
객체 간의 결합도를 낮추기 위한 전역 이벤트 시스템, 반드시 적절히 Unsubscribe를 호출하여 메모리 누수 방지

<table>
  <tr>
    <td><b>Subscribe()<b></td>
    <td>이벤트 등록</td>
  </tr>
  <tr>
    <td><b>Unsubscribe()<b></td>
    <td>이벤트 해제</td>
  </tr>
  <tr>
    <td><b>Publish()<b></td>
    <td>이벤트 발행</td>
  </tr>
</table>

```csharp
EventBus<LevelUpEvent>.Subscribe(OnLevelUp);
EventBus<LevelUpEvent>.Publish(new LevelUpEvent { Level = 10 });
```

<br>
<br>

---

### FSM

**StateMachine**

<table>
  <tr>
    <td><b>CurState<b></td>
    <td>현재 상태</td>
  </tr>
  <tr>
    <td><b>OnExit<b></td>
    <td>상태 Exit 콜백</td>
  </tr>
  <tr>
    <td><b>OnEnter<b></td>
    <td>상태 Enter 콜백</td>
  </tr>
  <tr>
    <td><b>Register(IState state)<b></td>
    <td>상태 등록</td>
  </tr>
  <tr>
    <td><b>Update()<b></td>
    <td>현재 상태 업데이트</td>
  </tr>
  <tr>
    <td><b>ChangeState()<b></td>
    <td>상태 변경</td>
  </tr>
</table>

```csharp
public class PlayerController : MonoBehaviour
{
    private StateMachine _stateMachine = new StateMachine();

    private void Start()
    {
        // 1. 상태 등록 (Register)
        _stateMachine.Register(new IdleState());
        _stateMachine.Register(new MoveState());

        // 2. 초기 상태 설정
        _stateMachine.ChangeState<IdleState>();
    }

    private void Update()
    {
        // 3. 현재 상태 업데이트 실행
        _stateMachine.Update();

        // 예시: 입력에 따른 상태 변경
        if (Input.GetKeyDown(KeyCode.W))
        {
            _stateMachine.ChangeState<MoveState>();
        }
    }
}
```

<br>

**IState**

상태머신이 관리할 상태, 상속을 통한 상태 구현

```csharp
public class IdleState : IState
{
    public void OnEnter() => Debug.Log("대기 상태 진입");
    public void Update() { /* 대기 로직 */ }
    public void OnExit() => Debug.Log("대기 상태 종료");
}

public class MoveState : IState
{
    public void OnEnter() => Debug.Log("이동 상태 진입");
    public void Update() { /* 이동 로직 */ }
    public void OnExit() => Debug.Log("이동 상태 종료");
}

```


<br>
<br>

---

### Services

간편한 참조와 의존성 주입을 위한 서비스 로케이터, 기본적으로 싱글톤 적 성격을 띄고 있으므로 Monobehaviour 객체는 등록시 DontDestroyOnLoad 처리되며 중복된 객체가 등록되거나 등록 해제시 파괴됨.

<table>
  <tr>
    <td><b>Register(T service)<b></td>
    <td>서비스 등록</td>
  </tr>
  <tr>
    <td><b>Unregister()<b></td>
    <td>서비스 등록 해제</td>
  </tr>
  <tr>
    <td><b>Get<b></td>
    <td>서비스 조회</td>
  </tr>
</table>


```csharp
Services.Register<IGameSettings>(this);
IGameSettings Settings => Services.Get<IGameSettings>();
```

<br>
<br>

---

### Input

**PointerManager**

InputSystem 을 통해 메인 클릭에 대한 처리를 담당.

<table>
  <tr>
    <td><b>PosInputReference<b></td>
    <td>포인터의 위치를 나타낼 InputActionReference</td>
  </tr>
  <tr>
    <td><b>ClickInputReference<b></td>
    <td>포인터의 입력을 나타낼 InputActionReference</td>
  </tr>
  <tr>
    <td><b>IsPressed</td>
    <td>포인터가 눌려있는지 여부</td>
  </tr>
      <tr>
    <td><b>ScreenPosition</td>
    <td>스크린 기준 포인터 위치</td>
  </tr>
      <tr>
    <td><b>WorldPosition</td>
    <td>월드 기준 포인터 위치</td>
  </tr>
       <tr>
    <td><b>GetHitCollider()</td>
    <td>레이캐스트를 통한 포인터 위치 콜라이더 검출</td>
  </tr>
      <tr>
    <td><b>SetCamera()</td>
    <td>수동 카메라 등록 필요시 등록</td>
  </tr>
</table>


<br>

**PointerDownEvent & UpEvent**

<table>
  <tr>
    <td><b>ScreenPosition<b></td>
    <td>이벤트 발생 기준 포인터의 스크린 위치</td>
  </tr>
  <tr>
    <td><b>WorldPosition<b></td>
    <td>이벤트 발생 기준 포인터의 월드 위치</td>
  </tr>
  <tr>
    <td><b>HitCollider</td>
    <td>이벤트 발생 기준 검출된 포인터 위치의 콜라이더</td>
  </tr>
</table>

<br>

```csharp

EventBus<PointerDownEvent>.Subscribe(OnPointerDown);
EventBus<PointerDownEvent>.Unsubscribe(OnPointerDown);

private void OnPointerDown(PointerDownEvent evt)
{
    // 1. 클릭된 위치의 콜라이더 확인
    if (evt.HitCollider != null)
    {
        Debug.Log($"클릭된 오브젝트: {evt.HitCollider.name}");
        Debug.Log($"월드 좌표: {evt.WorldPosition}");
    }
    else
    {
        Debug.Log("허공을 클릭했습니다.");
    }
}

```


<br>
<br>

---

### PreInitialize

**PreInitializer** 

<img width="472" height="270" alt="image" src="https://github.com/user-attachments/assets/f53b06e9-a2cf-4df8-ab0a-16bab73839d6" />

플레이시 `Preload Objects` 들을 생성하고 `Use Pre Initialize` 체크 시 사전 초기화를 진행. `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]` 시점에 병렬식 비동기로 진행되므로 씬에 미리 배치되어있던 오브젝트의 Awake등에서 호출 주의


<table>
  <tr>
    <td><b>IsInitialized<b></td>
    <td>초기화가 완료되었는지 확인</td>
  </tr>
  <tr>
    <td><b>InitProgress<b></td>
    <td>[완료 목록 개수 / 총 목록 개수] 의 진행률</td>
  </tr>
  <tr>
    <td><b>WaitInitAsync()</td>
    <td>외부에서 해당 초기화가 완료될때까지 비동기로 기다리는 UniTask</td></td>
  </tr>
</table>

```cs

public async void Awake()
{
    // 사전 초기화가 필요한 클래스를 참조할 경우 대기
    await PreInitializer.WaitInitAsync();

    //이후 참조
}

```

<br>

**IPreInitializable**

사전 초기화를 진행 할 대상. `UniTask<bool> InitAsync(PreInitContext ctx);` 매서드를 통해 초기화를 진행하고 결과를 bool 로 리턴. 만약 초기화중 참조해야 할 대상 또한 초기화 대상이라면 `context` 의 `GetAsync()` 를 호출하여 대상의 초기화를 기다린 후 진행 가능. 

```cs

```

 protected override async UniTask<bool> OnInitAsync(PreInitContext ctx)
{
    _resourceMgr = Services.Get<IResourceManager>();
    
    var resourceInit = _resourceMgr as IPreInitializable;
    if (resourceInit == null)
        return false;
    
    var result = await ctx.GetAsync(resourceInit);
    if (result == null) 
        return false;
}

<br>
<br>

---



## ℹ️사전 작업

![Preload](https://github.com/user-attachments/assets/5bb381a1-24b1-407c-8f56-ebd1e4dc6224)
![GetAsyncManager](https://github.com/user-attachments/assets/95862b4c-4cd2-432b-b358-bad5d98c0cf4)

> [!NOTE]
> 컴파일시 Resources 에 자동 생성 혹은 직접 생성한 LumosLibSetting 을 통해 <br>
> 사전 생성할 오브젝트를 추가, 초기화 할 수 있습니다. <br>
> Use 체크를 통해 사전 생성, 초기화를 실행 할 지 선택 할 수 있으며, <br>
> 어느 씬에서든지 런타임시 비동기적으로 사전 초기화를 진행합니다.

<br>

