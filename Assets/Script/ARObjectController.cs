using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectController : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>(); 
    private Animator arAni;
    private Touch touch;
    private Vector2 fingerPosition;
    private float touchDuration = 0f;
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    public GameObject arObject;
    public GameObject ballUI;
    public TextMeshProUGUI textUI;
    public bool isTouch = false;

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;
        else {
            touch = Input.GetTouch(0);
        }

        if (!arObject) {
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                Vector3 hitRotation = hitPose.rotation.eulerAngles - new Vector3(0, 180f, 0);
                arObject = Instantiate(GameManager.instance.arObject[GameManager.instance.arObjIndex], hitPose.position, Quaternion.Euler(hitRotation));
                ballUI.SetActive(true);
                textUI.text = "";
            }
        }
        else {
            GetComponent<ARObjectAction>().isMove = false;
            arAni = arObject.GetComponent<Animator>();
            arAni.SetBool("Hi", false);
            arAni.SetBool("Walk", false);
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == arObject) 
                {
                    isTouch = true;
                }
            }
            if (isTouch) {
                if (touch.tapCount == 1) {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            fingerPosition = touch.position;
                            break;
                        case TouchPhase.Stationary:
                            touchDuration += Time.deltaTime;
                            if (touchDuration >= 0.5f)
                            {
                                arAni.SetBool("Falling", true);
                            }
                            break;

                        case TouchPhase.Moved:
                            if (touchDuration >= 0.5f)
                            {
                                if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                                {
                                    var hitPose = hits[0].pose;
                                    arObject.transform.position = hitPose.position;
                                    arObject.transform.rotation = Quaternion.Euler(hitPose.rotation.eulerAngles - new Vector3(0, 180f, 0));
                                }
                            }
                            break;

                        case TouchPhase.Ended:
                            if (touchDuration < 0.5f) {
                                float swipeDistanceX = Mathf.Abs(fingerPosition.x - touch.position.x);
                                float swipeDistanceY = Mathf.Abs(fingerPosition.y - touch.position.y);
                                if (swipeDistanceY > 200f && swipeDistanceY > swipeDistanceX && fingerPosition.y - touch.position.y < 0)
                                {
                                    arAni.SetTrigger("Jump");
                                    GetComponent<ARObjectAction>().aniDuration = 0;
                                }
                            }
                            touchDuration = 0f;
                            isTouch = false;
                            arAni.SetBool("Falling", false);
                            break;
                    }
                }
                if (touch.tapCount >= 2) {
                    arAni.Play("Dance");
                    GetComponent<ARObjectAction>().aniDuration = 0;
                    isTouch = false;
                }

            }
        }
    }
}
