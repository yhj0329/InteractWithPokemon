# Interaction Pokemon

AR 환경에서 Pokemon과 상호작용해보세요!  
사용자는 포켓몬을 소환하고 소환 해제할 수 있습니다.  
또한 포켓몬에게 점프와 춤추기를 시킬 수 있어요!  
포켓몬은 스스로 돌아다니고 인사합니다. 물론 박수도 치고 응원도 해요!

설치 파일 : 

## 사용법
카메라 권한을 반드시 허용해야 합니다!
1. 포켓몬을 선택합니다.
2. AR Plane을 찾고 터치하여 포켓몬을 소환합니다.
3. 터치를 통해 상호작용합니다.  
\- 연속 두번 클릭 : 능력 사용과 춤추기 명령  
\- 위로 슬라이드 : 점프 명령  
\- 1초 이상 터치 : 포켓몬 강제 이동
4. 터치를 하지 않으면 스스로 활동합니다.  
\- 이동, 인사, 박수, 응원
5. 포켓몬 볼을 던져 포켓몬을 소환 해제합니다.
6. 1번 반복

## 시연 영상

#### 전체 영상
- 피카츄

- 파이리

#### 부분 영상
- 포켓몬 선택

- AR Plane 찾고 포켓몬 소환

- 포켓몬 소환 해제

- Occlusion 적용

- 터치를 통한 상호작용 (연속 두번 클릭)  
**객체를 정확히 터치해야 함**

- 터치를 통한 상호작용 (위로 슬라이드)  
**객체를 정확히 터치해야 함**

- 터치를 통한 상호작용 (1초 이상 터치)  
**객체를 정확히 터치해야 함**

- 포켓몬 활동(이동)

- 포켓몬 활동(인사)

- 포켓몬 활동(박수)

- 포켓몬 활동(응원)

- 음량 조절


## 중요 기능 설명

### AR Plane 탐색
다음 방식을 통해 AR 평면을 모바일에서 탐색하여 Prefabs를 만들 수 있다.

1. Unity Hierarchy 창에서 우클릭 한다.
![img.png](img.png)
2. XR 탭에 들어간다
3. AR Session, AR Default Plane, XR Origin을 클릭해서 GameObject를 추가한다.
4. XR Origin GameObject에 AR Plane Manager를 Add Component를 이용해서 추가한다.
![img_1.png](img_1.png)
5. 빌드해서 확인한다.

### AR Object 생성

다음 방식을 통해 화면에 있는 AR Plane을 터치시 AR Object를 생성할 수 있다.

1. XR Origin에 AR Raycast Manager를 Add Component를 이용해서 추가한다.  
(Raycaset Prefab은 상관 없다. Object instance를 생성할때  
"GetComponent&lt;ARRaycastManager&gt;().raycastPrefab"을 통해  
불러와서 사용할 수 있지만 본 프로젝트에서는 사용하지 않는다.)   
![img_2.png](img_2.png)
   
2. ARObjectController.cs를 Component에 추가한다.
   ARObjectController.cs
   ```cs
   ...
   void Update()
   {
       if (Input.touchCount == 0) return; // 터치가 없다면 해당 method를 실행하지 않는다.
       else {
           touch = Input.GetTouch(0);
       }
    
       if (!arObject) { // arObject가 하나 존재하면 arObject를 더이상 생성하지 않는다.
           if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
           { // arRay가 닿는 평면의 위치를 hits에 저장한다.
               var hitPose = hits[0].pose; // hits에서 가장 먼저 만난 좌표를 저장한다.
               Vector3 hitRotation = hitPose.rotation.eulerAngles - new Vector3(0, 180f, 0); 
               // 생성된 오브젝트가 카메라를 바라보도록 세팅한다.
               arObject = Instantiate(GameManager.instance.arObject[GameManager.instance.arObjIndex], hitPose.position, Quaternion.Euler(hitRotation));
               // GameManger.cs를 싱글톤으로 설정하여 그 안에 저장되어 있는 prefeb을 instance로 가져와 소환한다.
               SoundManager.instance.PlaySummonSound(GameManager.instance.arObjIndex);
               ballUI.SetActive(true);
               textUI.text = "";
           }
       }
       else 
       { ... }
   }
   ...
   ```

### Occlusion 적용

다음 방식을 통해 AR Object가 다른 환경에 의해 가려지는 효과를 생성할 수 있다.

1. XR Origin &gt; Camera Offset &gt; Main Camera에 AR Occlusion Manager를 Add Component를 이용해 추가한다.  
   (Human도 설정할 수 있지만 해당 기능은 ARKit(iphone)만 가능한 기능이며 ARCore(Android)는 지원하지 않는다.)
![img_3.png](img_3.png)

### AR Object 정확한 터치 감지

ARObjectController.cs의 다음 부분을 통해 ARObject와 상호작용할 때,  
정확히 생성된 ARObject를 클릭했을 때만 상호작용하고 다른 곳을 클릭했을 때는 상호작용하지 않는 기능을 구현한다.
```cs
...
void Update() 
{ 
    ...
    if (!arObject) { ... }
    else
    {
        GetComponent<ARObjectAction>().isMove = false;
        arAni = arObject.GetComponent<Animator>();
        arAni.SetBool("Hi", false);
        arAni.SetBool("Walk", false);
        Ray ray = arCamera.ScreenPointToRay(touch.position);
        // touch한 화면의 좌표로 부터 스크린에 광선을 뿌린다.
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // 뿌린 광선이 Unity 객체에 닿으면 True를 반환한다.
        {
            if (hit.collider.gameObject == arObject) // 닿은 객체가 arObject면 isTouch를 참으로 값을 할당한다.
            {
                isTouch = true;
            }
        }
        if (isTouch) { ... } // 정확히 arObject를 터치했다면 해당 기능을 수행한다.
    }
}
...
```

### UI 클릭시 다른 기능 사용 방지

GameMananger.cs에서 bool 변수를 활용해 다른 기능 Script가 작동하지 않도록 한다.  
(ARObjectAction.cs는 arObject가 존재해야지만 작동하기에 다른 bool 변수가 필요하지 않다.)

GameManager.cs
```cs
public static GameManager instance; // 싱글톤 변수(전역변수 선언)
public bool isRemove = false; // ARObjectRemover.cs의 작동을 막는 변수
public bool isSelect = true; // ARObjectController.cs의 작동을 막는 변수
public int arObjIndex = 0; // 선택한 포켓몬의 Index를 저장하는 변수
public GameObject[] arObject; // 선택할 포켓몬의 Prefab을 저장하는 변수
public GameObject selectUI;
public TextMeshProUGUI textUI;

void Awake() // 싱글톤 선언
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
```

ARObjectController.cs
```cs
...
void Update()
{
    if (GameManager.instance.isSelect) return;
    ...
}
...
```
ARObjectRemover.cs
```cs
...
void Update()
{
    if (GameManager.instance.isRemove) { ... }
}
```

## 참고 자료
[unity AR 기본 개념](https://vrworld.tistory.com/2)

[터치한 곳에 AR Object 생성하기](https://velog.io/@mings-k/Unity-AR-Raycast-%EC%82%AC%EC%9A%A9%ED%95%98%EA%B8%B0)