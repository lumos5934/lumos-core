# ✨lumos-library
유니티 기반 & 에셋에서 자주 사용되는 요소, <br>
또는 확장하는 기능의 스크립트를 모아놓아 개발의 생산성 상승을 위한 라이브러리

<br>
<br>

## ℹ️사전 세팅

![Scene](https://github.com/user-attachments/assets/e5b8cb62-61d4-415b-b8ae-fc7da5f223cd)
> [!NOTE]
> 어느 씬에서든 테스트를 용이하게 하기 위해 BaseSceneManager 를 상속받는 컴포넌트가 씬에 하나라도 있으면 씬이 로드 된 후 전체적인 사전 비동기 초기화를 진행하고 이후 해당 SceneManager 의 OnInitAsync 구현을 통해 씬에서의 초기화 & 동작들을 수행 할 수 있습니다. 그리고 싱글톤으로 구현되어 씬마다 하나의 SceneManager 를 보장 받습니다. 이로 인해 Awake() 와 Start() 에서의 외부 접근은 보장 받을 수 없습니다.



<br>
<br>

## ℹ️기능


* [ Project Config ](https://www.notion.so/PreInitialize-2a846d59656180cf9235d2e1f2633f0c?source=copy_link)
* [ Pre-Initialize ](https://www.notion.so/Pre-Initialize-2aa46d596561807bb6dde336babbb7bd?source=copy_link)
* [ UI ](https://www.notion.so/UI-2a846d5965618054a5b0dffe119a4b10?source=copy_link)
* [ Scene ](https://www.notion.so/Scene-2a846d59656180e1a447d8674ab3b8d2?source=copy_link)
* [ Global ](https://www.notion.so/Global-2a846d596561803f8199c6cdb734fbdc?source=copy_link)
* [ TestEditor ](https://www.notion.so/Test-Editor-2a846d596561801bb3abd2e4f4f57ebf?source=copy_link)
* [ Data ](https://www.notion.so/Data-2a846d596561802ca1f7fa96503e7c5e?source=copy_link)
* [ Audio ](https://www.notion.so/Audio-2a846d596561808cbc56d22734de8ead?source=copy_link)
* [ Resource ](https://www.notion.so/Resource-2a846d5965618067a36dee009c02630c?source=copy_link)
* [ Pool ](https://www.notion.so/Pool-2a846d59656180b4bff8fb4bcab34394?source=copy_link)
* [ FSM ](https://www.notion.so/FSM-2a846d596561803eaaadd4d02fd67503?source=copy_link)
* [ Extension ](https://www.notion.so/Extension-2a846d596561800ea5ccf89d2192834e?source=copy_link)


<br>
<br>

## ℹ️의존성

- **Newtonsoft.Json (자동 설치)**
- **DOTween (https://dotween.demigiant.com/)**
- **Tri-Inspector (https://github.com/codewriter-packages/Tri-Inspector?tab=readme-ov-file)**

<br>
<br>
