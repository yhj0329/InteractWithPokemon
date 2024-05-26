using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>(); 

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0) return;

        if (arRaycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            Vector3 hitRotation = hitPose.rotation.eulerAngles - new Vector3(0, 180f, 0);
            Instantiate(arRaycastManager.raycastPrefab, hitPose.position, Quaternion.Euler(hitRotation));
        }
    }
}
