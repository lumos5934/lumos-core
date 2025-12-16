# ✨lumos-library
유니티 기반 & 에셋에서 자주 사용되는 요소, <br>
또는 확장하는 기능의 스크립트를 모아놓아 개발의 생산성 상승을 위한 라이브러리

<br>
<br>

## ℹ️사전 작업

![Scene](https://github.com/user-attachments/assets/e5b8cb62-61d4-415b-b8ae-fc7da5f223cd)
![Create](https://github.com/user-attachments/assets/f657c69e-0a23-4469-bd71-285c691891e9)

> [!NOTE]
> 어느 씬에서든 BaseSceneManager 를 상속받는 컴포넌트가 씬에 하나라도 있으면 씬이 로드 된 후 전체적인 사전 비동기 초기화를 진행하고, <br>
이후 해당 SceneManager 의 OnInitAsync 구현을 통해 씬에서의 초기화 & 동작들을 수행 할 수 있습니다. <br>
이로 인해 Awake() 와 Start() 에서의 실행 순서는 보장 받을 수 없습니다. <br><br>
또한 에셋메뉴와 하이어라키창 우클릭을 통해 사전에 준비된 프리팹 , SO , Script 들을 생성 할 수 있습니다.



<br>
<br>

## ℹ️기능

### Core
* [ Project Config ](https://www.notion.so/PreInitialize-2a846d59656180cf9235d2e1f2633f0c?source=copy_link)
* [ Pre-Initialize ](https://www.notion.so/Pre-Initialize-2aa46d596561807bb6dde336babbb7bd?source=copy_link)
* [ Global ](https://www.notion.so/Global-2a846d596561803f8199c6cdb734fbdc?source=copy_link)
* [ Extension ](https://www.notion.so/Extension-2a846d596561800ea5ccf89d2192834e?source=copy_link)
* [ Resource ](https://www.notion.so/Resource-2a846d5965618067a36dee009c02630c?source=copy_link)
* [ UI ](https://www.notion.so/UI-2a846d5965618054a5b0dffe119a4b10?source=copy_link)
* [ Pool ](https://www.notion.so/Pool-2a846d59656180b4bff8fb4bcab34394?source=copy_link)
* [ FSM ](https://www.notion.so/FSM-2a846d596561803eaaadd4d02fd67503?source=copy_link)
* [ Audio ](https://www.notion.so/Audio-2a846d596561808cbc56d22734de8ead?source=copy_link)
* [ Data ](https://www.notion.so/Data-2a846d596561802ca1f7fa96503e7c5e?source=copy_link)
* [ TestEditor ](https://www.notion.so/Test-Editor-2a846d596561801bb3abd2e4f4f57ebf?source=copy_link)
* [ Event ](https://www.notion.so/Event-2b346d59656180918aeddf5a20c0d350?source=copy_link)
* [ Input ](https://www.notion.so/Input-2b346d59656180cf9248c92e8c5d465a?source=copy_link)
* [ Tutorial ](https://www.notion.so/Tutorial-2cb46d5965618079835ae312959c3118?source=copy_link)

### DOTween
* [ Tween Preset ](https://www.notion.so/Tween-Preset-2aa46d596561805a81f0d38f690ea271?source=copy_link)


<br>
<br>

## ℹ️의존성

- **Newtonsoft.Json (자동 설치)**
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676?locale=ko-KR&srsltid=AfmBOorERKqVIWHnYtX-Ib3WRvr0KqT-k48G_H4SXEk8uU_Eczqg4U9v)
- [Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector?tab=readme-ov-file)

<br>
<br>
