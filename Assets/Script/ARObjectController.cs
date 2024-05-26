using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectController : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>(); 

    private GameObject arObject;
    private Touch touch;
    private float touchDuration = 0f;
    private bool isTouch = false;

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
                arObject = Instantiate(arRaycastManager.raycastPrefab, hitPose.position, Quaternion.Euler(hitRotation));
            }
        }
        else {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == arObject) 
                {
                    isTouch = true;
                }
            }
            switch (touch.phase) {
                case TouchPhase.Began:
                    
                    break;

                case TouchPhase.Stationary:
                    if (isTouch) 
                    {
                        touchDuration += Time.deltaTime;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isTouch) 
                    {
                        touchDuration += Time.deltaTime;

                        if (touchDuration >= 1f)
                        {
                            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                            {
                                var hitPose = hits[0].pose;
                                arObject.transform.position = hitPose.position;
                                arObject.transform.rotation = Quaternion.Euler(hitPose.rotation.eulerAngles - new Vector3(0, 180f, 0));
                            }
                        }
                    }
                    
                    break;

                case TouchPhase.Ended:
                    touchDuration = 0f;
                    isTouch = false;
                    break;
            }
        }
    }
}
