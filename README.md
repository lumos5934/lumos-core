# ✨lumos-library
유니티 기반 & 에셋에서 자주 사용되는 요소, 또는 확장하는 기능의 스크립트를 모아놓아 개발의 생산성 상승을 위한 라이브러리

<br>

[ 기능 설명서 ](https://www.notion.so/Lumos-Library-2a846d59656180d583b9c73691d6e60f?source=copy_link)

**의존성**
- **Newtonsoft.Json (자동 설치)**
- **DOTween (https://dotween.demigiant.com/)**

<br>

## ℹ️사전 세팅

  ![사전 세팅](https://github.com/user-attachments/assets/02cd775d-2e2f-4728-891b-ab41895a1bd7)
  
* 어느 씬에서나 SceneManager.cs 배치
* Assets > Resources > PreInitialize Config (기본 자동 참조)
* 이후 SceneManager 의 Init 을 통해 해당 씬에 대한 동작 수행
